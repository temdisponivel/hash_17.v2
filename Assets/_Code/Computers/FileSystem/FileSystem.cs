using System;
using System.Text;
using HASH;
using HASH.Story;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using SimpleCollections.Util;
using UnityEngine;
using UnityEngine.Windows.Speech;

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

        public static readonly string[] KnownFileExtensions = {TextFileExtension, ImageFileExtension,};

        #endregion

        #region File System

        public static FileSystemData GetFileSystemFromSerializedData(SerializedFileSystem serializedFileSystem)
        {
            var fileSystemData = new FileSystemData();

            fileSystemData.AllDirectories = STable.Create<int, HashDir>(serializedFileSystem.Dirs.Length, true);
            var dirs = serializedFileSystem.Dirs;
            for (int i = 0; i < dirs.Length; i++)
            {
                var serializedDir = dirs[i];
                var dir = GetDirFromSerializedData(serializedDir);
                fileSystemData.AllDirectories[dir.DirId] = dir;
            }

            var files = serializedFileSystem;
            fileSystemData.AllFiles = STable.Create<int, HashFile>(files.ImageFiles.Length + files.TextFiles.Length, true);
            for (int i = 0; i < files.TextFiles.Length; i++)
            {
                var txtFile = files.TextFiles[i];
                var file = GetTextFileFromSerializedData(txtFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            for (int i = 0; i < files.ImageFiles.Length; i++)
            {
                var imgFile = files.ImageFiles[i];
                var file = GetImageFileFromSerializedData(imgFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            return fileSystemData;
        }

        #endregion

        #region Dir

        /// <summary>
        /// Changes the current dir to the given dir.
        /// </summary>
        public static void ChangeDir(HashDir dir)
        {
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
            data.CurrentDir = dir;

            TerminalUtil.UpdateCurrentPathLabel();
            TerminalUtil.UpdateCommandBuffer();
        }

        /// <summary>
        /// Returns the dir that has the given id.
        /// </summary>
        public static HashDir FindDirById(int dirId)
        {
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
            var dir = STable.Find(data.AllDirectories, dirId);
            return dir;
        }

        /// <summary>
        /// Finds a child of the parent folder by name.
        /// If no child is found, null is returned.
        /// </summary>
        public static HashDir FindChildByName(HashDir parent, string childName)
        {
            var childs = GetAllAvailableChild(parent);
            for (int i = 0; i < childs.Count; i++)
            {
                var child = childs[i];
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

            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
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

            if (currentDir != null)
            {
                if (!IsDirAvaibale(currentDir))
                    return null;
            }

            return currentDir;
        }

        public static void CacheRootDir()
        {
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
            data.RootDir = GetRootDir();
            ChangeDir(data.RootDir);
        }

        /// <summary>
        /// Finds and returns the root dir (the dir with parent id -1).
        /// If no root dir is found, null is returned.
        /// Only call this if the file system data was loaded.
        /// </summary>
        public static HashDir GetRootDir()
        {
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
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

        /// <summary>
        /// Navigate through the given path part considering currentDir as the current folder.
        /// </summary>
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
                    DebugUtil.Log(string.Format("THE DIR {0} HAS ITSELF AS PARENT!", dir.DirId), Color.red, DebugUtil.DebugCondition.Always,
                        DebugUtil.LogType.Info);
                    return null;
                }
#endif
                var pathStack = SList.Create<string>(5);
                SList.Clear(pathStack);
                while (dir.ParentDirId != -1)
                {
                    SList.Push(pathStack, dir.Name);
                    dir = FindDirById(dir.ParentDirId);
                }

                var builder = new StringBuilder(pathStack.Count * 10);
                while (pathStack.Count > 0)
                {
                    var last = SList.Pop(pathStack);
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
                var file = FindFileById(fileId);
                DebugUtil.Assert(file == null, string.Format("THE DIR {0} HAS A INVALID FILE {1}", dir.Name, fileId));

                file.ParentDirId = dir.DirId;
                file.ParentDir = dir;

                SList.Add(dir.Files, file);
            }
        }

        /// <summary>
        /// Cache the dirs children based on the ChildsDirid values.
        /// </summary>
        public static void CacheDirChildren(HashDir dir)
        {
            SList.Clear(dir.Childs);
            for (int i = 0; i < dir.ChildsDirId.Count; i++)
            {
                var childId = dir.ChildsDirId[i];
                var child = FindDirById(childId);

                DebugUtil.Assert(child == null, string.Format("THE DIR {0} HAS A INVALID CHILD {1}", dir.Name, childId));

                child.ParentDirId = dir.DirId;
                child.ParentDir = dir;

                SList.Add(dir.Childs, child);
            }
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
            CacheDirChildren(dir);
            CacheDirFiles(dir);
            CacheDirFullPath(dir);
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

            dir.Childs = SList.Create<HashDir>(dir.ChildsDirId.Count);
            dir.Files = SList.Create<HashFile>(dir.FilesId.Count);

            return dir;
        }

        /// <summary>
        /// Returns a list containing all the files of the given dir that are available for use.
        /// See IsFileAvailable.
        /// </summary>
        public static SimpleList<HashFile> GetAvailableFilesFromDir(HashDir dir)
        {
            var result = SList.Create<HashFile>(1);
            for (int i = 0; i < dir.Files.Count; i++)
            {
                var file = dir.Files[i];
                if (IsFileAvaibale(file))
                    SList.Add(result, file);
            }

            return result;
        }

        public static SimpleList<HashDir> GetAllAvailableChild(HashDir dir)
        {
            var result = SList.Create<HashDir>(dir.Childs.Count);
            for (int i = 0; i < dir.Childs.Count; i++)
            {
                var child = dir.Childs[i];
                if (IsDirAvaibale(child))
                    SList.Add(result, child);
            }

            return result;
        }

        /// <summary>
        /// Returns true if the given dir is avaiable for use.
        /// </summary>
        public static bool IsDirAvaibale(HashDir dir)
        {
            var files = GetAvailableFilesFromDir(dir);
            var childs = GetAllAvailableChild(dir);
            return files.Count > 0 || childs.Count > 0;
        }

        public static HashDir CreateDir(HashDir parent, string name)
        {
            var hashDir = new HashDir();
            hashDir.DirId = MathUtil.GetStringHash(name);
            hashDir.Name = name;
            hashDir.Childs = SList.Create<HashDir>(1);
            hashDir.ChildsDirId = SList.Create<int>(1);
            hashDir.Files = SList.Create<HashFile>(1);
            hashDir.FilesId = SList.Create<int>(1);
            hashDir.ParentDir = parent;
            if (parent != null)
            {
                hashDir.ParentDirId = parent.DirId;
                SList.Add(parent.Childs, hashDir);
                SList.Add(parent.ChildsDirId, hashDir.DirId);
            }
            else
                hashDir.ParentDirId = -1;

            CacheDirFullPath(hashDir);

            return hashDir;
        }

        public static void AddFileToDir(HashDir dir, HashFile file)
        {
            file.ParentDir = dir;
            file.ParentDirId = dir.DirId;

            SList.Add(dir.Files, file);
            SList.Add(dir.FilesId, file.FileId);

            CacheFile(file);
        }

        #endregion

        #region File

        /// <summary>
        /// Returns the file that has the given id.
        /// </summary>
        public static HashFile FindFileById(int fileId)
        {
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;
            HashFile file = STable.Find(data.AllFiles, fileId);
            return file;
        }

        /// <summary>
        /// Returns true if there's a file at path and this file is available for use. Stores the found file at the given parameter.
        /// Returns null if there's no file at the path.
        /// </summary>
        public static bool FileExistsAndIsAvailable(string path, out HashFile file)
        {
            file = FindFileByPath(path);
            return file != null && IsFileAvaibale(file);
        }

        /// <summary>
        /// Returns true there's a file at path. Stores the found file on out file parameter.
        /// </summary>
        public static bool FileExists(string path, out HashFile file)
        {
            file = FindFileByPath(path);
            return file != null;
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
                if (!IsFileAvaibale(file))
                    continue;

                if (string.Equals(file.FullName, fileName, StringComparison.InvariantCultureIgnoreCase))
                    return file;
            }
            return null;
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
            file.Status = serializedFile.Status;
            file.Password = serializedFile.Password;
            file.Condition = serializedFile.Condition;

            file.UserPermission = SList.Create<ClassPair<string, AccessPermission>>(serializedFile.UserPermission.Length);
            for (int i = 0; i < serializedFile.UserPermission.Length; i++)
            {
                var permission = serializedFile.UserPermission[i];
                var user = permission.Key;

                var pair = new ClassPair<string, AccessPermission>();
                pair.Key = user.UserName;
                pair.Value = permission.Value;

                SList.Add(file.UserPermission, pair);
            }

            return file;
        }

        /// <summary>
        /// Creates and returns a text file from the given serialized data.
        /// </summary>
        public static HashFile GetTextFileFromSerializedData(SerializedHashFileText serialized)
        {
            var file = GetFileFromSerializedData(serialized.File);
            file.FileType = HashFileType.Text;
            var txtFile = new TextFile();

            txtFile.Story = new Ink.Runtime.Story(serialized.TextAsset.text);
            StoryUtil.BindExternalFunctions(txtFile.Story);

            txtFile.EncryptedTextContent = TextUtil.EncryptString(txtFile.Story.ToString());
            file.Content = txtFile;
            return file;
        }

        /// <summary>
        /// Creates and returns a image file from the given serialized data.
        /// </summary>
        public static HashFile GetImageFileFromSerializedData(SerializedHashFileImage serialized)
        {
            var file = GetFileFromSerializedData(serialized.File);
            file.FileType = HashFileType.Image;
            var imageFile = new ImageFile();
            imageFile.ImageContent = serialized.ImageAsset;
            file.Content = imageFile;
            return file;
        }

        /// <summary>
        /// Caches the file parent dir.
        /// </summary>
        public static void CacheFileDir(HashFile file)
        {
            var parent = FindDirById(file.ParentDirId);
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
        public static void CacheFile(HashFile file)
        {
            file.FullPath = GetFileFullPath(file);
            file.FullName = GetFileFullName(file);
        }

        /// <summary>
        /// Cache files path, folder and load its contents.
        /// </summary>
        public static void CacheFileContents(HashFile file)
        {
            CacheFile(file);
        }

        /// <summary>
        /// Returns the string content of the given file. 
        /// </summary>
        public static string GetTextFileContent(TextFile file)
        {
            file.Story.ResetState();
            var result = file.Story.ContinueMaximally();
            return result;
        }

        /// <summary>
        /// Returns true if the given file is avaiable for use.
        /// </summary>
        public static bool IsFileAvaibale(HashFile file)
        {
            var result = StoryUtil.EvaluateCondition(file.Condition);
            var permission = GetAccessPermission(file);

            result = result && permission > AccessPermission.Hidden;
            return result;
        }

        public static AccessPermission GetAccessPermission(HashFile file)
        {
            var user = DataHolder.DeviceData.CurrentUser.Username;
            return GetAccessPermissionForUser(file, user);
        }

        public static AccessPermission GetAccessPermissionForUser(HashFile file, string username)
        {
            var permissionPair = SList.Find(file.UserPermission, p => string.Equals(p.Key, username));
            if (permissionPair != null)
                return permissionPair.Value;
            else
                return AccessPermission.Editable;
        }

        private static HashFile CreateBaseFile(HashDir parent, string name)
        {
            var hashFile = new HashFile();

            hashFile.Name = GetValidFileName(parent, name);
            hashFile.FileId = MathUtil.GetStringHash(name);

            hashFile.UserPermission = SList.Create<ClassPair<string, AccessPermission>>(1);
            hashFile.Condition = new HashStory.Condition();
            hashFile.Condition.MinimalDays = HashStory.MainState.CurrentDay;

            AddFileToDir(parent, hashFile);

            return hashFile;
        }

        public static HashFile CreateTextFile(HashDir parent, string name, string text)
        {
            var hashFile = CreateBaseFile(parent, name);
            var textFile = new TextFile();
            textFile.EncryptedTextContent = TextUtil.EncryptString(text);
            textFile.Story = new Ink.Runtime.Story(text);
            hashFile.Content = textFile;
            hashFile.FileType = HashFileType.Text;
            
            CacheFile(hashFile);
            
            return hashFile;
        }

        public static HashFile CreateImageFile(HashDir parent, string name, Texture2D image)
        {
            var hashFile = CreateBaseFile(parent, name);
            var imageFile = new ImageFile();
            imageFile.ImageContent = image;
            hashFile.Content = imageFile;
            hashFile.FileType = HashFileType.Image;
            
            CacheFile(hashFile);
            
            return hashFile;
        }

        public static string GetValidFileName(HashDir dir, string currentName)
        {
            int count = 0;
            for (int i = 0; i < dir.Files.Count; i++)
            {
                var file = dir.Files[i];
                if (file.Name.StartsWith(currentName))
                    count++;
            }
            if (count > 0)
                return string.Format("{0}_{1}", currentName, count);
            else
                return currentName;
        }

        #endregion

        #region Permission

        /// <summary>
        /// Returns the lower of the two given permissions.
        /// </summary>
        public static AccessPermission GetLowerPermissionAccess(AccessPermission permissionA, AccessPermission permissionB)
        {
            return (AccessPermission) Math.Min((int) permissionA, (int) permissionB);
        }

        #endregion

        #region Buffer

        public static void FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions option)
        {
            var commandBuffer = DataHolder.TerminalData.AvailableCommands;
            var data = DataHolder.DeviceData.CurrentDevice.FileSystem;

            SList.Clear(commandBuffer);

            var currentDir = data.CurrentDir;

            if (option == FillBufferFileSystemOptions.IncludeAll || option == FillBufferFileSystemOptions.IncludeDir)
            {
                if (currentDir.ParentDir != null)
                    SList.Add(commandBuffer, "..");

                SList.Add(commandBuffer, ".");

                var childs = GetAllAvailableChild(currentDir);
                for (int i = 0; i < childs.Count; i++)
                    SList.Add(commandBuffer, childs[i].FullPath);
            }

            if (option == FillBufferFileSystemOptions.IncludeAll || option == FillBufferFileSystemOptions.IncludeFile)
            {
                var files = GetAvailableFilesFromDir(currentDir);
                for (int i = 0; i < files.Count; i++)
                    SList.Add(commandBuffer, files[i].FullPath);
            }

            TerminalUtil.ChangeToAvailableCommandsBuffer();
        }

        #endregion

        #region String

        public static string GetWindowTitleForFile(HashFile file)
        {
            string format = "{0}{1}";
            string status = string.Empty;
            if (file.Status != FileStatus.Normal)
                status = string.Format(" - {0}", GetStatusString(file.Status));
            return string.Format(format, file.FullName, status);
        }

        public static string GetStatusString(FileStatus staus)
        {
            return staus.ToString();
        }

        #endregion

        #region Encryption

        public static void ChangeFileStatus(HashFile file, FileStatus newStatus)
        {
            file.Status = newStatus;
        }

        #endregion
    }
}