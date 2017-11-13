using System;
using System.IO;

namespace Backup.Logic
{
    public class FileDetail
    {
        /// <summary>
        /// The path to the file to backup
        /// </summary>
        public string FilePath { get; internal set; }

        /// <summary>
        /// The highest directory which is a source for backup
        /// </summary>
        public string Root { get; internal set; }

        /// <summary>
        /// The part of the path to copy
        /// </summary>
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