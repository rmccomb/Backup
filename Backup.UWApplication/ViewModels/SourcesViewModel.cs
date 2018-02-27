using System;
using System.Collections.ObjectModel;

namespace Backup.UWApplication.ViewModels
{
    public class SourcesViewModel
    {
        ObservableCollection<SourceViewModel> _sources = new ObservableCollection<SourceViewModel>();

        public SourcesViewModel()
        {
            // Initialise
            //this._sources.Add(new SourceViewModel("C:\\My Documents", "*.*", "Yes"));
            //this._sources.Add(new SourceViewModel("C:\\Temp", "*.cs", "Yes"));
            //this._sources.Add(new SourceViewModel("C:\\XX\\Y", "*.txt", "No"));

            foreach (var s in UWLogic.FileManager.GetSources())
            {
                this._sources.Add(new SourceViewModel(s.Directory, s.Pattern, s.ModifiedOnly, s.LastBackup));
            }
        }

        public ObservableCollection<SourceViewModel> Sources { get => _sources; }
    }

    public class SourceViewModel
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
        public SourceViewModel(string path, string pattern, string modifiedOnly, DateTime? lastBackup = null)
        {
            Directory = path;
            Pattern = pattern;
            ModifiedOnly = modifiedOnly;
            LastBackup = lastBackup == null ? DateTime.MinValue : lastBackup.Value;
        }


    }
}
