using System;
using System.Text;
using HASH;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Class that performs operations related to file system.
    /// </summary>
    public static class FileSystem
    {
        #region Properties

        public const string TextFileExtension = "txt";
        public const string ImageFileExtension = "img";

        public static readonly string[] KnownFileExtensions = { TextFileExtension, ImageFileExtension, };

        #endregion

        #region Dir

        /// <summary>
        /// Changes the current dir to the given dir.
        /// </summary>
        public static void ChangeDir(HashDir dir)
        {
            var data = Global.FileSystemData;
            data.CurrentDir = dir;

            TerminalUtil.UpdateCurrentPathLabel();
            TerminalUtil.UpdateCommandBuffer();
        }

        /// <summary>
        /// Returns the dir that has the given id.
        /// </summary>
        public static HashDir FindDir(int dirId)
        {
            var data = Global.FileSystemData;
            return STable.Find(data.AllDirectories, dirId);
        }

        /// <summary>
        /// Finds a child of the parent folder by name.
        /// If no child is found, null is returned.
        /// </summary>
        public static HashDir FindChildByName(HashDir parent, string childName)
        {
            for (int i = 0; i < parent.Childs.Count; i++)
            {
                var child = parent.Childs[i];
                if (string.Equals(child.Name, childName, StringComparison.InvariantCultureIgnoreCase))
                    return child;
            }
            return null;
        }

        /// <summary>
        /// Returns true if the dir exists. False otherwise.
        /// Stores the dir (if exists) or null on the dir parameter.
        /// </summary>
        public static bool DirExists(string path, out HashDir dir)
        {
            dir = FindDirByPath(path);
            return dir != null;
        }

        /// <summary>
        /// Returns the directory at the given path. Returns null if did not found a dir.
        /// </summary>
        public static HashDir FindDirByPath(string path)
        {
            if (PathUtil.GetPathType(path) != PathType.Folder)
                return null;

            var data = Global.FileSystemData;
            HashDir currentDir;

            // If a path starts with the separator, it means it starts on the root folder
            if (path.StartsWith(PathUtil.PathSeparator))
            {
                currentDir = data.RootDir;

                // remove the path separator because we are already on the root folder
                path = path.Remove(0, 1);
            }
            else
                currentDir = data.CurrentDir;

            var builder = new StringBuilder(path.Length);
            for (int i = 0; i < path.Length; i++)
            {
                var current = path[i];
                if (current == PathUtil.PathSeparator[0])
                {
                    currentDir = ProcessPathPart(currentDir, builder.ToString());
                    TextUtil.ClearBuilder(builder);
                    // if currentDir is null, it's a invalid path
                    if (currentDir == null)
                        return null;
                }
                else
                    builder.Append(current);
            }

            // If there's still something to process
            // this can happen if the path did NOT ended in a path separator
            if (builder.Length > 0)
            {
                currentDir = ProcessPathPart(currentDir, builder.ToString());
                TextUtil.ClearBuilder(builder);
            }

            return currentDir;
        }

        /// <summary>
        /// Finds and returns the root dir (the dir with parent id -1).
        /// If no root dir is found, null is returned.
        /// Only call this if the file system data was loaded.
        /// </summary>
        public static HashDir GetRootDir()
        {
            var data = Global.FileSystemData;
            if (data.RootDir != null)
                return data.RootDir;

            foreach (var dir in data.AllDirectories)
            {
                if (dir.Value.ParentDirId == -1)
                    return dir.Value;
            }

            DebugUtil.Error("Did not found any root folder!");

            return null;
        }

        private static HashDir ProcessPathPart(HashDir currentDir, string pathPart)
        {
            var folderName = pathPart;

            // If we should go up
            if (folderName == "..")
                currentDir = currentDir.ParentDir;
            // if it's NOT to stay on the current folder
            else if (folderName != ".")
                currentDir = FindChildByName(currentDir, folderName);

            return currentDir;
        }

        /// <summary>
        /// Shorthand for hashDir.FullPath = GetDirFullPath(hashDir)
        /// </summary>
        public static void CacheDirFullPath(HashDir hashDir)
        {
            hashDir.FullPath = GetDirFullPath(hashDir);
        }

        /// <summary>
        /// Calculates and return the full path of the dir.
        /// </summary>
        public static string GetDirFullPath(HashDir hashDir)
        {
            var dir = hashDir;
            string path;

            // Root folder is treated differently
            if (dir.ParentDirId == -1)
            {
                DebugUtil.Assert(dir.Name != PathUtil.PathSeparator, string.Format("THE DIR {0} HAS NO PARENT AND IT'S THE ROOT DIR!", dir.DirId));
                path = dir.Name;
            }
            else
            {
#if DEB
                if (dir.ParentDirId == dir.DirId)
                {
                    DebugUtil.Log(string.Format("THE DIR {0} HAS ITSELF AS PARENT!", dir.DirId), Color.red, DebugUtil.DebugCondition.Always, DebugUtil.LogType.Info);
                    return null;
                }
#endif
                var data = Global.FileSystemData;
                SList.Clear(data.PathStackHelper);
                while (dir.ParentDirId != -1)
                {
                    SList.Push(data.PathStackHelper, dir.Name);
                    dir = FindDir(dir.ParentDirId);
                }

                var builder = new StringBuilder(data.PathStackHelper.Count * 10);
                while (data.PathStackHelper.Count > 0)
                {
                    var last = SList.Pop(data.PathStackHelper);
                    builder.Append(PathUtil.AddSeparatorToStart(last));
                }
                path = builder.ToString();

                // since it's a folder, add slash to the end of it
                path = PathUtil.AddSeparatorToEnd(path);
            }

            return path;
        }

        /// <summary>
        /// Returns a list containing all of the dir files.
        /// </summary>
        public static void CacheDirFiles(HashDir dir)
        {
            SList.Clear(dir.Files);
            for (int i = 0; i < dir.FilesId.Count; i++)
            {
                var fileId = dir.FilesId[i];
                var file = FindFile(fileId);
                DebugUtil.Assert(file == null, string.Format("THE DIR {0} HAS A INVALID FILE {1}", dir.Name, fileId));
                SList.Add(dir.Files, file);
            }
        }

        /// <summary>
        /// Cache the parent of the given dir. This will add this dir as child of the its parent
        /// and store its parent on the parent property.
        /// </summary>
        public static void CacheParentDir(HashDir dir)
        {
            SList.Clear(dir.Childs);

            var parent = FindDir(dir.ParentDirId);
            if (parent != null)
                AddAsChild(parent, dir);
        }

        /// <summary>
        /// Adds the child to the list of childs of parent.
        /// Also adds the parent as the parent of the child.
        /// </summary>
        public static void AddAsChild(HashDir parent, HashDir child)
        {
            child.ParentDir = parent;
            SList.Add(parent.Childs, child);
            SList.Add(parent.ChildsDirId, child.DirId);
        }

        /// <summary>
        /// Caches the given dir's parent, childs and files.
        /// </summary>
        public static void CacheDirContent(HashDir dir)
        {
            CacheDirFullPath(dir);
            CacheDirFiles(dir);
            CacheParentDir(dir);
        }

        /// <summary>
        /// Creates and returns a dir from the given serialized data.
        /// </summary>
        public static HashDir GetDirFromSerializedData(SerializedHashDir serializedDir)
        {
            var dir = new HashDir();

            dir.DirId = serializedDir.DirId;
            dir.ParentDirId = serializedDir.ParentDirId;
            dir.Name = serializedDir.Name;
            dir.ChildsDirId = SList.FromArray(serializedDir.ChildsDirId);
            dir.FilesId = SList.FromArray(serializedDir.FilesId);
            dir.UserPermission = STable.Create<string, AccessPermission>(serializedDir.UserPermission.Length, true);

            dir.Childs = SList.Create<HashDir>(dir.ChildsDirId.Count);
            dir.Files = SList.Create<HashFile>(dir.FilesId.Count);

            for (int i = 0; i < serializedDir.UserPermission.Length; i++)
            {
                var permission = serializedDir.UserPermission[i];
                dir.UserPermission[permission.Key] = permission.Value;
            }

            return dir;
        }

        #endregion

        #region File

        /// <summary>
        /// Returns the file that has the given id.
        /// </summary>
        public static HashFile FindFile(int fileId)
        {
            var data = Global.FileSystemData;
            HashFile file;
            if (STable.TryGetValue(data.AllFiles, fileId, out file))
                return file;
            return null;
        }

        /// <summary>
        /// Finds and returns the path at the given file.
        /// Null if the path is invalid, not a file path or the file is not found.
        /// </summary>
        public static HashFile FindFileByPath(string filePath)
        {
            var folderPath = PathUtil.RemoveFileNameFromPath(filePath);
            var fileName = PathUtil.GetFileName(filePath);
            var fileDir = FindDirByPath(folderPath);
            if (fileDir == null)
                return null;

            return FindFileInDir(fileDir, fileName);
        }

        /// <summary>
        /// Finds the file with the given name on the given dir.
        /// Returns null if the file is not found.
        /// </summary>
        public static HashFile FindFileInDir(HashDir dir, string fileName)
        {
            var files = dir.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (string.Equals(file.FullName, fileName, StringComparison.InvariantCultureIgnoreCase))
                    return file;
            }
            return null;
        }

        /// <summary>
        /// Loads the file content into their content property.
        /// </summary>
        public static void LoadFileContent(HashFile file)
        {
            DebugUtil.Assert(file == null, "THE GIVEN FILE IS NULL!");

            switch (file.FileType)
            {
                case HashFileType.Text:
                    LoadTextFileContent(file.Content as TextFile);
                    break;
                case HashFileType.Image:
                    LoadImageFileContent(file.Content as ImageFile);
                    break;
                default:
                    DebugUtil.Assert(true, "PLEASE IMPLEMENT THE LOADING OF " + file.FileType);
                    break;
            }
        }

        /// <summary>
        /// Creates and returns a file from the given serialized data.
        /// </summary>
        public static HashFile GetFileFromSerializedData(SerializedHashFile serializedFile)
        {
            var file = new HashFile();
            file.FileId = serializedFile.FileId;
            file.Name = serializedFile.Name;
            file.ParentDirId = serializedFile.ParentDirId;

            file.UserPermission = STable.Create<string, AccessPermission>(serializedFile.UserPermission.Length, true);
            for (int i = 0; i < serializedFile.UserPermission.Length; i++)
            {
                var permission = serializedFile.UserPermission[i];
                file.UserPermission[permission.Key] = permission.Value;
            }

            return file;
        }

        /// <summary>
        /// Loads the content of a text file into their Content property.
        /// </summary>
        public static void LoadTextFileContent(TextFile textFile)
        {
            DebugUtil.Assert(textFile == null, "THE GIVEN TEXT FILE IS NULL!");

            var textAsset = ContentUtil.Load<TextAsset>(textFile.TextContentAssetPath);
            textFile.TextContent = textAsset.text;
            ContentUtil.Unload(textAsset);
        }

        /// <summary>
        /// Creates and returns a text file from the given serialized data.
        /// </summary>
        public static HashFile GetTextFileFromSerializedData(SerializedHashFileText serialized)
        {
            var file = GetFileFromSerializedData(serialized.File);
            file.FileType = HashFileType.Text;
            var txtFile = new TextFile();
            txtFile.TextContentAssetPath = serialized.TextAssetPath;
            file.Content = txtFile;
            return file;
        }

        /// <summary>
        /// Loads the content of the given image file.
        /// </summary>
        public static void LoadImageFileContent(ImageFile imageFile)
        {
            DebugUtil.Assert(imageFile == null, "THE GIVEN IMAGE FILE IS NULL!");

            var texture = ContentUtil.Load<Texture>(imageFile.ImageContentAssetPath);
            imageFile.ImageContent = texture;

            // Do not unload texture (like we unload text asset)
        }

        /// <summary>
        /// Creates and returns a image file from the given serialized data.
        /// </summary>
        public static HashFile GetImageFileFromSerializedData(SerializedHashFileImage serialized)
        {
            var file = GetFileFromSerializedData(serialized.File);
            file.FileType = HashFileType.Image;
            var imageFile = new ImageFile();
            imageFile.ImageContentAssetPath = serialized.ImageAssetPath;
            file.Content = imageFile;
            return file;
        }

        /// <summary>
        /// Caches the file parent dir.
        /// </summary>
        public static void CacheFileDir(HashFile file)
        {
            var parent = FindDir(file.ParentDirId);
            AddAsFile(parent, file);
        }

        /// <summary>
        /// Adds the given file to the list of files.
        /// </summary>
        public static void AddAsFile(HashDir dir, HashFile file)
        {
            SList.Add(dir.FilesId, file.FileId);
            SList.Add(dir.Files, file);
            file.ParentDir = dir;
        }

        /// <summary>
        /// Returns the file name + extension.
        /// </summary>
        public static string GetFileFullName(HashFile file)
        {
            return string.Format("{0}.{1}", file.Name, GetFileExtension(file.FileType));
        }

        /// <summary>
        /// Returns the file extension to the given file type.
        /// </summary>
        public static string GetFileExtension(HashFileType fileType)
        {
            switch (fileType)
            {
                case HashFileType.Text:
                    return TextFileExtension;
                case HashFileType.Image:
                    return ImageFileExtension;
                default:
                    DebugUtil.Assert(true, "DID NOT FOUND AN EXTENSION FOR THE FILE TYPE: " + fileType);
                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// Calculates and returns the file full path.
        /// </summary>
        public static string GetFileFullPath(HashFile file)
        {
            var parentDirFullPath = GetDirFullPath(file.ParentDir);
            var fullName = GetFileFullName(file);

            var path = string.Format("{0}{1}", parentDirFullPath, fullName); // parentDirFullPath already has a trailling slash
            return path;
        }

        /// <summary>
        /// Caches the file full path and full name.
        /// </summary>
        public static void CacheFilePaths(HashFile file)
        {
            file.FullPath = GetFileFullPath(file);
            file.FullName = GetFileFullName(file);
        }

        /// <summary>
        /// Cache files path, folder and load its contents.
        /// </summary>
        public static void CacheFileContents(HashFile file)
        {
            CacheFileDir(file);
            CacheFilePaths(file);
            LoadFileContent(file);
        }

        #endregion

        #region Permission

        /// <summary>
        /// Returns the lower of the two given permissions.
        /// </summary>
        public static AccessPermission GetLowerPermissionAccess(AccessPermission permissionA, AccessPermission permissionB)
        {
            return (AccessPermission)Math.Min((int)permissionA, (int)permissionB);
        }

        #endregion
        
        #region Buffer

        public static void FillCommandBufferWithAvailableDirectories()
        {
            var commandBuffer = Global.TerminalReferences.AvailableCommands;
            var data = Global.FileSystemData;
            
            SList.Clear(commandBuffer);

            var currentDir = data.CurrentDir;
            
            if (currentDir.ParentDir != null)
                SList.Add(commandBuffer, "..");
            
            SList.Add(commandBuffer, ".");

            var childs = currentDir.Childs;
            for (int i = 0; i < childs.Count; i++)
                SList.Add(commandBuffer, childs[i].FullPath);

            var files = currentDir.Files;
            for (int i = 0; i < files.Count; i++)
                SList.Add(commandBuffer, files[i].FullPath);
            
            TerminalUtil.ChangeToAvailableCommandsBuffer();
        }
        
        #endregion
    }
}