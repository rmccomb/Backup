﻿using Amazon;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Backup.Logic
{
    public class FileManager
    {
        // The name of the setting folder
        public const string SettingsFolder = ".bbackup";
        public const string SettingsName = "bbackup.settings.bin";
        public const string ArchiveFolder = "Archive"; // Default folder in the case of the user not setting one

        // App.config settings keys
        public const string TempDirKey = "tempDir"; // The location of SettingsFolder
        // ...others as req.

        // Files capturing the status of the files
        public const string DiscoveredName = "bbackup.disco.dat";
        public const string ProcessingName = "bbackup.processing.dat";
        public const string CatalogName = "bbackup.catalog.dat";
        public const string DestinationsName = "bbackup.destinations.dat";

        // The list of target directories
        public const string SourcesName = "bbackup.sources.dat";

        #region events
        public static event BackupSuccessHandler BackupSuccess;
        public delegate void BackupSuccessHandler(string successMessage);
        public static event BackupWarningHandler BackupWarning;
        public delegate void BackupWarningHandler(string warningMessage);
        public static event BackupErrorHandler BackupError;
        public delegate void BackupErrorHandler(string errorMessage);
        #endregion

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
                tempDir = Path.Combine(tempDir, SettingsFolder);
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
            CreateSettingsFile(settingsFileName);
            var stream = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            var settings = (DestinationSettings)formatter.Deserialize(stream);
            stream.Close();
            return settings;
        }

        private static void CreateSettingsFile(string settingsFileName)
        {
            if (!File.Exists(settingsFileName))
                SaveSettings(new DestinationSettings());
        }

        static public List<Source> GetSources()
        {
            CreateSourcesFile();
            var lines = File.ReadAllLines(Path.Combine(GetTempDirectory(), SourcesName)).Skip(1);
            return new List<Source>(
                (from l in lines
                 select new Source(
                    path: l.Split(',')[0].Trim(),
                    pattern: l.Split(',')[1].Trim(),
                    lastBackup: DateTime.Parse(l.Split(',')[2].Trim()))
                 ));
        }

        static public void SaveSources(List<Source> sources)
        {
            File.Delete(Path.Combine(GetTempDirectory(), SourcesName));
            CreateSourcesFile(sources);
        }

        /// <summary>
        /// Write the paths of files to backup to the disco file
        /// </summary>
        /// <param name="tempDir">Path to disco file</param>
        /// <param name="sources">List of source directories and search patterns</param>
        /// <param name="fromDate">File must have changed since this date to be included</param>
        static public IEnumerable<FileDetail> DiscoverFiles(
            List<Source> sources,
            string tempDir = null)
        {
            var files = new List<FileDetail>();

            sources.ForEach(s =>
            {
                files.AddRange(GetChangedFiles(s.Directory, s.Pattern, s.LastBackup));
            });

            return files;
        }

        /// <summary>
        /// Scan for files to backup
        /// </summary>
        /// <param name="directory">Root directory</param>
        /// <param name="pattern">File extension e.g. *.*</param>
        /// <param name="fromDate">The date of the last backup</param>
        /// <returns>List of files</returns>
        static public IEnumerable<FileDetail> GetChangedFiles(
            string directory,
            string pattern,
            DateTime fromDate)
        {
            var changed = new List<FileDetail>();
            var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var lastWrite = File.GetLastWriteTime(file);
                if (fromDate < lastWrite)
                    changed.Add(new FileDetail(file, directory));
            }

            return changed;
        }

        public static void SaveDiscoveredFiles(IEnumerable<FileDetail> fileDetails)
        {
            var discoFile = Path.Combine(GetTempDirectory(), DiscoveredName);

            if (File.Exists(discoFile))
                File.Delete(discoFile);

            File.AppendAllLines(discoFile, fileDetails.Select(f => $"{f.FilePath}, {f.SubPath}"));
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
                    writer.WriteLine("Directory, Pattern, LastBackup");

                    if (sources != null)
                    {
                        sources.ForEach(s =>
                        {
                            writer.WriteLine($"{s.Directory}, {s.Pattern}, {s.LastBackup.ToString()}");
                        });
                    }

                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Invoke the whole backup process - get sources, discover files, copy to destination
        /// </summary>
        static public void InvokeBackup()
        {
            try
            {
                var sources = GetSources();
                var files = DiscoverFiles(sources);
                SaveDiscoveredFiles(files);
                CopyFiles();
                UpdateTimestamp(sources);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                BackupError?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Do the backing up of the discovered files
        /// </summary>
        /// <param name="archiveDir">A directory to backup to</param>
        static public void CopyFiles(string archiveDir = null)
        {
            var settings = GetSettings();
            var tempDir = GetTempDirectory();

            var filesName = Path.Combine(tempDir, DiscoveredName);
            if (!File.Exists(filesName))
            {
                BackupWarning?.Invoke("No new or modified files were discovered");
                return; // Nothing to do
            }
            var files = File.ReadAllLines(filesName);
            if (files.Length == 0)
            {
                BackupWarning?.Invoke("No new or modified files were discovered");
                return; // Nothing to do
            }

            if (archiveDir == null)
                archiveDir = settings.FileSystemDirectory;

            // Has user set an archive folder?
            if (String.IsNullOrEmpty(archiveDir))
                archiveDir = Path.Combine(tempDir, ArchiveFolder);

            // Make the archive
            var zipArchivePath = DoCopy(archiveDir, files);

            // Upload the zip archive
            if (settings.IsS3BucketEnabled)
            {
                UploadArchive(settings, zipArchivePath);

                BackupSuccess?.Invoke($"Backup uploaded to S3 Bucket {settings.S3Bucket}");

                // Clean up if the user didn't want the File System option
                if (!settings.IsFileSystemEnabled)
                {
                    File.Delete(zipArchivePath);
                }
            }
        }

        private static void UploadArchive(DestinationSettings settings, string zipPath)
        {
            var region = RegionEndpoint.APSoutheast2; // TODO put in settings?
            var transferUtility = new TransferUtility(
                settings.AWSAccessKeyID, settings.AWSSecretAccessKey, region);

            // Create bucket if not found
            if (!transferUtility.S3Client.DoesS3BucketExist(settings.S3Bucket))
            {
                transferUtility.S3Client.PutBucket(
                    new PutBucketRequest() { BucketName = settings.S3Bucket });
            }

            try
            {
                // Copy zipfile 
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = settings.S3Bucket,
                    FilePath = zipPath,
                    //StorageClass // TODO
                };
                request.UploadProgressEvent += Request_UploadProgressEvent;
                transferUtility.Upload(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string DoCopy(string archiveDir, string[] files)
        {
            int fileCount = 0;
            // Create processing file and copy logging files to processing file
            //var processing = Path.Combine(tempDir, ProcessingName);
            //using (var processWriter = File.CreateText(processing))
            //{
            var uniqueArchiveDir = GetArchiveUniqueName(archiveDir);

            foreach (var line in files)
            {
                try
                {
                    // Copy file and append to processing file
                    var filepath = (from f in line.Split(',') select f.Trim()).First();
                    var subpath = (from f in line.Split(',') select f.Trim()).Last();

                    // Create the subpath in archive
                    var splits = subpath.Split('\\');
                    var subpathDir = "";
                    for (int i = 0; i < splits.Length - 1; i++)
                        subpathDir += "\\" + splits[i];

                    var newDir = Path.Combine(uniqueArchiveDir, subpathDir.TrimStart('\\'));
                    var di = Directory.CreateDirectory(newDir);
                    Debug.Write(di);

                    File.Copy(filepath, Path.Combine(uniqueArchiveDir, subpath.TrimStart('\\')), true);
                    fileCount++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }

            if (fileCount > 0)
            {
                try
                {
                    var zipFileArchive = $"{uniqueArchiveDir}.zip";

                    ZipFile.CreateFromDirectory(uniqueArchiveDir, zipFileArchive, CompressionLevel.Optimal, false);

                    // Clean up folders used to make the archive
                    DeleteZipSource(uniqueArchiveDir);

                    BackupSuccess?.Invoke($"{fileCount} file {(fileCount == 1 ? String.Empty : "s")} copied to {zipFileArchive}");
                    return zipFileArchive;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }

            return null;
        }

        private static void DeleteZipSource(string zipSource)
        {
            DeleteFolderContents(zipSource);
        }

        private static void DeleteFolderContents(string folder)
        {
            var files = Directory.EnumerateFiles(folder);
            foreach (var file in files)
                File.Delete(file);

            var dirs = Directory.EnumerateDirectories(folder);
            foreach (var dir in dirs)
            {
                if (Directory.EnumerateFiles(dir).Count() == 0 && Directory.EnumerateDirectories(dir).Count() == 0)
                {
                    Directory.Delete(dir);
                }
                else
                {
                    DeleteFolderContents(dir);
                }
            }
            Directory.Delete(folder);
        }

        static public string GetArchiveUniqueName(string archiveDir = null)
        {
            return Path.Combine(archiveDir, $"archive_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}");
        }

        /// <summary>
        /// Update the timestamp on the sources file
        /// </summary>
        public static void UpdateTimestamp(List<Source> sources)
        {
            sources.ForEach(s => s.LastBackup = DateTime.Now);
            SaveSources(sources);
        }

        private static void Request_UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            Debug.WriteLine($"{e.PercentDone}% done");
        }
    }
}
