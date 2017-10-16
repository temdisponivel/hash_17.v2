using HASH;
using SimpleCollections.Hash;

namespace HASH
{
    /// <summary>
    /// Represents a file on our virtual OS.
    /// </summary>
    public class HashFile
    {
        public int ParentDirId;

        public HashDir ParentDir;

        public int FileId;
        public string Name;
        public string FullName;
        public string FullPath;
        public HashFileType FileType;

        public object Content;

        public SimpleTable<string, AccessPermission> UserPermission;
    }
}