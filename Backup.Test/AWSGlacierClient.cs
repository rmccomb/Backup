using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Glacier;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;
using System.Diagnostics;
using Amazon.Glacier.Model;
using Backup.Logic;
using Amazon;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Backup.Test
{
    [TestClass]
    public class AWSGlacierClient
    {
        [TestMethod]
        public void UploadGlacier()
        {
            var vaultName = "test";
            var archive = @"C:\Users\Rob\source\repos\Backup\archive\archive_2017-11-24_122504.zip";
            var andrew = "";
            var secret = "";

            var manager = new ArchiveTransferManager(andrew, secret, Amazon.RegionEndpoint.USEast1);

            // Upload an archive.
            string archiveId = manager.Upload(vaultName, "getting started archive test", archive).ArchiveId;
            Debug.WriteLine($"Archive ID: {archiveId}");
        }

        [TestMethod]
        public void DownloadGlacier()
        {
            var archiveId = "SOUSY3B601HbkJSiFdrzvE3CUDHuLHaoxbKwbIiSIdIuQTjcTw_XXm0i38HGLaJhubxRrTdvY-5UjYsSLWK0PVMcG5Mf9LKd9oBPAuwNIzGGhZmZ7zF9DaQU6lDuPh2C_4ViIc5dlw";
            var settings = FileManager.GetSettings();
            var downloadFilePath = @"C:\Users\Rob\source\repos\Backup\archive\glacier.zip";
            try
            {
                var manager = new ArchiveTransferManager(
                    settings.AWSAccessKeyID,
                    settings.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

                var options = new DownloadOptions();
                options.StreamTransferProgress += AWSGlacierClient.Progress;

                // High-level method to download an archive.
                // Intiating the archive retrieval job and then polling SQS queue for the archive to be available
                // This polling takes about 4 hours. Once the archive is available, downloading will begin.
                manager.Download(settings.AWSGlacierVault, archiveId, downloadFilePath, options);
            }
            catch (AmazonGlacierException e) { Debug.WriteLine(e.Message); throw e; }
            catch (AmazonServiceException e) { Debug.WriteLine(e.Message); throw e; }
            catch (Exception e) { Debug.WriteLine(e.Message); throw e; }
        }

        static int currentPercentage = -1;
        static void Progress(object sender, StreamTransferProgressArgs args)
        {
            if (args.PercentDone != currentPercentage)
            {
                currentPercentage = args.PercentDone;
                Debug.WriteLine($"Downloaded {args.PercentDone}%");
            }
        }

        [TestMethod]
        public void DescribeVault()
        {
            var settings = FileManager.GetSettings();
            DescribeVaultRequest describeVaultRequest = new DescribeVaultRequest()
            {
                VaultName = settings.AWSGlacierVault
            };
            var client = new AmazonGlacierClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));

            DescribeVaultResponse describeVaultResponse = client.DescribeVault(describeVaultRequest);
            Debug.WriteLine("\nVault description...");
            Debug.WriteLine(
              "\nVaultName: " + describeVaultResponse.VaultName +
              "\nVaultARN: " + describeVaultResponse.VaultARN +
              "\nVaultCreationDate: " + describeVaultResponse.CreationDate +
              "\nNumberOfArchives: " + describeVaultResponse.NumberOfArchives +
              "\nSizeInBytes: " + describeVaultResponse.SizeInBytes +
              "\nLastInventoryDate: " + describeVaultResponse.LastInventoryDate
              );
        }

        [TestMethod]
        public void GetVaultsList()
        {
            string lastMarker = null;
            var settings = FileManager.GetSettings();
            DescribeVaultRequest describeVaultRequest = new DescribeVaultRequest()
            {
                VaultName = settings.AWSGlacierVault
            };
            var client = new AmazonGlacierClient(
                settings.AWSAccessKeyID,
                settings.AWSSecretAccessKey,
                RegionEndpoint.GetBySystemName(settings.AWSS3Region.SystemName));
            do
            {
                ListVaultsRequest request = new ListVaultsRequest()
                {
                    Marker = lastMarker
                };
                ListVaultsResponse response = client.ListVaults(request);

                foreach (DescribeVaultOutput output in response.VaultList)
                {
                    Debug.WriteLine("Vault Name: {0} \tCreation Date: {1} \t #of archives: {2}",
                                      output.VaultName, output.CreationDate, output.NumberOfArchives);
                }
                lastMarker = response.Marker;
            } while (lastMarker != null);
        }

        //[TestMethod]
        //public void ReadInventoryFile()
        //{
        //    var inventoryFile = Path.Combine(FileManager.GetTempDirectory(), FileManager.InventoryName);
        //    if (File.Exists(inventoryFile))
        //    {
        //        // Found inventory 
        //        using (StreamReader file = File.OpenText(inventoryFile))
        //        {
        //            var text = File.ReadAllText(inventoryFile);
        //            //Debug.WriteLine(JsonConvert.DeserializeObject<Inventory>(text));
        //            //Debug.WriteLine(JsonConvert.DeserializeObject(text));
        //            var inventory = new Inventory();
        //            JsonConvert.PopulateObject(text, inventory);
        //            Debug.WriteLine(inventory);
        //        }

        //    }
        //}

        [TestMethod]
        public void DeserialiseJson()
        {
            var inventoryFile = Path.Combine(FileManager.GetTempDirectory(), FileManager.InventoryName);
            if (File.Exists(inventoryFile))
            {
                // Found inventory 
                using (var file = File.Open(inventoryFile, FileMode.Open))
                {
                    var s = new DataContractJsonSerializer(typeof(Inventory));
                    var inventory = (Inventory)(s.ReadObject(file));
                    Debug.WriteLine($"Archive ID: {inventory.ArchiveList[0].ArchiveId}");
                }
            }

        }
    }
}

