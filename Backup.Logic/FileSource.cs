using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    public class FileSource
    {
        public string UniqueArchiveDir { get; set; }
        public string ZipFileArchive => $"{UniqueArchiveDir}.zip"; 
        public int FileCount { get; set; }
    }
}
