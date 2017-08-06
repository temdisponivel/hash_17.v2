using SimpleCollections.Hash;
using SimpleCollections.Lists;

namespace HASH.OS.FileSystem
{
    /// <summary>
    /// Class that represents a directory.
    /// </summary>
    public class HashDir
    {
        public int DirId;
        public string Name;
        public string FullPath;

        public int ParentDirId;
        public SimpleList<int> ChildsDirId;
        public SimpleList<int> FilesId;

        public HashDir ParentDir;
        public SimpleList<HashFile> Files;
        public SimpleList<HashDir> Childs;

        public SimpleTable<int, AccessPermission> UserPermission;
    }
}