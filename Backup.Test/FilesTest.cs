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
            var path = ConfigurationManager.AppSettings[FileManager.TempDirKey];

            // Get files changed since fromDate
            var sources = FileManager.GetSources(path);

            // Dummy sources
            if (sources.Count == 0)
                sources.Add(new Source(@"C:\Users\Rob\source\repos\Backup\Backup.Test\test files", "*.*"));

            var lastDate = FileManager.GetLastDate(path);
            FileManager.DiscoverFiles(path, sources, lastDate);

            // Iterate discovered files and copy while updating status of file in list
            var archive = ConfigurationManager.AppSettings[FileManager.ArchiveDirKey];
            FileManager.Process(path, archive);


        }

        [TestMethod]
        public void Config()
        {
            FileManager.GetSources(ConfigurationManager.AppSettings[FileManager.TempDirKey]);
            Debug.WriteLine(FileManager.GetLastDate(ConfigurationManager.AppSettings[FileManager.TempDirKey]));
        }

        [TestMethod]
        public void GetSources()
        {
            var path = @"C:\Users\Rob\source\repos\Backup\Backup.Test";
            var sources = FileManager.GetSources(path);
            sources.ForEach(s => Debug.WriteLine($"{s.Directory}, {s.Pattern}"));
        }
    }
}
