﻿using System;
using HASH;

namespace HASH
{
    /// <summary>
    /// Class that handles path.
    /// </summary>
    public static class PathUtil
    {
        public const string PathSeparator = "/";

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
            return path.EndsWith(PathSeparator);
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

        /// <summary>
        /// Returns the path type of the given type.
        /// </summary>
        public static PathType GetPathType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return PathType.Invalid;

            var ext = GetExtensionInPath(path);

            if (string.IsNullOrEmpty(ext))
                return PathType.Folder;
            else
                return PathType.File;
        }

        /// <summary>
        /// Returns the dir path of the given file path.
        /// </summary>
        public static string GetDirPath(string filePath)
        {
            var fileName = GetFileName(filePath);
            return RemovePathPart(filePath, fileName);
        }

        /// <summary>
        /// Returns the extension that the given path ends in.
        /// If there's no extension in the path or the extension is not known, returns empty.
        /// </summary>
        public static string GetExtensionInPath(string path)
        {
            for (int i = 0; i < FileSystem.KnownFileExtensions.Length; i++)
            {
                var ext = FileSystem.KnownFileExtensions[i];
                if (path.EndsWith(string.Format(".{0}", ext), StringComparison.InvariantCultureIgnoreCase))
                    return ext;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the file name (with extention) that the path points to.
        /// If the path do not point to a file, returns empty.
        /// </summary>
        public static string GetFileName(string path)
        {
            if (GetPathType(path) != PathType.File)
                return string.Empty;

            var parts = path.Split(PathSeparator[0]);
            var fileName = parts[parts.Length - 1];
            return fileName;
        }

        /// <summary>
        /// Removes the file name and extension from the given path.
        /// Returns empty is the path is invalid.
        /// </summary>
        public static string RemoveFileNameFromPath(string path)
        {
            var fileName = GetFileName(path);
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
            var resultingPath = path.Replace(fileName, string.Empty);
            // if the remaining path is empty, return a dot to simbilize that it on current dir
            if (string.IsNullOrEmpty(resultingPath))
                return ".";
            else
                return resultingPath;
        }

        /// <summary>
        /// Returns the last dir of the path. The name will be returned without any slashes.
        /// Returns empty if the path is not a dir path.
        /// </summary>
        public static string GetDirName(string path)
        {
            if (GetPathType(path) != PathType.Folder)
                return string.Empty;

            var parts = path.Split(PathSeparator[0]);
            var pathName = parts[parts.Length - 1];

            // if the path ended with the path separator, this will be null
            // in that case we need to get the second to last part of the path
            if (string.IsNullOrEmpty(pathName))
                pathName = parts[parts.Length - 2];

            return pathName;
        }

        /// <summary>
        /// Returns the given file name without any known extentions.
        /// </summary>
        public static string RemoveExtensionFromFileName(string name)
        {
            var ext = GetExtensionInPath(name);
            while (!string.IsNullOrEmpty(ext))
            {
                name = name.Replace(string.Format(".{0}", ext), string.Empty);
                ext = GetExtensionInPath(name);
            }
            return name;
        }

        /// <summary>
        /// Removes the toRemove part from data.
        /// </summary>
        public static string RemovePathPart(string data, string toRemove)
        {
            return data.Replace(toRemove, string.Empty);
        }

        /// <summary>
        /// Returns the file type for the given file extension. This extension is supposed to be the real
        /// file extension (txt, png, jpg, etc).
        /// </summary>
        public static HashFileType GetFileTypeByExtension(string fileExtension)
        {
            if (fileExtension == ".txt")
                return HashFileType.Text;
            else if (fileExtension == ".png")
                return HashFileType.Image;
            else
                return HashFileType.Invalid;
        }

        public static string ToUnityPath(string path)
        {
            return path.Replace("\\", "/");
        }

        public static void MatchPathFashion(ref string pathA, ref string pathB)
        {
            pathA = ToUnityPath(pathA);
            pathB = ToUnityPath(pathB);
            bool finalTrailingA = pathA.EndsWith("/");
            bool finalTrailingB = pathB.EndsWith("/");

            if (finalTrailingA && !finalTrailingB)
                pathB += "/";
            
            if (finalTrailingB && !finalTrailingA)
                pathA += "/";
        }
    }

    /// <summary>
    /// Enumerates the possible file types.
    /// </summary>
    public enum PathType
    {
        Invalid,
        File = 0,
        Folder = 5,
    }
}