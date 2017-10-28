using System;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// The serialized version of a hashfile.
    /// </summary>
    [Serializable]
    public struct SerializedHashFile
    {
        public string Name;
        public int ParentDirId;
        public int FileId;
        public PermissionPair[] UserPermission;
    }

    /// <summary>
    /// The serialized version of a hashtextfile.
    /// </summary>
    [Serializable]
    public struct SerializedHashFileText
    {
        public SerializedHashFile File;
        public TextAsset TextAsset;
    }

    /// <summary>
    /// The serialized version of a imagehashfile.
    /// </summary>
    [Serializable]
    public struct SerializedHashFileImage
    {
        public SerializedHashFile File;
        public Texture2D ImageAsset;
    }
}