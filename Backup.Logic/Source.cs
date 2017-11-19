using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public class Source
    {
        private string directory;
        private string pattern;
        private DateTime lastBackup;

        public string Directory { get => directory; set => directory = value; }
        public string Pattern { get => pattern; set => pattern = value; }
        public DateTime LastBackup { get => lastBackup; set => lastBackup = value; }

        public Source(string path, string pattern, DateTime? lastBackup = null)
        {
            Directory= path;
            Pattern = pattern;
            LastBackup = lastBackup == null ? DateTime.MinValue : lastBackup.Value;
        }
    }
}
