using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backup.Logic;
using System.Configuration;
using System.Diagnostics;

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
                    new Source(@"C:\Users\Rob\source\repos\Backup\Backup.Test\test files\", "*.*"));

            FileManager.SaveDiscoveredFiles(FileManager.DiscoverFiles(sources));

            // Iterate discovered files and copy while updating status of file in list
            var archive = @"C:\Users\Rob\source\repos\Backup\Backup.Test\archive";
            FileManager.DoBackup(archive);

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
            var settings = new DestinationSettings
            {
                ArchiveDirectory = @"C:\Users\Rob\source\repos\Backup\Backup.Test\archive",
                AWSProfileName = "default",
                AWSAccessKeyID = "AKIAIK5CONHAOVWPN27Q",
                AWSSecretAccessKey = "test123",
                S3Bucket = "backup.tiz.digital"
            };
            FileManager.SaveSettings(settings);

            var loaded = FileManager.GetSettings();
            Assert.AreEqual(loaded.ArchiveDirectory, settings.ArchiveDirectory);
            Assert.AreEqual(loaded.AWSAccessKeyID, settings.AWSAccessKeyID);
            Assert.AreEqual(loaded.AWSProfileName, settings.AWSProfileName);
            Assert.AreEqual(loaded.AWSSecretAccessKey, settings.AWSSecretAccessKey);

        }
    }
}
