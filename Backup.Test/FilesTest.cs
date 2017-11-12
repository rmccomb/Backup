using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backup.Logic;
using System.Configuration;

namespace Backup.Test
{
    [TestClass]
    public class FileTest
    {
        [TestMethod]
        public void Process()
        {
            var path = @"C:\Users\Rob\source\repos\Backup\Backup.Test";

            // Create control files
            FileManager.Initialise(path);

            // Get files changed since fromDate
            var sources = new List<Source>() { new Source(path, "*.*") };
            FileManager.DiscoverFiles(path, sources, DateTime.Now);
            //files.ForEach(f => Console.WriteLine(f.FilePath));

            //FileManager.AppendFiles(path, files);

            // Iterate list and copy while updating status of file in list
            var archive = ConfigurationManager.AppSettings[FileManager.ArchiveDirKey];
            FileManager.Process(path, archive);

            // When complete remove processing file and disco files

        }

        [TestMethod]
        public void Config()
        {
            FileManager.Initialise(ConfigurationManager.AppSettings[FileManager.SettingsKey]);
        }

        [TestMethod]
        public void Test()
        {
        }
    }
}
