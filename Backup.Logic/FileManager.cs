using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public class FileManager
    {
        public const string SettingsFolder = ".backup";
        public const string SettingsKey = "settings";
        public const string CatalogName = "catalog.dat";
        public const string SourcesName = "sources.dat"; 

        static public List<FileDetail> GetChangedFiles(string directory,
            string pattern,
            DateTime fromDate)
        {
            var changed = new List<FileDetail>();
            var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var lastWrite = File.GetLastWriteTimeUtc(file);
                if (lastWrite < fromDate)
                    changed.Add(new FileDetail { FilePath = file });
            }

            return changed;
        }
        
        /// <summary>
        /// Create the control files catalog.dat and sources.dat
        /// </summary>
        /// <param name="path">Location of control files</param>
        public static void Initialise(string path)
        {
            var catalog = Path.Combine(path, CatalogName);
            if (!File.Exists(catalog))
            {
                using (var writer = File.CreateText(catalog))
                {
                    writer.WriteLine("Filename, Timestamp");
                    writer.Close();
                }
            }
            var sources = Path.Combine(path, SourcesName);
            if (!File.Exists(sources))
            {
                using (var writer = File.CreateText(sources))
                {
                    writer.WriteLine("Directory, Pattern");
                    writer.Close();
                }
            }

        }
    }
}
