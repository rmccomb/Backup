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

        public string Directory { get => directory; set => directory = value; }
        public string Pattern { get => pattern; set => pattern = value; }

        public Source(string path, string pattern)
        {
            this.Directory= path;
            this.Pattern = pattern;
        }
    }
}
