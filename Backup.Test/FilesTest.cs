using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backup.Logic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Backup.Test
{
    [TestClass]
    public class FileTest
    {
        [TestMethod]
        public void Process()
        {
            // Get files changed since fromDate
            var sources = FileManager.GetSources();

            // Dummy sources
            if (sources.Count == 0)
                sources.Add(
                    new Source(@"C:\Users\Rob\source\repos\Backup\Backup.Test\test files\", "Yes", "*.*"));

            FileManager.SaveDiscoveredFiles(FileManager.DiscoverFiles(sources));

            // Iterate discovered files and copy while updating status of file in list
            var archive = @"C:\Users\Rob\source\repos\Backup\Backup.Test\archive";
            FileManager.CopyFiles(archive);

            FileManager.UpdateTimestamp(sources);
        }

        [TestMethod]
        public void GetSources()
        {
            var sources = FileManager.GetSources();
            sources.ForEach(s => Debug.WriteLine($"{s.Directory}, {s.Pattern}"));
        }

        [TestMethod]
        public void ReadWriteSettingsFile()
        {
            var settings = new Settings
            {
                FileSystemDirectory = @"C:\Users\Rob\source\repos\Backup\Backup.Test\archive",
                AWSAccessKeyID = "AKIAIK5CONHAOVWPN27Q",
                AWSSecretAccessKey = "test123",
                AWSS3Bucket = "backup.tiz.digital",
                CreateBackupOnStart = true
            };
            FileManager.SaveSettings(settings);

            var loaded = FileManager.GetSettings();
            Assert.AreEqual(loaded.FileSystemDirectory, settings.FileSystemDirectory);
            Assert.AreEqual(loaded.AWSAccessKeyID, settings.AWSAccessKeyID);
            Assert.AreEqual(loaded.AWSSecretAccessKey, settings.AWSSecretAccessKey);
            Assert.AreEqual(loaded.CreateBackupOnStart, settings.CreateBackupOnStart);
        }

        [TestMethod]
        public async Task AsyncTestAsync()
        {
            //await PausePrintAsync();
            Debug.WriteLine(DateTime.Now.ToString("hh:MM:ss"));
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(PausePrintAsync());
            }
            await Task.WhenAll(tasks);
            Debug.WriteLine("carrying on");
            Debug.WriteLine(DateTime.Now.ToString("hh:MM:ss"));
        }

        public static Task PausePrintAsync()
        {
            Debug.WriteLine(DateTime.Now.ToString("hh:MM:ss"));
            var tcs = new TaskCompletionSource<bool>();
            new Timer(_ =>
            {
                Debug.WriteLine("Hello");
                tcs.SetResult(true);
            }).Change(1000, Timeout.Infinite);
            return tcs.Task;
        }

        [TestMethod]
        public void GetDocuments()
        {
            var startPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileList = new List<string>();
            var pattern = new string[] { "*.txt", "*.doc" };
            foreach (var p in pattern)
            {
                try
                {
                    var files = Directory.GetFiles(startPath, p, SearchOption.AllDirectories);
                    fileList.AddRange(files);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            fileList.ForEach(f => Debug.WriteLine(f));
        }
    }
}
