using System;

namespace Backup.Logic
{
    public class DownloadObjectInfo
    {
        public Settings Settings { get; set; }
        public string DownloadDirectory { get; set; }
        public string ObjectKey { get; set; }
    }
}
