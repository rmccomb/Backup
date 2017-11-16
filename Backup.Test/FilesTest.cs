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
            var tempDir = @"C:\Users\Rob\source\repos\Backup\Backup.Test\test files";

            // Get files changed since fromDate
            var sources = FileManager.GetSources(tempDir);

            // Dummy sources
            if (sources.Count == 0)
                sources.Add(new Source(tempDir, "*.*"));

            var lastDate = FileManager.GetLastDate(tempDir);
            FileManager.DiscoverFiles(sources, lastDate);

            // Iterate discovered files and copy while updating status of file in list
            var archive = @"C:\Users\Rob\source\repos\Backup\archive";
            FileManager.DoBackup(archive);

        }

        [TestMethod]
        public void Config()
        {
            FileManager.GetSources();
            Debug.WriteLine(FileManager.GetLastDate());
        }

        [TestMethod]
        public void GetSources()
        {
            var path = @"C:\Users\Rob\source\repos\Backup\Backup.Test";
            var sources = FileManager.GetSources(path);
            sources.ForEach(s => Debug.WriteLine($"{s.Directory}, {s.Pattern}"));
        }

        [TestMethod]
        public void ReadWriteSettingsFile()
        {
            var settings = new DestinationSettings
            {
                ArchiveDirectory = @"C:\Users\Rob\source\repos\Backup\archive",
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
