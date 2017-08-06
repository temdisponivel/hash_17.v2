using System;
using System.Runtime.InteropServices;
using System.Text;
using HASH.OS.FileSystem.FileTypes;
using HASH17.Util;
using HASH17.Util.Text;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.OS.FileSystem
{
    /// <summary>
    /// Class that performs operations related to file system.
    /// </summary>
    public static class FileSystem
    {
        #region Properties

        public const char PathSeparator = '/';

        #endregion

        #region Dir

        /// <summary>
        /// Returns the dir that has the given id.
        /// </summary>
        public static HashDir FindDir(int dirId)
        {
            var data = Global.FileSystemData;
            return STable.Find(data.AllDirectories, dirId);
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
                DebugUtil.Assert(dir.Name != PathSeparator.ToString(), "THERE'S A DIR WITHOUT PARENT ID THAT IS NOT THE ROOT DIR!");
                path = dir.Name;
            }
            else
            {
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
                    builder.Append(AddSeparatorToStart(last));
                }
                path = builder.ToString();

                // since it's a folder, add slash to the end of it
                path = AddSeparatorToEnd(path);
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
        /// Cache the given dir's parent and child dirs.
        /// </summary>
        public static void CacheDirChildAndParent(HashDir dir)
        {
            SList.Clear(dir.Childs);
            dir.ParentDir = null;
            for (int i = 0; i < dir.ChildsDirId.Count; i++)
            {
                var dirId = dir.ChildsDirId[i];
                var child = FindDir(dirId);
                DebugUtil.Assert(child == null, string.Format("THE DIR {0} HAS A INVALID FILE {1}", dir.Name, dirId));
                SList.Add(dir.Childs, child);
            }
            dir.ParentDir = FindDir(dir.ParentDirId);
        }

        /// <summary>
        /// Caches the given dir's parent, childs and files.
        /// </summary>
        public static void CacheDirContent(HashDir dir)
        {
            CacheDirFullPath(dir);
            CacheDirFiles(dir);
            CacheDirChildAndParent(dir);
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
        /// Caches the file parent dir.
        /// </summary>
        public static void CacheFileDir(HashFile file)
        {
            file.ParentDir = FindDir(file.ParentDirId);
        }

        /// <summary>
        /// Returns the file name + extension.
        /// </summary>
        public static string GetFileFullName(HashFile file)
        {
            return string.Format("{0}.{1}", file.Name, file.Extension);
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

        #region Path

        /// <summary>
        /// Returns the concatenated path of parent and child.
        /// </summary>
        public static string ConcatPath(string parentName, string childName)
        {
            return string.Format("{0}{1}{2}", parentName, PathSeparator, childName);
        }

        /// <summary>
        /// Returns true if the given path ends with a path separator.
        /// </summary>
        public static bool HasTrailingSlash(string path)
        {
            return path.EndsWith(PathSeparator.ToString());
        }

        /// <summary>
        /// Adds the path sepator to the start of the path.
        /// </summary>
        public static string AddSeparatorToStart(string path)
        {
            return string.Format("{0}{1}", PathSeparator, path);
        }

        /// <summary>
        /// Adds a path separator to the end of the given path.
        /// </summary>
        public static string AddSeparatorToEnd(string path)
        {
            return string.Format("{0}{1}", path, PathSeparator);
        }

        #endregion
    }
}