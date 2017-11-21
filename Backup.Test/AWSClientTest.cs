using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Configuration;
using Amazon;

namespace Backup.Test
{
    [TestClass]
    public class AWSClientTest
    {
        const int FIVE_MINUTES = 5 * 60 * 1000;
        TransferUtility transferUtility;
        string bucket;
        string uploadFile;

        [TestMethod]
        public void PutFile()
        {
            this.bucket = "test-bucket-rmccomb";
            // Create bucket if not found
            if (!this.transferUtility.S3Client.DoesS3BucketExist(this.bucket))
            {
                this.transferUtility.S3Client.PutBucket(
                    new PutBucketRequest() { BucketName = this.bucket });
            }

            var request = new TransferUtilityUploadRequest
            {
                BucketName = this.bucket,
                FilePath = this.uploadFile
            };
            request.UploadProgressEvent += Request_UploadProgressEvent;
            this.transferUtility.Upload(request);
        }

        [TestMethod]
        public void DeleteBucket()
        {
            this.bucket = "test-bucket-rmccomb";
            if (this.transferUtility.S3Client.DoesS3BucketExist(this.bucket))
            {
                this.transferUtility.S3Client.DeleteObject(this.bucket, "file.txt");
                this.transferUtility.S3Client.DeleteBucket(this.bucket);
            }
        }

        private void Request_UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            Debug.WriteLine($"{e.PercentDone}% done");
        }

        [TestInitialize]
        public void LoadConfiguration()
        {
            //NameValueCollection appConfig = ConfigurationManager.AppSettings;

            //if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            //{
            //    throw new Exception("AWSProfileName is not set in the App.Config");
            //}

            var accessKeyId = "";
            var secretAccessKey = "";
            var region = RegionEndpoint.APSoutheast2;
            this.transferUtility = new TransferUtility(accessKeyId, secretAccessKey, region);
            this.bucket = "stuff.tiz.digital";
            this.uploadFile = @"C:\Users\Rob\source\repos\Backup\Backup.Test\test files\file.txt";
        }

    }
}

