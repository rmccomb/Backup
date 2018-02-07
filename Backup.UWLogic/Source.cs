using System;

namespace Backup.UWLogic
{
    public class Source
    {
        private string directory;
        private string pattern;
        private string modifiedOnly;
        private DateTime lastBackup;
        public const string NeverText = "Never";

        public string Directory { get => directory; set => directory = value; }
        public string Pattern { get => pattern; set => pattern = value; }
        public string ModifiedOnly { get => modifiedOnly; set => modifiedOnly = value; }
        public DateTime LastBackup { get => lastBackup; set => lastBackup = value; }
        public string LastBackupText => lastBackup == DateTime.MinValue ? NeverText : lastBackup.ToString();
        public bool IsModifiedOnly { get => ModifiedOnly == "Yes"; }
        public Source(string path, string pattern, string modifiedOnly, DateTime? lastBackup = null)
        {
            Directory = path;
            Pattern = pattern;
            ModifiedOnly = modifiedOnly;
            LastBackup = lastBackup == null ? DateTime.MinValue : lastBackup.Value;
        }

        public Source(string path, string pattern, string modifiedOnly, string lastBackupText)
        {
            Directory = path;
            Pattern = pattern;
            ModifiedOnly = modifiedOnly;
            LastBackup = lastBackupText == NeverText ? DateTime.MinValue : DateTime.Parse(lastBackupText);
        }


    }
}

