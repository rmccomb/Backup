using System;

namespace Backup.Logic
{
    [Serializable]
    class Topic
    {
        public string TopicARN { get; set; }
        public string JobId { get; set; }
        public string QueueARN { get; set; }
        public string QueueUrl { get; internal set; }
    }
}
