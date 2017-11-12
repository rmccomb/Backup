using System;
using System.IO;

namespace Backup.Logic
{
    public class FileDetail
    {
        public string FilePath { get; internal set; }

        public string Root { get; internal set; }

        public string SubPath
        {
            get
            {
                var rootsplits = Root.Split('\\');
                var filesplits = FilePath.Split('\\');

                var ret = "";

                for(int i = 0; i < filesplits.Length; i++)
                {
                    if (i < rootsplits.Length - 1 && filesplits[i] == rootsplits[i])
                        continue;

                    ret += '\\' + filesplits[i];
                }

                return ret;
            }
        }
    }
}