﻿using Amazon;
using Amazon.Glacier.Transfer;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using System.Runtime.Serialization.Json;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS.Model;
using Amazon.Glacier;
using Amazon.Glacier.Model;
using System.Text;
using System.Web.Script.Serialization;

namespace Backup.Logic
{
    public class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";
        public const string SettingsName = "bbackup.settings.bin";
        public const string ArchiveFolder = "Archive"; // Default folder in the case of the user not setting one

        // App.config settings keys
        public const string TempDirKey = "tempDir"; // The location of SettingsFolder
        // ...others as req.

        // Files for persistant data
        public const string DiscoveredFileName = "bbackup.disco.dat";
        public const string ProcessingFileName = "bbackup.processing.dat";
        public const string CatalogFileName = "bbackup.catalog.dat";
        public const string DestinationsFileName = "bbackup.destinations.dat";
        public const string InventoryFileName = "glacier.inventory.json";
        public const string InventoryTopicFileName = "glacier.inventory.topic";
        public const string ArchiveTopicFileName = "glacier.archive_#.topic";
        public const string InventoryModelFileName = "bback.inventory.model";

        // The list of target directories
        public const string SourcesFileName = "bbackup.sources.dat";

        #region events
        public static event BackupSuccessHandler BackupSuccess;
        public delegate void BackupSuccessHandler(string successMessage);
        public static event BackupWarningHandler BackupWarning;
        public delegate void BackupWarningHandler(string warningMessage);
        public static event BackupErrorHandler BackupError;
        public delegate void BackupErrorHandler(string errorMessage);

        public static event DownloadSuccessHandler DownloadSuccess;
        public delegate void DownloadSuccessHandler(string successMessage);
        public static event DownloadWarningHandler DownloadWarning;
        public delegate void DownloadWarningHandler(string warningMessage);
        public static event DownloadErrorHandler DownloadError;
        public delegate void DownloadErrorHandler(string errorMessage);
        #endregion
        #region SQS Policy
        const string SQS_POLICY =
            @"{" +
            "    \"Version\" : \"2012-10-17\"," +
            "    \"Statement\" : [" +
            "        {" +
            "            \"Sid\" : \"sns-rule\"," +
            "            \"Effect\" : \"Allow\"," +
            "            \"Principal\" : \"*\"," +
            "            \"Action\"    : \"sqs:SendMessage\"," +
            "            \"Resource\"  : \"{QuernArn}\"," +
            "            \"Condition\" : {" +
            "                \"ArnLike\" : {" +
            "                    \"aws:SourceArn\" : \"{TopicArn}\"" +
            "                }" +
            "            }" +
            "        }" +
            "    ]" +
            "}";
        #endregion

        static bool isCancelling = false;
        static public void Cancel()
        {
            isCancelling = true;
        }

        /// <summary>
        /// Get the configured temp directory from application settings or, 
        /// if not defined create a default under user's Application Data
        /// </summary>
        static public string GetTempDirectory()
        {
            var tempDir = ConfigurationManager.AppSettings[TempDirKey];
            if (string.IsNullOrEmpty(tempDir))
            {
                tempDir = Directory.GetParent(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    tempDir = Directory.GetParent(tempDir).ToString();
                }

                // Default to our folder if nothing set
                tempDir = Path.Combine(tempDir, SettingsFolder);
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }

        public static void SaveSettings(Settings settings)
        {
            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(GetTempDirectory(), SettingsName);
            File.Delete(settingsFileName);
            var stream = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, settings);
            stream.Close();
        }

        public static Settings GetSettings()
        {
            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(GetTempDirectory(), SettingsName);
            if (!File.Exists(settingsFileName))
                SaveSettings(new Settings());
            var stream = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (Settings)formatter.Deserialize(stream);
            stream.Close();
            return settings;
        }

        public static Topic GetExistingTopic(string topicName)
        {
            var file = Path.Combine(GetTempDirectory(), topicName);
            if (File.Exists(file))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                var topic = (Topic)formatter.Deserialize(stream);
                stream.Close();

                // Update status
                //topic.Status = ProcessTopic(topic, outputFile);
                Debug.WriteLine($"Found topic file: {topic.TopicFileName} {topic.Status}");
                return topic;
            }
            return null;
        }

        private static void SaveTopicFile(Topic topic)
        {
            var formatter = new BinaryFormatter();
            var topicFile = Path.Combine(GetTempDirectory(), topic.TopicFileName);
            File.Delete(topicFile);
            var stream = new FileStream(topicFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, topic);
            stream.Close();
            Debug.WriteLine("Saved topic file: " + topic.TopicFileName);
        }

        static public List<Source> GetSources()
        {
            CreateSourcesFile();
            var lines = File.ReadAllLines(Path.Combine(GetTempDirectory(), SourcesFileName)).Skip(1);
            return new List<Source>(
                (from l in lines
                 select new Source(
                    path: l.Split(',')[0].Trim(),
                    pattern: l.Split(',')[1].Trim(),
                    modifiedOnly: l.Split(',')[2].Trim(),
                    lastBackup: DateTime.Parse(l.Split(',')[3].Trim()))
                 ));
        }

        static public void SaveSources(List<Source> sources)
        {
            File.Delete(Path.Combine(GetTempDirectory(), SourcesFileName));
            CreateSourcesFile(sources);
        }

        /// <summary>
        /// Get the objects (keys) in the S3 Bucket
        /// </summary>
        static public async Task<IEnumerable<string>> GetBucketContentsAsync()
        {
            var settings = GetSettings();

            AmazonS3Client client = new AmazonS3Client(
                settings.AWSAccessKeyID, 
                settings.AWSSecretAccessKey, 
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            // Issue call
            //ListBucketsResponse response = client.ListBuckets();
            ListObjectsV2Request objRequest = new ListObjectsV2Request
            {
                BucketName = settings.AWSS3Bucket
            };
            ListObjectsV2Response objResponse = await client.ListObjectsV2Async(objRequest);
            return objResponse.S3Objects.Select(o => o.Key);
        }

        public static async Task DownloadS3ObjectAsync(string downloadDir, string objectKey)
        {
            try
            {
                var settings = GetSettings();
                var downloadInfo = new DownloadObjectInfo { DownloadDirectory = downloadDir, ObjectKey = objectKey };
                AmazonS3Client client = new AmazonS3Client(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                var request = new GetObjectRequest { BucketName = settings.AWSS3Bucket, Key = objectKey};
                var response = await client.GetObjectAsync(request);
                await SaveObjectAsync(response, downloadInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DownloadError?.Invoke("Error during download");
                throw ex;
            }
        }

        private static async Task SaveObjectAsync(GetObjectResponse response, DownloadObjectInfo info)
        {
            response.WriteObjectProgressEvent += Response_WriteObjectProgressEvent;
            string checkedFilePath = CheckFilePath(info);

            await response.WriteResponseStreamToFileAsync(checkedFilePath, false, new CancellationToken());
            DownloadSuccess?.Invoke($"Backup downloaded {info.ObjectKey} to {Path.Combine(info.DownloadDirectory, checkedFilePath)}");
        }

        public static string GetArchiveTopicFileName(string archiveId)
        {
            return ArchiveTopicFileName.Replace("#", archiveId.Substring(archiveId.Length-15, 15).ToString());
        }

        /// <summary>
        /// Issue a request to get Glacier inventory or process existing request
        /// </summary>
        public static GlacierResult GetGlacierInventory(bool createNewJob)
        {
            #region pseudocode
            // Is there an inventory file?
            // Yes: return Inventory and issue new request
            // Not found:
            //   Is an inventory request issued?
            //      Yes: get request details
            //      No: Issue request and save SNS topic, Job ID, Queue URL
            // Poll topic for response
            // Poll Message received?
            //    Yes: serialize inventory to file and return
            //    No: return details of inventory request (in progress)
            #endregion
            try
            {
                Topic topic = GetExistingTopic(InventoryTopicFileName);
                if (topic == null && createNewJob)
                {
                    // Issue new request and serialize topic details to global file
                    topic = SetupTopicAndSubscriptions(InventoryTopicFileName, GetTempDirectory(), null, InventoryFileName);
                    topic.Type = "inventory-retrieval";
                    topic.Description = "This job is to download a vault inventory";
                    InitiateGlacierJob(topic);
                    return topic.Status; // only requested - no need to process yet
                }
                if (!createNewJob)
                    return GlacierResult.NoJob;
                
                var result = ProcessQueue(topic);
                Debug.WriteLine("ProcessQueue result: " + result);
                if (result == GlacierResult.Completed)
                {
                    DownloadSuccess?.Invoke("Glacier Inventory was updated");
                }

                return topic.Status;
            }
            catch (Exception ex)
            {
                BackupError?.Invoke(ex.Message);
                return GlacierResult.Error;
            }
        }

        /// <summary>
        /// Get a Glacier archive
        /// </summary>
        public static GlacierResult RequestGlacierArchive(string archiveId, string downloadDirectory, string filename)
        {
            #region pseudocode
            // Is there an archive request issued (check for file)?
            // Yes: Poll queue for response
            // No: create a request and persist details to file
            // Poll topic 
            // Message received?
            // Yes: serialise job to filePath
            // No: NOP
            #endregion
            try
            {
                string topicFileName = GetArchiveTopicFileName(archiveId);
                Topic topic = GetExistingTopic(topicFileName);
                if (topic == null)
                {
                    // Issue new request and serialize topic details to file
                    topic = SetupTopicAndSubscriptions(topicFileName, downloadDirectory, archiveId, filename);
                    topic.Type = "archive-retrieval";
                    topic.Description = "This job is to download a vault archive";
                    topic.ArchiveId = archiveId;
                    InitiateGlacierJob(topic);
                }
                //ProcessQueue(topic);
                //if (topic.Status == GlacierResult.Completed)
                //    DownloadSuccess?.Invoke($"Backup downloaded {topic.OutputPath}");

                return topic.Status;
            }
            catch (Exception ex)
            {
                BackupError?.Invoke(ex.Message);
                return GlacierResult.Error;
            }
        }

        /// <summary>
        /// Get any downloaded inventory file of the Glacier vault,
        /// build a view model with statuses of any Archive jobs and return
        /// </summary>
        public static ArchiveModelList GetArchiveModel()
        {
            // Look for downloaded inventory
            var inventoryFile = Path.Combine(GetTempDirectory(), InventoryFileName);
            if (File.Exists(inventoryFile))
            {
                // Found inventory, return model with the statuses of any related topics found
                using (var file = File.Open(inventoryFile, FileMode.Open))
                {
                    Debug.WriteLine("Getting Archive Model");
                    var s = new DataContractJsonSerializer(typeof(Inventory));
                    var inventory = (Inventory)s.ReadObject(file);
                    var dt = DateTime.Parse(inventory.InventoryDate);
                    var model = new ArchiveModelList { InventoryDate = dt.ToString("dd-MM-yyyy") };
                    foreach (var archive in inventory.ArchiveList)
                    {
                        var topicFile = GetArchiveTopicFileName(archive.ArchiveId);
                        var status = GetExistingTopic(topicFile)?.Status ?? GlacierResult.NoJob;
                        model.Add(new ArchiveModel
                        {
                            ArchiveId = archive.ArchiveId,
                            Description = archive.ArchiveDescription,
                            GlacierJobStatus = status,
                            Size = archive.Size,
                            ArchiveTopicFilePath = Path.Combine(GetTempDirectory(), topicFile)
                        });
                    }

                    // Delete topic files which are not related to the inventory
                    var topicFiles = Directory.GetFiles(
                        GetTempDirectory(), "glacier.archive*.topic", SearchOption.TopDirectoryOnly);
                    var toDelete = from t in topicFiles
                                   where !model.Exists(m => m.ArchiveTopicFilePath == t)
                                   select t;

                    toDelete.ToList().ForEach(t =>
                    {
                        File.Delete(Path.Combine(GetTempDirectory(), t));
                        Debug.WriteLine($"Deleted {t}");
                    });

                    // Update inventory model file
                    SaveInventoryModel(model);
                    return model;
                }
            }
            Debug.WriteLine("WARNING: no inventory model");
            return null;
        }

        public static void ProcessArchiveModel()
        {
            var model = GetArchiveModel();
            if (model != null)
            {
                foreach (var m in model)
                {
                    if (isCancelling)
                        break;

                    if (m.ArchiveTopicFilePath != null)
                    {
                        var topicFile = GetArchiveTopicFileName(m.ArchiveId);
                        var topic = GetExistingTopic(topicFile);
                        if (topic != null)
                        {
                            try
                            {
                                var result = ProcessQueue(topic);
                                //if (result == GlacierResult.Completed)
                                //    DownloadSuccess?.Invoke($"Glacier archive was downloaded to {topic.GetOutputFile()}");
                            }
                            catch (Exception ex)
                            {
                                DownloadError?.Invoke("An error occurred processing a Glacier job. " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private static void SaveInventoryModel(ArchiveModelList model)
        {
            var formatter = new BinaryFormatter();
            var inventoryModelFileName = Path.Combine(GetTempDirectory(), InventoryModelFileName);
            File.Delete(inventoryModelFileName);
            var stream = new FileStream(inventoryModelFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, model);
            stream.Close();
            Debug.WriteLine("Saved inventory model");
        }

        public static GlacierResult ProcessQueue(Topic topic)
        {
            // Check for notifications on topic and process any message
            try
            {
                var settings = GetSettings();
                using (var client = new AmazonGlacierClient(
                            settings.AWSAccessKeyID,
                            settings.AWSSecretAccessKey,
                            RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
                {
                    var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = topic.QueueUrl, MaxNumberOfMessages = 1 };
                    var sqsClient = new AmazonSQSClient(settings.AWSAccessKeyID, settings.AWSSecretAccessKey, RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));
                    var receiveMessageResponse = sqsClient.ReceiveMessage(receiveMessageRequest);
                    if (receiveMessageResponse.Messages.Count == 0)
                    {
                        topic.Status = GlacierResult.Incomplete;
                        SaveTopicFile(topic);
                        return topic.Status;
                    }

                    // Process message
                    string status = GetResponseStatus(receiveMessageResponse);
                    if (string.Equals(status, GlacierUtils.JOB_STATUS_SUCCEEDED, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DownloadGlacierJobOutput(topic.JobId, client, settings.AWSGlacierVault, topic.GetOutputFile());
                        Debug.WriteLine($"Downloaded job output to {topic.GetOutputFile()}");
                        if (topic.ArchiveId != null)
                            DownloadSuccess?.Invoke($"Glacier archive was downloaded to {topic.GetOutputFile()}");
                        DeleteTopic(topic);
                        return GlacierResult.Completed;
                    }
                    else if (string.Equals(status, GlacierUtils.JOB_STATUS_FAILED, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DownloadError?.Invoke("Job failed, cannot download the file");
                        DeleteTopic(topic);
                        return GlacierResult.JobFailed;
                    }
                    else if (string.Equals(status, GlacierUtils.JOB_STATUS_INPROGRESS, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DownloadWarning?.Invoke("Job in progress, Queue ARN: " + topic.QueueARN);
                        DeleteTopic(topic);
                        return GlacierResult.JobInProgress;
                    }
                    else
                    {
                        DeleteTopic(topic);
                        return GlacierResult.Error;
                    }
                }
            }
            catch (AmazonServiceException azex)
            {
                Debug.WriteLine("AmazonServiceException " + azex.Message);

                if (azex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Invalid credentials
                    BackupWarning?.Invoke("Invalid AWS credentials were provided while connecting");
                    return GlacierResult.Incomplete;
                }

                // Glacier ref: "A job ID will not expire for at least 24 hours after Amazon Glacier completes the job."
                DeleteTopic(topic);
                BackupWarning?.Invoke("A Glacier job has expired, a new job will be issued");
                // TODO reissue expired job
                InitiateGlacierJob(topic);
                return topic.Status;
                //throw azex;
            }
            catch (Exception ex)
            {
                DeleteTopic(topic);
                throw ex;
            }
        }

        private static void DeleteTopic(Topic topic)
        {
            var settings = GetSettings();
            var snsClient = new AmazonSimpleNotificationServiceClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            var sqsClient = new AmazonSQSClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            // Cleanup topic & queue & local file
            try { snsClient.DeleteTopic(new DeleteTopicRequest() { TopicArn = topic.TopicARN }); } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            try { sqsClient.DeleteQueue(new DeleteQueueRequest() { QueueUrl = topic.QueueUrl }); } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            
            // TODO Delete the errored/complete files on startup?
            File.Delete(Path.Combine(GetTempDirectory(), topic.TopicFileName));
            Debug.WriteLine($"Deleted topic {topic.TopicARN}");
            Debug.WriteLine($"Deleted topic file {topic.TopicFileName}");
        }

        private static string GetResponseStatus(ReceiveMessageResponse receiveMessageResponse)
        {
            Message message = receiveMessageResponse.Messages[0];
            var jss = new JavaScriptSerializer();
            var outer = jss.Deserialize<Dictionary<string, string>>(message.Body);
            var fields = jss.Deserialize<Dictionary<string, object>>(outer["Message"]);
            string status = fields["StatusCode"] as string;
            Debug.WriteLine("Message status: " + status);                 
            return status;
        }

        public static void DownloadGlacierJobOutput(
            string jobId, 
            AmazonGlacierClient client, 
            string vaultName, 
            string filePath)
        {
            try
            {
                var getJobOutputRequest = new GetJobOutputRequest
                {
                    JobId = jobId,
                    VaultName = vaultName
                };

                var getJobOutputResponse = client.GetJobOutput(getJobOutputRequest);
                using (Stream webStream = getJobOutputResponse.Body)
                {
                    using (Stream fileToSave = File.Create(filePath))
                    {
                        CopyStream(webStream, fileToSave);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[65536];
            int length;
            while ((length = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, length);
            }
        }

        private static void InitiateGlacierJob(Topic topic)
        {
            // Make the call to AWS Glacier to get inventory
            var settings = GetSettings();
            using (var client = new AmazonGlacierClient(
                        settings.AWSAccessKeyID,
                        settings.AWSSecretAccessKey,
                        RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
            {
                var initJobRequest = new InitiateJobRequest
                {
                    VaultName = settings.AWSGlacierVault,
                    JobParameters = new JobParameters
                    {
                        Type = topic.Type,
                        Description = topic.Description,
                        ArchiveId = topic.ArchiveId,
                        SNSTopic = topic.TopicARN,
                    }
                };

                var initJobResponse = client.InitiateJob(initJobRequest);
                topic.JobId = initJobResponse.JobId;
                topic.Status = GlacierResult.JobRequested;
                SaveTopicFile(topic);
            }
        }

        //private static void InitiateArchiveRetrievalOLD(Topic topic)
        //{
        //    // Make the call to AWS Glacier to get archive
        //    var settings = GetSettings();
        //    using (var client = new AmazonGlacierClient(
        //                settings.AWSAccessKeyID,
        //                settings.AWSSecretAccessKey,
        //                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
        //    {
        //        InitiateJobRequest initJobRequest = new InitiateJobRequest
        //        {
        //            VaultName = settings.AWSGlacierVault,
        //            JobParameters = new JobParameters
        //            {
        //                Type = "archive-retrieval",
        //                ArchiveId = topic.ArchiveId,
        //                SNSTopic = topic.TopicARN,
        //            }
        //        };

        //        //Debug.WriteLine("Job requested: " + initJobRequest.JobParameters.Type);
        //        InitiateJobResponse initJobResponse = client.InitiateJob(initJobRequest);
        //        topic.JobId = initJobResponse.JobId;
        //        topic.Status = GlacierResult.DownloadRequested;
        //        SaveTopicFile(topic);
        //    }
        //}

        private static Topic SetupTopicAndSubscriptions(string topicFileName, string outputDirectory, string archiveId = null, string filename = null)
        {
            var topic = new Topic {
                TopicFileName = topicFileName,
                OutputDirectory = outputDirectory,
                ArchiveId = archiveId,
                FileName = filename
            };
            long ticks = DateTime.Now.Ticks;
            var settings = GetSettings();

            #region Setup SNS topic
            var snsClient = new AmazonSimpleNotificationServiceClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            var sqsClient = new AmazonSQSClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            var topicArn = snsClient.CreateTopic(new CreateTopicRequest { Name = "GlacierDownload-" + ticks }).TopicArn;
            //Debug.WriteLine($"topicArn: {topicArn}");
            topic.TopicARN = topicArn;
            #endregion

            #region Setup SQS queue
            var createQueueRequest = new CreateQueueRequest { QueueName = "GlacierDownload-" + ticks };
            var createQueueResponse = sqsClient.CreateQueue(createQueueRequest);
            var queueUrl = createQueueResponse.QueueUrl;
            //Debug.WriteLine($"QueueURL: {queueUrl}");
            topic.QueueUrl = queueUrl;

            var getQueueAttributesRequest = new GetQueueAttributesRequest
            {
                AttributeNames = new List<string> { "QueueArn" },
                QueueUrl = queueUrl
            };
            var response = sqsClient.GetQueueAttributes(getQueueAttributesRequest);
            var queueArn = response.QueueARN;
            Debug.WriteLine($"QueueArn: {queueArn}");
            topic.QueueARN = queueArn;
            #endregion

            // Setup the Amazon SNS topic to publish to the SQS queue.
            // TODO SMS subscription
            snsClient.Subscribe(new SubscribeRequest()
            {
                Protocol = "sqs",
                Endpoint = queueArn,
                TopicArn = topicArn
            });

            // Add the policy to the queue so SNS can send messages to the queue.
            var policy = SQS_POLICY.Replace("{TopicArn}", topicArn).Replace("{QuernArn}", queueArn);

            sqsClient.SetQueueAttributes(new SetQueueAttributesRequest
            {
                QueueUrl = queueUrl,
                Attributes = new Dictionary<string, string>
                    {
                        { QueueAttributeName.Policy, policy }
                    }
            });

            return topic;
        }

        private static string CheckFilePath(DownloadObjectInfo info)
        {
            // start: does file already exist?
            //   yes: rename
            //      name contains '.'?
            //      yes:    split and rename with n
            //      no:     append n to name
            //      n++
            //      goto start
            //   no: download

            var file = info.ObjectKey;
            var splits = info.ObjectKey.Split('.');
            string baseName = "";
            for (int i = 0; i < splits.Count() - 1; i++)
                baseName += splits[i] + '.';
            baseName = baseName.TrimEnd('.');
            int n = 0;
            while (File.Exists(Path.Combine(info.DownloadDirectory, file)))
            {
                n++;
                if (splits.Count() > 1)
                    file = $"{baseName} ({n})." + splits[splits.Count() - 1];
                else
                    file = $"{info.ObjectKey} ({n})";
            }
            var checkedFilePath = Path.Combine(info.DownloadDirectory, file);
            return checkedFilePath;
        }

        private static void Response_WriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            Debug.WriteLine($"Percent done: {e.PercentDone}%");
        }

        /// <summary>
        /// Write the paths of files to backup to the disco file
        /// </summary>
        /// <param name="tempDir">Path to disco file</param>
        /// <param name="sources">List of source directories and search patterns</param>
        /// <param name="fromDate">File must have changed since this date to be included</param>
        static public IEnumerable<FileDetail> DiscoverFiles(
            List<Source> sources,
            string tempDir = null)
        {
            var settings = GetSettings();
            var files = new List<FileDetail>();

            sources.ForEach(s =>
            {
                files.AddRange(GetFiles(s.Directory, s.Pattern, s.IsModifiedOnly, s.LastBackup));
            });

            return files;
        }

        /// <summary>
        /// Scan for files to backup
        /// </summary>
        /// <param name="directory">Root directory</param>
        /// <param name="pattern">File extension e.g. *.*</param>
        /// <param name="fromDate">The date of the last backup</param>
        /// <returns>List of files</returns>
        static private IEnumerable<FileDetail> GetFiles(
            string directory,
            string pattern,
            bool isModifiedOnly,
            DateTime fromDate)
        {
            var changed = new List<FileDetail>();
            var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);

            if(isModifiedOnly)
            {
                foreach (var file in files)
                {
                    var lastWrite = File.GetLastWriteTime(file);
                    if (fromDate < lastWrite)
                        changed.Add(new FileDetail(file, directory));
                }
            }
            else
            {
                changed.AddRange(files.Select(f => new FileDetail(f, directory)));
            }

            return changed;
        }

        public static void SaveDiscoveredFiles(IEnumerable<FileDetail> fileDetails)
        {
            var discoFile = Path.Combine(GetTempDirectory(), DiscoveredFileName);

            if (File.Exists(discoFile))
                File.Delete(discoFile);

            File.AppendAllLines(discoFile, fileDetails.Select(f => $"{f.FilePath}, {f.SubPath}"));
        }

        /// <summary>
        /// Create the sources file
        /// </summary>
        /// <param name="tempDir">Location of sources file</param>
        static private void CreateSourcesFile(List<Source> sources = null)
        {
            string tempDir = GetTempDirectory();

            var sourcesPath = Path.Combine(tempDir, SourcesFileName);
            if (!File.Exists(sourcesPath))
            {
                using (var writer = File.CreateText(sourcesPath))
                {
                    writer.WriteLine("Directory, Pattern, ModifiedOnly, LastBackup");

                    if (sources != null)
                    {
                        sources.ForEach(s =>
                        {
                            writer.WriteLine($"{s.Directory}, {s.Pattern}, {s.ModifiedOnly}, {s.LastBackup.ToString()}");
                        });
                    }

                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Invoke the whole backup process - get sources, discover files, copy to destination
        /// </summary>
        static public void InvokeBackup()
        {
            try
            {
                var sources = GetSources();
                var files = DiscoverFiles(sources);
                SaveDiscoveredFiles(files);
                CopyFiles();
                UpdateTimestamp(sources);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                BackupError?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Invoke backup of the current files list, possibly modified by user
        /// </summary>
        static public void InvokeBackupFromUser()
        {
            try
            {
                var sources = GetSources();
                CopyFiles();
                UpdateTimestamp(sources);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                BackupError?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Do the backing up of the discovered files
        /// </summary>
        /// <param name="archiveDir">A directory to backup to</param>
        static public void CopyFiles(string archiveDir = null)
        {
            var settings = GetSettings();
            var tempDir = GetTempDirectory();

            var filesName = Path.Combine(tempDir, DiscoveredFileName);
            if (!File.Exists(filesName))
            {
                BackupWarning?.Invoke("No new or modified files were discovered");
                return; // Nothing to do
            }
            var files = File.ReadAllLines(filesName);
            if (files.Length == 0)
            {
                BackupWarning?.Invoke("No new or modified files were discovered");
                return; // Nothing to do
            }

            if (archiveDir == null)
                archiveDir = settings.FileSystemDirectory;

            // Has user set an archive folder?
            if (String.IsNullOrEmpty(archiveDir))
                archiveDir = Path.Combine(tempDir, ArchiveFolder);

            // Make the archive
            var zipArchivePath = DoCopy(archiveDir, files);

            // Upload the zip archive
            if (settings.IsS3BucketEnabled)
            {
                UploadS3Archive(settings, zipArchivePath);

                BackupSuccess?.Invoke($"Backup uploaded to S3 Bucket {settings.AWSS3Bucket}");

                // Clean up if the user didn't want the File System option
                if (!settings.IsFileSystemEnabled)
                {
                    File.Delete(zipArchivePath);
                }
            }

            if (settings.IsGlacierEnabled)
            {
                UploadGlacierArchive(settings, zipArchivePath);

                BackupSuccess?.Invoke($"Backup uploaded to Glacier Vault {settings.AWSGlacierVault}");

                // Clean up if the user didn't want the File System option
                if (!settings.IsFileSystemEnabled)
                {
                    File.Delete(zipArchivePath);
                }
            }
        }

        private static void UploadGlacierArchive(Settings settings, string zipArchivePath)
        {
            var manager = new ArchiveTransferManager(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey, 
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            var options = new UploadOptions();
            options.StreamTransferProgress += FileManager.UploadGlacierProgress;

            var fi = new FileInfo(zipArchivePath);

            // Upload an archive.
            string archiveId = manager.Upload(
                settings.AWSGlacierVault, 
                fi.Name, 
                zipArchivePath, 
                options).ArchiveId;

            // TODO persist archive ID
            Debug.WriteLine($"Archive ID: {archiveId}");
        }

        private static void UploadGlacierProgress(object sender, StreamTransferProgressArgs e)
        {
            Debug.WriteLine($"Uploaded {e.PercentDone}");
        }

        private static void UploadS3Archive(Settings settings, string zipPath)
        {
            var transferUtility = new TransferUtility(
                settings.AWSAccessKeyID, 
                settings.AWSSecretAccessKey, 
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            // Create bucket if not found
            if (!transferUtility.S3Client.DoesS3BucketExist(settings.AWSS3Bucket))
            {
                transferUtility.S3Client.PutBucket(
                    new PutBucketRequest() { BucketName = settings.AWSS3Bucket });
            }

            try
            {
                // Copy zipfile 
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = settings.AWSS3Bucket,
                    FilePath = zipPath,
                    //StorageClass // TODO
                };
                request.UploadProgressEvent += Request_UploadProgressEvent;
                transferUtility.Upload(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Copy files to the given directory and return the zip file name
        /// </summary>
        private static string DoCopy(string archiveDir, string[] files)
        {
            int fileCount = 0;
            // Create processing file and copy logging files to processing file
            //var processing = Path.Combine(tempDir, ProcessingName);
            //using (var processWriter = File.CreateText(processing))
            //{
            var uniqueArchiveDir = GetArchiveUniqueName(archiveDir);

            foreach (var line in files)
            {
                try
                {
                    // Copy file and append to processing file
                    var filepath = (from f in line.Split(',') select f.Trim()).First();
                    var subpath = (from f in line.Split(',') select f.Trim()).Last();

                    // Create the subpath in archive
                    var splits = subpath.Split('\\');
                    var subpathDir = "";
                    for (int i = 0; i < splits.Length - 1; i++)
                        subpathDir += "\\" + splits[i];

                    var newDir = Path.Combine(uniqueArchiveDir, subpathDir.TrimStart('\\'));
                    var di = Directory.CreateDirectory(newDir);
                    Debug.Write(di);

                    File.Copy(filepath, Path.Combine(uniqueArchiveDir, subpath.TrimStart('\\')), true);
                    fileCount++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }

            if (fileCount > 0)
            {
                try
                {
                    var zipFileArchive = $"{uniqueArchiveDir}.zip";

                    ZipFile.CreateFromDirectory(uniqueArchiveDir, zipFileArchive, CompressionLevel.Optimal, false);

                    // Clean up folders used to make the zip archive
                    DeleteZipSource(uniqueArchiveDir);

                    BackupSuccess?.Invoke($"{fileCount} file {(fileCount == 1 ? String.Empty : "s")} copied to {zipFileArchive}");
                    return zipFileArchive;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }

            return null;
        }

        private static void DeleteZipSource(string zipSource)
        {
            Thread.Sleep(1000);
            DeleteFolderContents(zipSource);
        }

        private static void DeleteFolderContents(string folder)
        {
            var files = Directory.EnumerateFiles(folder);
            foreach (var file in files)
                File.Delete(file);

            var dirs = Directory.EnumerateDirectories(folder);
            foreach (var dir in dirs)
            {
                if (Directory.EnumerateFiles(dir).Count() == 0 && Directory.EnumerateDirectories(dir).Count() == 0)
                {
                    Directory.Delete(dir);
                }
                else
                {
                    DeleteFolderContents(dir);
                }
            }
            Directory.Delete(folder);
        }

        static internal string GetArchiveUniqueName(string archiveDir = null)
        {
            return Path.Combine(archiveDir, $"archive_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}");
        }

        /// <summary>
        /// Update the timestamp on the sources file
        /// </summary>
        public static void UpdateTimestamp(List<Source> sources)
        {
            sources.ForEach(s => s.LastBackup = DateTime.Now);
            SaveSources(sources);
        }

        private static void Request_UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            Debug.WriteLine($"{e.PercentDone}% done");
        }

    }
}
