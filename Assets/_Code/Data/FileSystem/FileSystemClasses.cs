﻿using System;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
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
    
    /// <summary>
    /// The serialized version of a hashfile.
    /// </summary>
    [Serializable]
    public struct SerializedHashFile
    {
        public string Name;
        public int ParentDirId;
        public int FileId;
        public FileStatus Status;
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

    /// <summary>
    /// Enumerates all possible file status. This is how we know if the player can open a file or not.
    /// </summary>
    public enum FileStatus
    {
        Normal = 0,
        Encrypted = 10,
    }
}