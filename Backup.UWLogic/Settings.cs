using System;
using System.IO;
using System.Runtime.Serialization;

namespace Backup.UWLogic
{
    [DataContract]
    public class Settings
    {
        string fileSystemDirectory;
        public string FileSystemDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(fileSystemDirectory))
                    return Path.Combine(FileManager.GetTempDirectory(), FileManager.ArchiveFolder);
                else
                    return fileSystemDirectory;
            }
            set { this.fileSystemDirectory = value; }
        }

        [DataMember] public bool IsFileSystemEnabled { get; set; }
        [DataMember] public string AWSS3Bucket { get; set; }
        [DataMember] public bool IsS3BucketEnabled { get; set; }

        [DataMember] public bool IsGlacierEnabled { get; set; }
        [DataMember] public string AWSGlacierVault { get; set; }

        [DataMember] public string AWSAccessKeyID { get; set; }
        [DataMember] public string AWSSecretAccessKey { get; set; }

        [DataMember] public bool CreateBackupOnStart { get; set; }
        [DataMember] public AWSRegionEndPoint AWSS3Region { get; set; }
        [DataMember] public AWSRegionEndPoint AWSGlacierRegion { get; set; }

        [DataMember] public string SMSContact { get; set; }
        [DataMember] public bool IsSMSContactEnabled { get; set; }

        [DataMember] public DateTime InventoryUpdateRequested { get; set; }

        [DataMember] public bool IsBackupOnLogoff { get; set; }

        [DataMember] public bool IsLaunchOnLogon { get; set; }
    }
}
