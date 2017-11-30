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
        public string AWSS3Bucket { get; set; }
        public bool IsS3BucketEnabled { get; set; }

        public bool IsGlacierEnabled { get; set; }
        public string GlacierVaultName { get; set; }

        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }

        public bool CreateBackupOnStart { get; set; }
        public AWSRegionEndPoint AWSS3Region { get; set; }
        public AWSRegionEndPoint AWSGlacierRegion { get; set; }

    }
}
