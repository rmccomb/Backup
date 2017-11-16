using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public static class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";
        public const string SettingsName = "bbackup.settings.bin";

        // App.config settings keys
        public const string TempDirKey = "tempDir"; // The location of SettingsFolder
        // ...others as req.

        // Files capturing the status of the files
        public const string DiscoName = "bbackup.disco.dat";
        public const string ProcessingName = "bbackup.processing.dat";
        public const string CatalogName = "bbackup.catalog.dat";
        public const string LastDateName = "bbackup.last.dat";
        public const string DestinationsName = "bbackup.destinations.dat";

        // The list of target directories
        public const string SourcesName = "bbackup.sources.dat";

        /// <summary>
        /// Get the configured temp directory from application settings or, 
        /// if not defined create a default under user's Application Data
        /// </summary>
        static public string GetTempDirectory()
        {
            var tempDir = ConfigurationManager.AppSettings[TempDirKey];
            if (string.IsNullOrEmpty(tempDir))
            {
                tempDir = Directory.GetParent(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    tempDir = Directory.GetParent(tempDir).ToString();
                }

                // Default to our folder if nothing set
                tempDir = Path.Combine(tempDir, FileManager.SettingsFolder);
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }

        public static void SaveSettings(DestinationSettings settings)
        {
            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(GetTempDirectory(), SettingsName);
            File.Delete(settingsFileName);
            var stream = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, settings);
            stream.Close();
        }

        public static DestinationSettings GetSettings()
        {
            var formatter = new BinaryFormatter();
            var settingsFileName = Path.Combine(GetTempDirectory(), SettingsName);
            var stream = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (DestinationSettings)formatter.Deserialize(stream);
            stream.Close();
            return settings;
        }

        /// <summary>
        /// Get the date of the last backup from file
        /// </summary>
        static public DateTime GetLastDate(string tempDir = null)
        {
            if (tempDir == null)
                tempDir = GetTempDirectory();

            if (!File.Exists(Path.Combine(tempDir, LastDateName)))
                return DateTime.MinValue;

            using (var reader = File.OpenText(Path.Combine(tempDir, LastDateName)))
            {
                var line = reader.ReadLine();
                return DateTime.Parse(line);
            }
        }

        static public List<Source> GetSources(string tempDir = null)
        {
            if (tempDir == null)
                tempDir = GetTempDirectory();

            CreateSourcesFile();

            var lines = File.ReadAllLines(Path.Combine(tempDir, SourcesName));
            return new List<Source>(
                (from l in lines select new Source(l.Split(',')[0].Trim(), l.Split(',')[1].Trim())).Skip(1));
        }

        static public void SaveSources(List<Source> sources, string tempDir = null)
        {
            if (tempDir == null)
                tempDir = GetTempDirectory();

            File.Delete(Path.Combine(tempDir, SourcesName));
            CreateSourcesFile(sources);
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
            List<Source> sources,
            DateTime fromDate,
            string tempDir = null)
        {
            if (tempDir == null)
                tempDir = GetTempDirectory();

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
        static private void CreateSourcesFile(List<Source> sources = null)
        {
            string tempDir = GetTempDirectory();

            var sourcesPath = Path.Combine(tempDir, SourcesName);
            if (!File.Exists(sourcesPath))
            {
                using (var writer = File.CreateText(sourcesPath))
                {
                    writer.WriteLine("Directory, Pattern");

                    if (sources != null)
                    {
                        sources.ForEach(s =>
                        {
                            writer.WriteLine($"{s.Directory}, {s.Pattern}");
                        });
                    }

                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Do the backing up of the discovered files
        /// </summary>
        /// <param name="archiveDir">A directory to backup to</param>
        static public void DoBackup(string archiveDir = null)
        {
            var tempDir = GetTempDirectory();

            var discoFile = Path.Combine(tempDir, DiscoName);
            if (!File.Exists(discoFile))
                return; // Nothing to do

            var discoLines = File.ReadAllLines(discoFile);

            // Create processing file
            var processing = Path.Combine(tempDir, ProcessingName);
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
            if (File.Exists(Path.Combine(tempDir, LastDateName)))
                File.Delete(Path.Combine(tempDir, LastDateName));

            using (var writer = File.CreateText(Path.Combine(tempDir, LastDateName)))
            {
                writer.WriteLine(DateTime.Now);
                writer.Close();
            }
            #endregion
        }
    }
}
