using Backup.UWLogic;
using System.Collections.ObjectModel;

namespace Backup.UWApplication.ViewModels
{
    public class SourcesViewModel
    {
        ObservableCollection<Source> _sources = new ObservableCollection<Source>();

        public SourcesViewModel()
        {
            // Initialise
            //this._sources.Add(new Source("C:\\My Documents", "*.*", "Yes"));
            //this._sources.Add(new Source("C:\\Temp", "*.cs", "Yes"));
            //this._sources.Add(new Source("C:\\XX\\Y", "*.txt", "No"));

            foreach (var source in FileManager.GetSources())
            {
                this._sources.Add(source);
            }
        }

        public ObservableCollection<Source> Sources { get => _sources; }
    }
}
