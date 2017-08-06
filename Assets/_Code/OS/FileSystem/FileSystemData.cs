using System;
using System.Text;
using SimpleCollections.Hash;
using SimpleCollections.Lists;

namespace HASH.OS.FileSystem
{
    /// <summary>
    /// Store all the file system data.
    /// </summary>
    public class FileSystemData
    {
        public HashDir CurrentDir;

        public SimpleTable<int, HashDir> AllDirectories;
        public SimpleTable<int, HashFile> AllFiles;

        public SimpleList<string> PathStackHelper;
    }
}