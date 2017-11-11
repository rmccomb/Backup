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
        public void GetFiles()
        {
            var path = @"C:\Users\Rob\source\repos\Backup\Backup.Test";
            var fromDate = DateTime.Now;
            var pattern = "*.*";

            var files = FileManager.GetChangedFiles(path, pattern, fromDate);

            files.ForEach(f => Console.WriteLine(f.FilePath));
        }

        [TestMethod]
        public void CreateControlFiles()
        {
            var path = ConfigurationManager.AppSettings[FileManager.SettingsKey];
            FileManager.Initialise(path);
        }

        [TestMethod]
        public void CreateTargetsFile()
        {
        }
    }
}
