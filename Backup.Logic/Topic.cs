using System;
using System.IO;

namespace Backup.Logic
{
    [Serializable]
    public class Topic
    {
        public string TopicFileName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string TopicARN { get; set; }

        public string JobId { get; set; }
        // TODO Check job expiry and delete topic?

        public string QueueARN { get; set; }
        public string QueueUrl { get; internal set; }
        public GlacierResult Status { get; set; }
        public string OutputDirectory { get; set; }
        public string ArchiveId { get; internal set; }
        public string FileName { get; internal set; }

        public string GetOutputFile() => Path.Combine(OutputDirectory, FileName ?? TopicFileName);
    }
}
