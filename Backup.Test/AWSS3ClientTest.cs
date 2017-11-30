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
    public class AWSS3ClientTest
    {
        TransferUtility transferUtility;
        string bucket;
        string uploadFile;

        [TestMethod]
        public void S3PutFile()
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
        public void ListBucketContents()
        {
            var andrew = "";
            var secret = "";

            //this.bucket = "stuff.tiz.digital";
            AmazonS3Client client = new AmazonS3Client(andrew, secret, RegionEndpoint.APSoutheast2);

            // Issue call
            ListBucketsResponse response = client.ListBuckets();

            // View response data
            Debug.WriteLine($"Buckets owner - {response.Owner.DisplayName}");
            foreach (S3Bucket bucket in response.Buckets)
            {
                Debug.WriteLine($"{bucket.BucketName}, Created {bucket.CreationDate}");
            }

            ListObjectsV2Request objRequest = new ListObjectsV2Request
            {
                BucketName = "rmccomb"
            };

            ListObjectsV2Response objResponse = client.ListObjectsV2(objRequest);
            foreach (S3Object obj in objResponse.S3Objects)
            {
                Debug.WriteLine($"{obj.BucketName} {obj.Key}");
            }
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
            var accessKeyId = "";
            var secretAccessKey = "";
            var region = RegionEndpoint.APSoutheast2;
            this.transferUtility = new TransferUtility(accessKeyId, secretAccessKey, region);
            this.bucket = "stuff.tiz.digital";
            this.uploadFile = @"C:\Users\Rob\source\repos\Backup\Backup.Test\test files\file.txt";
        }

    }
}

