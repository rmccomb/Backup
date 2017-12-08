using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Backup.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Amazon.Glacier;
    using Amazon.Glacier.Model;
    using Amazon.Glacier.Transfer;
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using Amazon.SimpleNotificationService.Model;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Backup.Logic;
    using Amazon;
    using System.Diagnostics;

    namespace glacier.amazon.com.docsamples
    {
        /*  The following C# code example retrieves the vault inventory for the specified vault.
            IMPORTANT Note that it takes about four hours for most jobs to complete.

            The example performs the following tasks:

            Set up an Amazon SNS topic.
            Amazon Glacier sends notification to this topic after it completes the job.

            Set up an Amazon SQS queue.
            The example attaches a policy to the queue to enable the Amazon SNS topic to post messages.

            Initiate a job to download the specified archive.
            In the job request, the example specifies the Amazon SNS topic so that Amazon Glacier can send a 
            message after it completes the job.

            Periodically check the Amazon SQS queue for a message.
            If there is a message, parse the JSON and check if the job completed successfully. If it did, 
            download the archive.The code example uses the JSON.NET library (see JSON.NET) to parse the JSON.

            Clean up by deleting the Amazon SNS topic and the Amazon SQS queue it created.            
        */
        [TestClass]
        public class VaultInventoryJobLowLevelUsingSNSSQS
        {
            DestinationSettings settings;
            string topicArn;
            string queueUrl;
            string queueArn;
            string vaultName = "*** Provide vault name ***";
            string fileName = "*** Provide file name and path where to store inventory ***";
            AmazonSimpleNotificationServiceClient snsClient;
            AmazonSQSClient sqsClient;
            const string SQS_POLICY =
                "{" +
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

            [TestInitialize]
            public void Init()
            {
                settings = FileManager.GetSettings();
                vaultName = settings.AWSGlacierVault;
                fileName = @"C:\Users\Rob\source\repos\Backup\archive\inventory.json";
            }

            [TestMethod]
            public void VaultLowLevel()
            {
                try
                {
                    using (var client = new AmazonGlacierClient(
                        settings.AWSAccessKeyID,
                        settings.AWSSecretAccessKey,
                        RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
                    {
                        Debug.WriteLine("Setup SNS topic and SQS queue.");
                        SetupTopicAndQueue();

                        Debug.WriteLine("Retrieve Inventory List");
                        GetVaultInventory(client);
                    }
                    Debug.WriteLine("Operations successful.");
                }
                catch (AmazonGlacierException e) { Debug.WriteLine(e.Message); }
                catch (AmazonServiceException e) { Debug.WriteLine(e.Message); }
                catch (Exception e) { Debug.WriteLine(e.Message); }
                finally
                {
                    // Delete SNS topic and SQS queue.
                    snsClient.DeleteTopic(new DeleteTopicRequest() { TopicArn = this.topicArn });
                    sqsClient.DeleteQueue(new DeleteQueueRequest() { QueueUrl = this.queueUrl });
                }
            }

            [TestMethod]
            public void RestartPollingQueue()
            {
                using (var client = new AmazonGlacierClient(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName)))
                {
                    this.queueUrl = "https://sqs.ap-southeast-2.amazonaws.com/019910574325/GlacierDownload-636482535974517302";
                    var jobId = "fQfO2jfxjxZeGgy-mDYz2_p74vuWGaTF9wugaIi-Mbpa5vqiSOLROTrXxRLHwz3LXHsadyOrn_HDXsefe_yk4Z6rb4T_";
                    ProcessQueue(jobId, client);
                }

                // Delete SNS topic and SQS queue.
                snsClient.DeleteTopic(new DeleteTopicRequest() { TopicArn = this.topicArn }); // TODO need topic ARN
                sqsClient.DeleteQueue(new DeleteQueueRequest() { QueueUrl = this.queueUrl });
            }

            void SetupTopicAndQueue()
            {
                long ticks = DateTime.Now.Ticks;

                // Setup SNS topic.
                snsClient = new AmazonSimpleNotificationServiceClient(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                sqsClient = new AmazonSQSClient(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                topicArn = snsClient.CreateTopic(new CreateTopicRequest { Name = "GlacierDownload-" + ticks }).TopicArn;
                Debug.WriteLine($"topicArn: {topicArn}");

                var createQueueRequest = new CreateQueueRequest { QueueName = "GlacierDownload-" + ticks };
                var createQueueResponse = sqsClient.CreateQueue(createQueueRequest);
                this.queueUrl = createQueueResponse.QueueUrl;
                Debug.WriteLine($"QueueURL: {this.queueUrl}");

                var getQueueAttributesRequest = new GetQueueAttributesRequest {
                    AttributeNames = new List<string> { "QueueArn" },
                    QueueUrl = this.queueUrl
                };
                var response = sqsClient.GetQueueAttributes(getQueueAttributesRequest);
                queueArn = response.QueueARN;
                Debug.WriteLine($"QueueArn: {queueArn}");

                // Setup the Amazon SNS topic to publish to the SQS queue.
                snsClient.Subscribe(new SubscribeRequest() {
                    Protocol = "sqs",
                    Endpoint = queueArn,
                    TopicArn = topicArn
                });

                // Add the policy to the queue so SNS can send messages to the queue.
                var policy = SQS_POLICY.Replace("{TopicArn}", topicArn).Replace("{QuernArn}", queueArn);

                sqsClient.SetQueueAttributes(new SetQueueAttributesRequest {
                    QueueUrl = this.queueUrl,
                    Attributes = new Dictionary<string, string>
                    {
                        { QueueAttributeName.Policy, policy }
                    }
                });
            }

            void GetVaultInventory(AmazonGlacierClient client)
            {
                // Initiate job.
                var initJobRequest = new InitiateJobRequest()
                {
                    VaultName = vaultName,
                    JobParameters = new JobParameters()
                    {
                        Type = "inventory-retrieval",
                        Description = "This job is to download a vault inventory.",
                        SNSTopic = topicArn,
                    }
                };

                var initJobResponse = client.InitiateJob(initJobRequest);
                string jobId = initJobResponse.JobId;

                // Check queue for a message and if job completed successfully, download inventory.
                ProcessQueue(jobId, client);
            }

            void ProcessQueue(string jobId, AmazonGlacierClient client)
            {
                var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = this.queueUrl, MaxNumberOfMessages = 1 };
                bool jobDone = false;
                while (!jobDone)
                {
                    Debug.WriteLine("Poll SQS queue");
                    if(sqsClient == null)
                        sqsClient = new AmazonSQSClient(settings.AWSAccessKeyID, settings.AWSSecretAccessKey, RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                    var receiveMessageResponse = sqsClient.ReceiveMessage(receiveMessageRequest);
                    if (receiveMessageResponse.Messages.Count == 0)
                    {
                        Thread.Sleep(10000 * 60);
                        continue;
                    }
                    Debug.WriteLine("Got message");
                    Message message = receiveMessageResponse.Messages[0];
                    //Dictionary<string, string> outerLayer = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Body);
                    //Dictionary<string, object> fields = JsonConvert.DeserializeObject<Dictionary<string, object>>(outerLayer["Message"]);
                    //string statusCode = fields["StatusCode"] as string;

                    //if (string.Equals(statusCode, GlacierUtils.JOB_STATUS_SUCCEEDED, StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Debug.WriteLine("Downloading job output");
                    //    DownloadOutput(jobId, client); // Save job output to the specified file location.
                    //}
                    //else if (string.Equals(statusCode, GlacierUtils.JOB_STATUS_FAILED, StringComparison.InvariantCultureIgnoreCase))
                    //    Debug.WriteLine("Job failed... cannot download the inventory.");

                    jobDone = true;
                    sqsClient.DeleteMessage(new DeleteMessageRequest { QueueUrl = this.queueUrl, ReceiptHandle = message.ReceiptHandle });
                }
            }

            void DownloadOutput(string jobId, AmazonGlacierClient client)
            {
                var getJobOutputRequest = new GetJobOutputRequest {
                    JobId = jobId,
                    VaultName = vaultName
                };

                var getJobOutputResponse = client.GetJobOutput(getJobOutputRequest);
                using (Stream webStream = getJobOutputResponse.Body) {
                    using (Stream fileToSave = File.OpenWrite(this.fileName)) {
                        CopyStream(webStream, fileToSave);
                    }
                }
            }

            void CopyStream(Stream input, Stream output)
            {
                byte[] buffer = new byte[65536];
                int length;
                while ((length = input.Read(buffer, 0, buffer.Length)) > 0) {
                    output.Write(buffer, 0, length);
                }
            }
        }
    }
}
