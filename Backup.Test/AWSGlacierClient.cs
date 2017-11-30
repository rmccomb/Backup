using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Glacier;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;
using System.Diagnostics;
using Amazon.Glacier.Model;

namespace Backup.Test
{
    [TestClass]
    public class AWSGlacierClient
    {
        [TestMethod]
        public void GlacierPutFile()
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
        public void GlacierGetArchive()
        {
            var vaultName = "test";
            var archiveId = "PJN_XpM9iRZJM_NFYrFjJz1TD2 - nTzzF1uQS5aSNe0P7ThyyAGb8YWuNspfFYB4XlkbLka7wFtfURrJ5n17iRbNbgfprN7_9Fup3bgEG48YXjV - 0Ywi0XBGtRpc5siqjXB8EFB55Pg";
            var andrew = "";
            var secret = "";
            var downloadFilePath = @"C:\Users\Rob\source\repos\Backup\archive\glacier.zip";
            try
            {
                var manager = new ArchiveTransferManager(andrew, secret, Amazon.RegionEndpoint.USEast1);
                var options = new DownloadOptions();
                options.StreamTransferProgress += AWSGlacierClient.Progress;

                // Download an archive.
                // Intiating the archive retrieval job and then polling SQS queue for the archive to be available
                // This polling takes about 4 hours. Once the archive is available, downloading will begin.
                manager.Download(vaultName, archiveId, downloadFilePath, options);
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


        //static string vaultName = "*** Provide vault name ***";
        //static AmazonGlacierClient client;

        //public static void Main(string[] args)
        //{
        //    try
        //    {
        //        using (client = new AmazonGlacierClient(Amazon.RegionEndpoint.USWest2))
        //        {
        //            Console.WriteLine("Creating a vault.");
        //            CreateAVault();
        //            DescribeVault();
        //            GetVaultsList();
        //            Console.WriteLine("\nVault created. Now press Enter to delete the vault...");
        //            Console.ReadKey();
        //            DeleteVault();
        //        }
        //    }
        //    catch (AmazonGlacierException e) { Console.WriteLine(e.Message); }
        //    catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
        //    catch (Exception e) { Console.WriteLine(e.Message); }
        //    Console.WriteLine("To continue, press Enter");
        //    Console.ReadKey();
        //}

        //static void CreateAVault()
        //{
        //    CreateVaultRequest request = new CreateVaultRequest()
        //    {
        //        VaultName = vaultName
        //    };
        //    CreateVaultResponse response = client.CreateVault(request);
        //    Console.WriteLine("Vault created: {0}\n", response.Location);
        //}

        [TestMethod]
        public void DescribeVault()
        {
            DescribeVaultRequest describeVaultRequest = new DescribeVaultRequest()
            {
                VaultName = "test"
            };

            var client = new AmazonGlacierClient(Amazon.RegionEndpoint.USEast1);

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
            Debug.WriteLine("\n List of vaults in your account in the specific region ...");
            var client = new AmazonGlacierClient(Amazon.RegionEndpoint.USEast1);
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

        //static void DeleteVault()
        //{
        //    DeleteVaultRequest request = new DeleteVaultRequest()
        //    {
        //        VaultName = vaultName
        //    };
        //    DeleteVaultResponse response = client.DeleteVault(request);
        //}
    }
}

