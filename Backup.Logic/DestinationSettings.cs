using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    [Serializable]
    public class DestinationSettings
    {
        public string ArchiveDirectory { get; set; }
        public string S3Bucket { get; set; }
        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }
    }
}
