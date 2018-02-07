using System;
using System.IO;
using System.Runtime.Serialization;

namespace Backup.UWLogic
{
    [DataContract]
    public class Topic
    {
        [DataMember]
        public string TopicFileName { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string TopicARN { get; set; }
        [DataMember]
        public string JobId { get; set; }
        [DataMember]
        public string QueueARN { get; set; }
        [DataMember]
        public string QueueUrl { get; internal set; }
        [DataMember]
        public GlacierResult Status { get; set; }
        [DataMember]
        public string OutputDirectory { get; set; }
        [DataMember]
        public string ArchiveId { get; internal set; }
        [DataMember]
        public string FileName { get; internal set; }
        [DataMember]
        public DateTime DateRequested { get; internal set; }

        public string GetOutputFile() => Path.Combine(OutputDirectory, FileName ?? TopicFileName);
    }
}
