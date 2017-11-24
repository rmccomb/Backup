using System;
using System.Collections.Generic;
using System.IO;

namespace Backup.Logic
{
    [Serializable]
    public class DestinationSettings
    {
        string fileSystemDirectory;
        public string FileSystemDirectory
        {
            get
            { if (String.IsNullOrEmpty(fileSystemDirectory))
                    return Path.Combine(FileManager.GetTempDirectory(), FileManager.ArchiveFolder);
                else
                    return fileSystemDirectory;
            }
            set { this.fileSystemDirectory = value; }
        }
        public bool IsFileSystemEnabled { get; set; }
        public string S3Bucket { get; set; }
        public bool IsS3BucketEnabled { get; set; }
        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }
        public bool CreateBackupOnStart { get; set; }
    }
}
