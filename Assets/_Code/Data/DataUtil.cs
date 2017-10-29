using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HASH;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HASH
{
    /// <summary>
    /// Utility class for the serialized data.
    /// </summary>
    public static class DataUtil
    {
        #region Properties

        const string FILE_SYSTEM_FOLDER = "Assets/Resources/FileSystem/";
        const string RESOURCE_FOLDER_PATH_IN_PROJECT = "Assets/Resources/";

#if MOCK_DATA
        public const string FileSystemDataPath = "Data/Mock/SerializedFileSystem";
        public const string ProgramsDataPath = "Data/Mock/SerializedPrograms";
#else
        public const string FileSystemDataPath = "Data/SerializedFileSystem";
        public const string ProgramsDataPath = "Data/SerializedPrograms";
#endif

        public static SerializedFileSystem SerializedFileSystem;
        public static SerializedPrograms SerializedPrograms;

        #endregion

        #region Load / Unload

        /// <summary>
        /// Loads all the data into memory.
        /// </summary>
        public static IEnumerator Load()
        {
            yield return LoadFileSystemData();
            yield return LoadProgramsData();
        }

        /// <summary>
        /// Process serialized data and stores at the respective data on the global class.
        /// Only call this after calling (and waiting for) Load.
        /// </summary>
        public static void ProcessLoadedData()
        {
            ProcessFileSystemData();
            ProcessProgramsData();

            CacheData();
        }

        /// <summary>
        /// Process the file system data. Only call this after calling (and waiting for) the LoadFileSystemData.
        /// </summary>
        public static void ProcessFileSystemData()
        {
            var fileSystemData = new FileSystemData();

            fileSystemData.PathStackHelper = SList.Create<string>(10);

            fileSystemData.AllDirectories = STable.Create<int, HashDir>(SerializedFileSystem.Dirs.Length, true);
            var dirs = SerializedFileSystem.Dirs;
            for (int i = 0; i < dirs.Length; i++)
            {
                var serializedDir = dirs[i];
                var dir = HASH.FileSystem.GetDirFromSerializedData(serializedDir);
                fileSystemData.AllDirectories[dir.DirId] = dir;
            }

            var files = SerializedFileSystem;
            fileSystemData.AllFiles = STable.Create<int, HashFile>(files.ImageFiles.Length + files.TextFiles.Length, true);
            for (int i = 0; i < files.TextFiles.Length; i++)
            {
                var txtFile = files.TextFiles[i];
                var file = FileSystem.GetTextFileFromSerializedData(txtFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            for (int i = 0; i < files.ImageFiles.Length; i++)
            {
                var imgFile = files.ImageFiles[i];
                var file = HASH.FileSystem.GetImageFileFromSerializedData(imgFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            DataHolder.FileSystemData = fileSystemData;
        }

        /// <summary>
        /// Loads the programs data. Only call this after calling (and waiting for) the LoadProgramData.
        /// </summary>
        public static void ProcessProgramsData()
        {
            var programData = new ProgramsData();

            programData.AllPrograms = SList.Create<Program>(SerializedPrograms.Programs.Length);
            for (int i = 0; i < SerializedPrograms.Programs.Length; i++)
            {
                var serializedProg = SerializedPrograms.Programs[i];
                var prog = ProgramUtil.GetProgramFromSerializedData(serializedProg);
                programData.AllPrograms[i] = prog;
            }

            programData.ArgNameHelper = SSet.Create<string>(10, false);

            DataHolder.ProgramData = programData;
        }

        /// <summary>
        /// Cache files and dirs data.
        /// </summary>
        public static void CacheData()
        {
            CacheDirData();
            CacheFilesData();
        }

        /// <summary>
        /// Cache dirs.
        /// </summary>
        public static void CacheDirData()
        {
            var dirs = DataHolder.FileSystemData.AllDirectories;
            
            foreach (var dir in dirs)
                FileSystem.CacheDirContent(dir.Value);
        }

        /// <summary>
        /// Cache all files content.
        /// </summary>
        public static void CacheFilesData()
        {
            var files = DataHolder.FileSystemData.AllFiles;
            foreach (var file in files)
                FileSystem.CacheFileContents(file.Value);
        }

        /// <summary>
        /// Unloads memory from data.
        /// </summary>
        public static void Unload()
        {
            Resources.UnloadAsset(SerializedFileSystem);
            Resources.UnloadAsset(SerializedPrograms);
        }

        #endregion

        #region Loading

        /// <summary>
        /// Loads and stores the file system data here.
        /// </summary>
        public static IEnumerator LoadFileSystemData()
        {
            var op = Resources.LoadAsync<SerializedFileSystem>(FileSystemDataPath);
            yield return op;
            SerializedFileSystem = op.asset as SerializedFileSystem;
            DebugUtil.Assert(SerializedFileSystem == null, string.Format("COULD NOT FIND FILE SYSTEM DATA AT {0}", FileSystemDataPath));
        }

        /// <summary>
        /// Loads and stores the programs data here.
        /// </summary>
        public static IEnumerator LoadProgramsData()
        {
            var op = Resources.LoadAsync<SerializedPrograms>(ProgramsDataPath);
            yield return op;
            SerializedPrograms = op.asset as SerializedPrograms;
            DebugUtil.Assert(SerializedFileSystem == null, string.Format("COULD NOT FIND PROGRAMS DATA AT {0}", ProgramsDataPath));
        }

        #endregion

        #region Baking

#if UNITY_EDITOR

        [MenuItem("HASH/Bake file system")]
        public static void BakeFileSystemData()
        {
            var allDirs = new List<SerializedHashDir>();
            var allTextFiles = new List<SerializedHashFileText>();
            var allImageFiles = new List<SerializedHashFileImage>();

            var folders = Directory.GetDirectories(FILE_SYSTEM_FOLDER);
            for (int i = 0; i < folders.Length; i++)
                FillDirsAndFiles(folders[i], -1, allDirs, allTextFiles, allImageFiles);

            // change the name of the root folder to be the unix-like
            var root = allDirs[0];
            root.Name = PathUtil.PathSeparator;
            allDirs[0] = root;

            var fileSystem = Resources.Load<SerializedFileSystem>(FileSystemDataPath);

            fileSystem.Dirs = allDirs.ToArray();
            fileSystem.ImageFiles = allImageFiles.ToArray();
            fileSystem.TextFiles = allTextFiles.ToArray();

            EditorUtility.SetDirty(fileSystem);
            Selection.activeObject = fileSystem;
            
            Debug.Log("Baking done!");
        }

        private static void FillDirsAndFiles(
            string dir,
            int parentId,
            List<SerializedHashDir> allDirs,
            List<SerializedHashFileText> allTextFiles,
            List<SerializedHashFileImage> allImageFiles)
        {
            var parent = new SerializedHashDir();

            parent.Name = Path.GetFileName(dir);
            parent.DirId = MathUtil.GetStringHash(dir);
            parent.ParentDirId = parentId;

            var childs = Directory.GetDirectories(dir);
            var parentDirPath = PathUtil.RemovePathPart(dir, RESOURCE_FOLDER_PATH_IN_PROJECT);
            var files = Resources.LoadAll<HashFileSO>(parentDirPath);
            
            var filesIds = new List<int>();
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileType = file.Type;
                
                var filePath = AssetDatabase.GetAssetPath(file);

                {
                    // We need this validation because Resources.LoadAll will return files on subdirectory as well
                    var filePathRelativeToResource = PathUtil.RemovePathPart(filePath, RESOURCE_FOLDER_PATH_IN_PROJECT);
                    var fileName = Path.GetFileName(filePathRelativeToResource);

                    var fileDirPath = PathUtil.RemovePathPart(filePathRelativeToResource, fileName); 
                    PathUtil.MathPathFashion(ref fileDirPath, ref parentDirPath);
                    if (fileDirPath != parentDirPath)
                        continue;
                }
                
                if (fileType == HashFileType.Invalid)
                    continue;

                var hashFile = new SerializedHashFile();

                var name = file.name;
                hashFile.Name = name; 
                hashFile.FileId = MathUtil.GetStringHash(filePath);
                hashFile.ParentDirId = parent.DirId;
                hashFile.UserPermission = file.Permissions;
                hashFile.Status = file.Status;
                hashFile.Password = file.Password;

                switch (fileType)
                {
                    case HashFileType.Text:
                        var textFile = new SerializedHashFileText();
                        textFile.File = hashFile;
                        
                        textFile.TextAsset = file.Content as TextAsset;
                        if (textFile.TextAsset == null)
                            Debug.LogError("This file [CLICK ME] is marked as text but it's content is not a text asset!", file);

                        allTextFiles.Add(textFile);
                        break;
                    case HashFileType.Image:
                        var imageFile = new SerializedHashFileImage();
                        imageFile.File = hashFile;
                        
                        imageFile.ImageAsset = file.Content as Texture2D;
                        
                        if (imageFile.ImageAsset == null)
                            Debug.LogError("This file [CLICK ME] is marked as image but it's content is not a texture asset!", file);

                        allImageFiles.Add(imageFile);
                        break;
                    default:
                        Debug.LogError("Type " + fileType + " not implemented yet!");
                        break;
                }

                filesIds.Add(hashFile.FileId);
            }

            parent.FilesId = filesIds.ToArray();
            parent.ChildsDirId = new int[childs.Length];

            for (int i = 0; i < childs.Length; i++)
                parent.ChildsDirId[i] = MathUtil.GetStringHash(childs[i]);

            allDirs.Add(parent);

            for (int i = 0; i < childs.Length; i++)
            {
                FillDirsAndFiles(
                    childs[i],
                    parent.DirId,
                    allDirs,
                    allTextFiles,
                    allImageFiles);
            }
        }

#endif

        #endregion
    }
}