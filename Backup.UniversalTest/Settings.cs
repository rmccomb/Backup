using System;
using System.IO;
using System.Runtime.Serialization;

namespace Backup.UniversalTest
{
    [DataContract]
    public class Settings
    {
        [DataMember] public bool IsFileSystemEnabled { get; set; }
        [DataMember] public string AWSS3Bucket { get; set; }
        [DataMember] public bool IsS3BucketEnabled { get; set; }

        [DataMember] public bool IsGlacierEnabled { get; set; }
        [DataMember] public string AWSGlacierVault { get; set; }

        [DataMember] public string AWSAccessKeyID { get; set; }
        [DataMember] public string AWSSecretAccessKey { get; set; }

    }
}
