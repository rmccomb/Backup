using System;

namespace Backup.Logic
{
    public class DownloadObjectInfo
    {
        public DestinationSettings Settings { get; set; }
        public string DownloadDirectory { get; set; }
        public string ObjectKey { get; set; }
    }
}
