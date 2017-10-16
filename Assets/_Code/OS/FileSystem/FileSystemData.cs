using System;
using System.Text;
using SimpleCollections.Hash;
using SimpleCollections.Lists;

namespace HASH
{
    /// <summary>
    /// Store all the file system data.
    /// </summary>
    public class FileSystemData
    {
        public HashDir CurrentDir;
        public HashDir RootDir;

        public SimpleTable<int, HashDir> AllDirectories;
        public SimpleTable<int, HashFile> AllFiles;

        public SimpleList<string> PathStackHelper;
    }
}