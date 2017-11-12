using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public class Source
    {
        public string Directory;
        public string Pattern;

        public Source(string path, string pattern)
        {
            this.Directory= path;
            this.Pattern = pattern;
        }
    }
}
