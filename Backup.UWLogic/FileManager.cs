using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
using System.Web.Script.Serialization;
using Windows.Storage;
using System.Runtime.Serialization;
using Windows.Storage.Streams;

namespace Backup.UWLogic
{
    public class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";
        public const string SettingsName = "backup.settings";
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
        public static event BackupStartedHandler BackupStarted;
        public delegate void BackupStartedHandler();
        public static event BackupCompletedHandler BackupCompleted;
        public delegate void BackupCompletedHandler();

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

        /// <summary>
        /// Get the configured temp directory from application settings or, 
        /// if not defined create a default under user's Application Data
        /// </summary>
        static public string GetTempDirectory()
        {
            //var tempDir = System.Configuration.ConfigurationManager.AppSettings.Get(TempDirKey);
            //if (string.IsNullOrEmpty(tempDir))
            //{
            //    tempDir = Directory.GetParent(
            //        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            //    if (Environment.OSVersion.Version.Major >= 6)
            //    {
            //        tempDir = Directory.GetParent(tempDir).ToString();
            //    }

            //    // Default to our folder if nothing set
            //    tempDir = Path.Combine(tempDir, SettingsFolder);
            //    if (!Directory.Exists(tempDir))
            //        Directory.CreateDirectory(tempDir);
            //}

            //return tempDir;

            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }

        public static async void SaveSettingsAsync(Settings settings)
        {
            Debug.WriteLine("SaveSettings");

            var localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(localFolder.Path);
            var formatter = new DataContractSerializer(typeof(Settings));
            StorageFile file = await localFolder.CreateFileAsync(SettingsName, CreationCollisionOption.ReplaceExisting);
            MemoryStream stream = new MemoryStream();
            formatter.WriteObject(stream, settings);
            await FileIO.WriteBytesAsync(file, stream.ToArray());
        }

        public static async Task<Settings> GetSettingsAsync()
        {
            Debug.WriteLine("GetSettings");

            var localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(localFolder.Path);
            var settingsFilePath = Path.Combine(localFolder.Path, SettingsName);
            if (!File.Exists(settingsFilePath))
                return new Settings();
                //SaveSettings(new Settings());

            var formatter = new DataContractSerializer(typeof(Settings));
            StorageFile file = await localFolder.GetFileAsync(SettingsName);
            var stream = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (Settings)formatter.ReadObject(stream);
            return settings;
        }

        public static Topic GetExistingTopic(string topicName)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = Path.Combine(localFolder.Path, topicName);
            if (File.Exists(file))
            {
                var formatter = new DataContractSerializer(typeof(Topic));
                var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                var topic = (Topic)formatter.ReadObject(stream);

                // Update status
                //topic.Status = ProcessTopic(topic, outputFile);
                Debug.WriteLine($"Found topic file: {topic.TopicFileName} {topic.Status}");
                return topic;
            }
            return null;
        }

        private static async void SaveTopicFile(Topic topic)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var formatter = new DataContractSerializer(typeof(Topic));
            //var topicFile = Path.Combine(GetTempDirectory(), topic.TopicFileName);
            //File.Delete(topicFile);
            StorageFile file = await localFolder.CreateFileAsync(topic.TopicFileName, CreationCollisionOption.ReplaceExisting);
            MemoryStream stream = new MemoryStream();
            formatter.WriteObject(stream, topic);
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
            var settings = await GetSettingsAsync();

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
                var settings = await GetSettingsAsync();
                var downloadInfo = new DownloadObjectInfo { DownloadDirectory = downloadDir, ObjectKey = objectKey };
                AmazonS3Client client = new AmazonS3Client(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                var request = new GetObjectRequest { BucketName = settings.AWSS3Bucket, Key = objectKey };
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
            return ArchiveTopicFileName.Replace("#", archiveId.Substring(archiveId.Length - 15, 15).ToString());
        }

        /// <summary>
        /// Issue a request to get Glacier inventory or process existing request
        /// </summary>
        public static async Task<GlacierResult> GetGlacierInventoryAsync(Settings settings)
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
                var updateIsDue = IsInventoryUpdateDue(settings);
                if (topic == null && updateIsDue)
                {
                    // Issue new request and serialize topic details to global file
                    topic = await SetupTopicAndSubscriptionsAsync(InventoryTopicFileName, GetTempDirectory(), settings.AWSGlacierRegion.SystemName, null, InventoryFileName);
                    topic.Type = "inventory-retrieval";
                    topic.Description = "This job is to download a vault inventory";
                    InitiateGlacierJob(topic);
                    settings.InventoryUpdateRequested = DateTime.Now;
                    SaveSettingsAsync(settings);
                    return topic.Status; // only requested - no need to process yet
                }
                if (!updateIsDue)
                    return GlacierResult.NoJob;

                var result = await ProcessQueueAsync(topic);
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

        private static bool IsInventoryUpdateDue(Settings settings)
        {
            return settings.IsGlacierEnabled
                && DateTime.Now - settings.InventoryUpdateRequested > new TimeSpan(24, 0, 0);
        }

        /// <summary>
        /// Get a Glacier archive
        /// </summary>
        public static async Task<GlacierResult> RequestGlacierArchiveAsync(string archiveId, string downloadDirectory, string filename)
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
                    var settings = GetSettingsAsync().Result;
                    // Issue new request and serialize topic details to file
                    topic = await SetupTopicAndSubscriptionsAsync(topicFileName, downloadDirectory, settings.AWSGlacierRegion.SystemName, archiveId, filename);
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
                    var model = new ArchiveModelList { InventoryDate = dt };
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
                    SaveInventoryModelAsync(model);
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
                    if (m.ArchiveTopicFilePath != null)
                    {
                        var topicFile = GetArchiveTopicFileName(m.ArchiveId);
                        var topic = GetExistingTopic(topicFile);
                        if (topic != null)
                        {
                            try
                            {
                                var result = ProcessQueueAsync(topic);
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

        private static async void SaveInventoryModelAsync(ArchiveModelList model)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var formatter = new DataContractSerializer(typeof(ArchiveModelList));
            var inventoryModelFileName = Path.Combine(localFolder.Path, InventoryModelFileName);
            StorageFile file = await localFolder.CreateFileAsync(SettingsName, CreationCollisionOption.ReplaceExisting);
            MemoryStream stream = new MemoryStream();
            formatter.WriteObject(stream, model);
            await FileIO.WriteBytesAsync(file, stream.ToArray());
            Debug.WriteLine("Saved inventory model");
        }

        public static async Task<GlacierResult> ProcessQueueAsync(Topic topic)
        {
            // Check for notifications on topic and process any message
            try
            {
                var settings = GetSettingsAsync().Result;
                using (var client = new AmazonGlacierClient(
                            settings.AWSAccessKeyID,
                            settings.AWSSecretAccessKey,
                            RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
                {
                    var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = topic.QueueUrl, MaxNumberOfMessages = 1 };
                    var sqsClient = new AmazonSQSClient(settings.AWSAccessKeyID, settings.AWSSecretAccessKey, RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));
                    var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);
                    if (receiveMessageResponse.Messages.Count == 0)
                    {
                        topic.Status = GlacierResult.Incomplete;
                        SaveTopicFile(topic);
                        return topic.Status;
                    }

                    // Process message
                    string status = GetResponseStatus(receiveMessageResponse);
                    if (string.Equals(status, GlacierUtils.JOB_STATUS_SUCCEEDED, StringComparison.OrdinalIgnoreCase))
                    {
                        DownloadGlacierJobOutputAsync(topic.JobId, client, settings.AWSGlacierVault, topic.GetOutputFile());
                        Debug.WriteLine($"Downloaded job output to {topic.GetOutputFile()}");
                        if (topic.ArchiveId != null)
                            DownloadSuccess?.Invoke($"Glacier archive was downloaded to {topic.GetOutputFile()}");
                        DeleteTopicAsync(topic);
                        return GlacierResult.Completed;
                    }
                    else if (string.Equals(status, GlacierUtils.JOB_STATUS_FAILED, StringComparison.OrdinalIgnoreCase))
                    {
                        DownloadError?.Invoke("Job failed, cannot download the file");
                        DeleteTopicAsync(topic);
                        return GlacierResult.JobFailed;
                    }
                    else if (string.Equals(status, GlacierUtils.JOB_STATUS_INPROGRESS, StringComparison.OrdinalIgnoreCase))
                    {
                        DownloadWarning?.Invoke("Job in progress, Queue ARN: " + topic.QueueARN);
                        DeleteTopicAsync(topic);
                        return GlacierResult.JobInProgress;
                    }
                    else
                    {
                        DeleteTopicAsync(topic);
                        return GlacierResult.Error;
                    }
                }
            }
            catch (AmazonServiceException azex)
            {
                // Handle specific potential errors here
                Debug.WriteLine("AmazonServiceException " + azex.Message);

                if (azex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Invalid credentials
                    BackupError?.Invoke("Invalid AWS credentials were provided while connecting");
                    return GlacierResult.Incomplete;
                }
                if (azex.InnerException != null
                    && azex.InnerException is System.Net.WebException
                    && ((System.Net.WebException)azex.InnerException).Status == System.Net.WebExceptionStatus.NameResolutionFailure)
                {
                    // Not connected to internet
                    BackupError?.Invoke("Network connection failure");
                    return GlacierResult.Incomplete;
                }
                if (azex.InnerException != null
                    && azex.InnerException is System.Net.WebException)
                {
                    // Network errors
                    BackupError?.Invoke($"A network error occurred ({((System.Net.WebException)azex.InnerException).Status})");
                    return GlacierResult.Incomplete;
                }
                if (azex.StatusCode == System.Net.HttpStatusCode.BadRequest
                    //&& topic.Status == GlacierResult.JobRequested 
                    && azex.Message.Contains("The specified queue does not exist")
                    && DateTime.Now - topic.DateRequested < new TimeSpan(24, 0, 0))
                {
                    // Job was recently requested and the queue has not been created yet
                    Debug.WriteLine("Job request may be in progress");
                    return GlacierResult.JobRequested;
                }

                // TODO Check expiry?
                // Glacier ref: "A job ID will not expire for at least 24 hours after Amazon Glacier completes the job."
                DeleteTopicAsync(topic);
                BackupWarning?.Invoke("An AWS Glacier job has expired, a new job will be issued");

                // Reissue expired job
                InitiateGlacierJob(topic);
                return topic.Status;
            }
            catch (Exception ex)
            {
                DeleteTopicAsync(topic);
                throw ex;
            }
        }

        private static async Task DeleteTopicAsync(Topic topic)
        {
            var settings = GetSettingsAsync().Result;
            var snsClient = new AmazonSimpleNotificationServiceClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            var sqsClient = new AmazonSQSClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            // Cleanup topic & queue & local file
            try { await snsClient.DeleteTopicAsync(new DeleteTopicRequest() { TopicArn = topic.TopicARN }); } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            try { await sqsClient.DeleteQueueAsync(new DeleteQueueRequest() { QueueUrl = topic.QueueUrl }); } catch (Exception ex) { Debug.WriteLine(ex.Message); }

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

        public static async void DownloadGlacierJobOutputAsync(
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

                var getJobOutputResponse = await client.GetJobOutputAsync(getJobOutputRequest);
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
            var settings = GetSettingsAsync().Result;
            using (var client = new AmazonGlacierClient(
                        settings.AWSAccessKeyID,
                        settings.AWSSecretAccessKey,
                        RegionEndpoint.GetBySystemName(settings.AWSGlacierRegion.SystemName)))
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

                var initJobResponse = client.InitiateJobAsync(initJobRequest).Result;
                topic.JobId = initJobResponse.JobId;
                topic.Status = GlacierResult.JobRequested;
                topic.DateRequested = DateTime.Now;
                SaveTopicFile(topic);
            }
        }

        private static async Task<Topic> SetupTopicAndSubscriptionsAsync(string topicFileName, string outputDirectory, string regionSystemName, string archiveId = null, string filename = null)
        {
            var topic = new Topic
            {
                TopicFileName = topicFileName,
                OutputDirectory = outputDirectory,
                ArchiveId = archiveId,
                FileName = filename,
                DateRequested = DateTime.Now
            };
            long ticks = DateTime.Now.Ticks;
            var settings = GetSettingsAsync().Result;

            #region Setup SNS topic
            var snsClient = new AmazonSimpleNotificationServiceClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(regionSystemName));

            var sqsClient = new AmazonSQSClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(regionSystemName));

            var topicArn = snsClient.CreateTopicAsync(new CreateTopicRequest { Name = "GlacierDownload-" + ticks }).Result.TopicArn;
            //Debug.WriteLine($"topicArn: {topicArn}");
            topic.TopicARN = topicArn;
            #endregion

            #region Setup SQS queue
            var createQueueRequest = new CreateQueueRequest { QueueName = "GlacierDownload-" + ticks };
            var createQueueResponse = sqsClient.CreateQueueAsync(createQueueRequest).Result;
            var queueUrl = createQueueResponse.QueueUrl;
            //Debug.WriteLine($"QueueURL: {queueUrl}");
            topic.QueueUrl = queueUrl;

            var getQueueAttributesRequest = new GetQueueAttributesRequest
            {
                AttributeNames = new List<string> { "QueueArn" },
                QueueUrl = queueUrl
            };
            var response = await sqsClient.GetQueueAttributesAsync(getQueueAttributesRequest);
            var queueArn = response.QueueARN;
            Debug.WriteLine($"QueueArn: {queueArn}");
            topic.QueueARN = queueArn;
            #endregion

            // Setup the Amazon SNS topic to publish to the SQS queue.
            // TODO SMS subscription
            await snsClient.SubscribeAsync(new SubscribeRequest()
            {
                Protocol = "sqs",
                Endpoint = queueArn,
                TopicArn = topicArn
            });

            // Add the policy to the queue so SNS can send messages to the queue.
            var policy = SQS_POLICY.Replace("{TopicArn}", topicArn).Replace("{QuernArn}", queueArn);

            await sqsClient.SetQueueAttributesAsync(new SetQueueAttributesRequest
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
            var settings = GetSettingsAsync().Result;
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
            var files = new List<string>();
            GetFiles(directory, pattern, files);

            if (isModifiedOnly)
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

        static void GetFiles(string path, string pattern, List<string> list)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                var isDir = info.Attributes.HasFlag(System.IO.FileAttributes.Directory);
                var isHidden = info.Attributes.HasFlag(System.IO.FileAttributes.Hidden);
                var isSys = info.Attributes.HasFlag(System.IO.FileAttributes.System);
                var isReparse = info.Attributes.HasFlag(System.IO.FileAttributes.ReparsePoint);
                if (isSys || isHidden /*|| isReparse*/) return;

                list.AddRange(Directory.GetFiles(path, pattern));

                var dirs = Directory.GetDirectories(path);
                foreach (var d in dirs)
                {
                    GetFiles(d, pattern, list);
                }
            }
            catch (Exception ex)
            {
                DirectoryInfo info = new DirectoryInfo(path);
                var att1 = info.Attributes.HasFlag(FileAttributes.Directory);

                Debug.WriteLine(ex.Message);
                BackupError?.Invoke(ex.Message);
            }
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
                BackupStarted?.Invoke();

                //while (!cts.IsCancellationRequested)
                //{
                //    Thread.Sleep(1000);
                //    Debug.WriteLine("WORKING");
                //}
                //Debug.WriteLine("COMPLETE");
                //isWorking = false;
                //return;

                var sources = GetSources();
                var files = DiscoverFiles(sources);
                SaveDiscoveredFiles(files);
                CopyFiles();
                UpdateTimestamp(sources);

                BackupCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                BackupError?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Invoke backup of the discovered files list, possibly modified by user
        /// </summary>
        static public void InvokeBackupFromUser()
        {
            try
            {
                BackupStarted?.Invoke();

                CopyFiles();

                UpdateTimestamp(GetSources());

                BackupCompleted?.Invoke();
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
            var fileSource = DoCopy(archiveDir, files);

            // Upload the zip archive
            if (settings.IsS3BucketEnabled)
            {
                UploadS3Archive(settings, fileSource.ZipFileArchive);

                BackupSuccess?.Invoke($"Backup uploaded to S3 Bucket {settings.AWSS3Bucket}");

                // Clean up if the user didn't want the File System option
                if (!settings.IsFileSystemEnabled)
                {
                    File.Delete(fileSource.ZipFileArchive);
                }
            }

            if (settings.IsGlacierEnabled)
            {
                UploadGlacierArchive(settings, fileSource.ZipFileArchive);

                BackupSuccess?.Invoke($"Backup uploaded to Glacier Vault {settings.AWSGlacierVault}");

                // Clean up if the user didn't want the File System option
                if (!settings.IsFileSystemEnabled)
                {
                    File.Delete(fileSource.ZipFileArchive);
                }
            }

            try
            {
                // Clean up source directory created for zip archive
                DeleteZipSource(fileSource.UniqueArchiveDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                BackupWarning?.Invoke($"An error occurred removing the temporary archive files: {fileSource.UniqueArchiveDir}");
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
        private static FileSource DoCopy(string archiveDir, string[] files)
        {
            var fileSource = new FileSource { UniqueArchiveDir = GetArchiveUniqueName(archiveDir) };

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

                    var newDir = Path.Combine(fileSource.UniqueArchiveDir, subpathDir.TrimStart('\\'));
                    var di = Directory.CreateDirectory(newDir);
                    //Debug.Write(di);

                    try
                    {
                        var dest = Path.Combine(fileSource.UniqueArchiveDir, subpath.TrimStart('\\'));
                        File.Copy(filepath, dest, true);
                        File.SetAttributes(dest, FileAttributes.Normal);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.WriteLine("UnauthorizedAccessException: " + ex.Message);
                        BackupWarning?.Invoke($"Unauthorized access error for {filepath}. File will be skipped.");
                    }
                    catch (IOException io)
                    {
                        Debug.WriteLine("IOException: " + io.Message);
                        BackupWarning?.Invoke($"IO Error for {filepath}. File will be skipped.");
                    }

                    fileSource.FileCount++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }

            if (fileSource.FileCount > 0)
            {
                try
                {
                    ZipFile.CreateFromDirectory(fileSource.UniqueArchiveDir, fileSource.ZipFileArchive, CompressionLevel.Optimal, false);

                    // Clean up folders used to make the zip archive
                    //DeleteZipSource(fileSource.UniqueArchiveDir);

                    BackupSuccess?.Invoke($"{fileSource.FileCount} file {(fileSource.FileCount == 1 ? String.Empty : "s")} copied to {fileSource.ZipFileArchive}");
                    return fileSource;
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
            Thread.Sleep(250);
            DeleteFolderContents(zipSource);
        }

        private static void DeleteFolderContents(string folder)
        {
            var files = Directory.EnumerateFiles(folder);
            foreach (var file in files)
            {
                File.Delete(file);
            }

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
