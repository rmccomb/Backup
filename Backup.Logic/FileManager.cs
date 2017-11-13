using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public static class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";

        // The configurable location of where the settings for the application live
        public const string TempDirKey = "tempDir";
        public const string ArchiveDirKey = "archiveDir";

        // Files capturing the status of the files
        public const string DiscoName = "bbackup.disco.dat";

        /// <summary>
        /// Get the date of the last backup from file
        /// </summary>
        public static DateTime GetLastDate(string tempDir)
        {
            if (!File.Exists(Path.Combine(tempDir, LastDateName)))
                return DateTime.MinValue;

            using (var reader = File.OpenText(Path.Combine(tempDir, LastDateName)))
            {
                var line = reader.ReadLine();
                return DateTime.Parse(line);
            }
        }

        public const string ProcessingName = "bbackup.processing.dat";
        public const string CatalogName = "bbackup.catalog.dat";
        public const string LastDateName = "bbackup.last.dat";

        // The list of target directories
        public const string SourcesName = "bbackup.sources.dat";

        static public List<Source> GetSources(string tempDir)
        {
            CreateSourcesFile(tempDir);

            var lines = File.ReadAllLines(Path.Combine(tempDir, SourcesName));
            return new List<Source>(
                (from l in lines select new Source(l.Split(',')[0].Trim(), l.Split(',')[1].Trim())).Skip(1));
        }

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
                if (fromDate < lastWrite)
                    changed.Add(new FileDetail { FilePath = file, Root = directory });
            }

            return changed;
        }

        /// <summary>
        /// Write the paths of files to backup to the disco file
        /// </summary>
        /// <param name="tempDir">Path to disco file</param>
        /// <param name="sources">List of source directories and search patterns</param>
        /// <param name="fromDate">File must have changed since this date to be included</param>
        static public void DiscoverFiles(
            string tempDir,
            List<Source> sources,
            DateTime fromDate)
        {
            var discoFile = Path.Combine(tempDir, DiscoName);

            if (File.Exists(discoFile))
                File.Delete(discoFile);

            sources.ForEach(s =>
            {
                var files = GetChangedFiles(s.Directory, s.Pattern, fromDate);
                File.AppendAllLines(discoFile, files.Select(f => $"{f.FilePath}, {f.SubPath}"));
            });
        }
        
        /// <summary>
        /// Create the sources file
        /// </summary>
        /// <param name="tempDir">Location of sources file</param>
        static private void CreateSourcesFile(string tempDir)
        {
            var sources = Path.Combine(tempDir, SourcesName);
            if (!File.Exists(sources))
            {
                using (var writer = File.CreateText(sources))
                {
                    writer.WriteLine("Directory, Pattern");
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Backup files discovered
        /// </summary>
        /// <param name="path">A target directory</param>
        /// <param name="archiveDir">A directory to backup to</param>
        static public void Process(string path, string archiveDir = null)
        {
            var discoFile = Path.Combine(path, DiscoName);
            if (!File.Exists(discoFile))
                return; // Nothing to do

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

            // When complete remove disco file

            #region Write backup date
            if (File.Exists(Path.Combine(path, LastDateName)))
                File.Delete(Path.Combine(path, LastDateName));

            using (var writer = File.CreateText(Path.Combine(path, LastDateName)))
            {
                writer.WriteLine(DateTime.Now);
                writer.Close();
            }
            #endregion
        }
    }
}
