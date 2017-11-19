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
        public const string NeverText = "Never";

        public string Directory { get => directory; set => directory = value; }
        public string Pattern { get => pattern; set => pattern = value; }
        public DateTime LastBackup { get => lastBackup; set => lastBackup = value; }
        public string LastBackupText => lastBackup == DateTime.MinValue ? NeverText : lastBackup.ToString();

        public Source(string path, string pattern, DateTime? lastBackup = null)
        {
            Directory= path;
            Pattern = pattern;
            LastBackup = lastBackup == null ? DateTime.MinValue : lastBackup.Value;
        }

        public Source(string path, string pattern, string lastBackupText) 
        {
            Directory = path;
            Pattern = pattern;
            LastBackup = lastBackupText == NeverText ? DateTime.MinValue : DateTime.Parse(lastBackupText);
        }
    }
}
