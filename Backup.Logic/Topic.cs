using System;

namespace Backup.Logic
{
    [Serializable]
    public class Topic
    {
        public string TopicFileName { get; set; }
        public string TopicARN { get; set; }
        public string JobId { get; set; }
        public string QueueARN { get; set; }
        public string QueueUrl { get; internal set; }
        public GlacierResult Status { get; set; }
        public string OutputPath { get; set; }
    }
}
