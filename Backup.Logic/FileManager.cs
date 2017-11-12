using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";

        // The configurable location of where the settings for the application live
        public const string SettingsKey = "settings";
        public const string ArchiveDirKey = "archive";

        // Files capturing the status of the files
        public const string DiscoName = "bbackup.disco.dat";
        public const string ProcessingName = "bbackup.processing.dat";
        public const string CatalogName = "bbackup.catalog.dat";

        // The list of target directories
        public const string SourcesName = "bbackup.sources.dat";

        /// <summary>
        /// Scan for files to backup
        /// </summary>
        /// <param name="directory">Root directory</param>
        /// <param name="pattern">File extension e.g. *.*</param>
        /// <param name="fromDate">The date of the last backup</param>
        /// <returns>List of files</returns>
        static public List<FileDetail> GetChangedFiles(
            string directory,
            string pattern,
            DateTime fromDate)
        {
            var changed = new List<FileDetail>();
            var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var lastWrite = File.GetLastWriteTimeUtc(file);
                if (lastWrite < fromDate)
                    changed.Add(new FileDetail { FilePath = file, Root = directory });
            }

            return changed;
        }

        static public void DiscoverFiles(
            string discoPath,
            List<Source> sources,
            DateTime fromDate)
        {
            var discoFile = Path.Combine(discoPath, DiscoName);

            if (File.Exists(discoFile))
                File.Delete(discoFile);

            sources.ForEach(s =>
            {
                var files = GetChangedFiles(s.Directory, s.Pattern, fromDate);
                File.AppendAllLines(discoFile, files.Select(f => $"{f.FilePath}, {f.SubPath}"));
            });
        }
        
        /// <summary>
        /// Create the control files sources.dat
        /// </summary>
        /// <param name="path">Location of sources file</param>
        static public void Initialise(string path)
        {
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


        static public void Process(string path, string archiveDir = null)
        {
            var discoFile = Path.Combine(path, DiscoName);
            var discoLines = File.ReadAllLines(discoFile);

            // Create processing file
            var processing = Path.Combine(path, ProcessingName);
            using (var processWriter = File.CreateText(processing))
            {
                foreach (var discoLine in discoLines)
                {
                    try
                    {
                        // Copy file and append to processing file
                        var filepath = (from f in discoLine.Split(',') select f.Trim()).First();
                        var subpath = (from f in discoLine.Split(',') select f.Trim()).Last();

                        // Create the subpath in archive
                        var splits = subpath.Split('\\');
                        var subpathDir = "";
                        for (int i = 0; i < splits.Length - 1; i++)
                            subpathDir += "\\" + splits[i];

                        var newDir = Path.Combine(archiveDir, subpathDir.TrimStart('\\'));
                        var di = Directory.CreateDirectory(newDir);
                        Debug.Write(di);

                        File.Copy(filepath, Path.Combine(archiveDir, subpath.TrimStart('\\')), true);
                        processWriter.WriteLine(discoLine);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw ex;
                    }
                }
            }
        }
    }
}
