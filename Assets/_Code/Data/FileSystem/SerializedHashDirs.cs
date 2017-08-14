using System;
using HASH.OS.FileSystem;
using SimpleCollections.Util;

namespace HASH.Data.FileSystem
{
    /// <summary>
    /// Empty class to "concretize" the class pair in order to serialized it.
    /// </summary>
    [Serializable]
    public class PermissionPair : ClassPair<string, AccessPermission>
    {
    }

    /// <summary>
    /// Serialized version of the hashdir.
    /// </summary>
    [Serializable]
    public struct SerializedHashDir
    {
        public string Name;
        public int DirId;
        public int ParentDirId;
        public int[] ChildsDirId;
        public int[] FilesId;
        public PermissionPair[] UserPermission;
    }
}