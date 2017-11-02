using System;
using HASH.Story;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Enumerates all possible access permissions.
    /// </summary>
    [Flags]
    public enum AccessPermission
    {
        None = 1 << 0,
        Read = 1 << 1,
        Write = 1 << 2,
        All = Read | Write,
    }
    
    /// <summary>
    /// Enumerates all possible file types.
    /// </summary>
    public enum HashFileType
    {
        Invalid = -1,
        Text = 0,
        Image = 5, 
    }
    
    /// <summary>
    /// Represents a text file on our virtual OS.
    /// </summary>
    public class TextFile
    {
        public Ink.Runtime.Story Story;
        public string EncryptedTextContent;
    }
    
    /// <summary>
    /// Represents a image file on our virtual OS.
    /// </summary>
    public class ImageFile
    {
        public Texture2D ImageContent;
    }
    
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

        public string Password;

        public object Content;

        public SimpleTable<string, AccessPermission> UserPermission;

        public FileStatus Status;
        public HashStory.Condition Condition;
    }
    
    
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
    }
    
    /// <summary>
    /// Store all the file system data.
    /// </summary>
    public class FileSystemData
    {
        public HashDir CurrentDir;
        public HashDir RootDir;

        public SimpleTable<int, HashDir> AllDirectories;
        public SimpleTable<int, HashFile> AllFiles;
    }

    public enum FillBufferFileSystemOptions
    {
        IncludeDir,
        IncludeFile,
        IncludeAll,
    }
}