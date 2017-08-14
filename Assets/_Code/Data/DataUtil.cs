using System.Collections;
using HASH.Data.FileSystem;
using HASH.Data.Programs;
using HASH.OS.FileSystem;
using HASH.OS.Programs;
using HASH.OS.Shell;
using HASH.Util;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.Data
{
    /// <summary>
    /// Utility class for the serialized data.
    /// </summary>
    public static class DataUtil
    {
        #region Properties

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
                var dir = OS.FileSystem.FileSystem.GetDirFromSerializedData(serializedDir);
                fileSystemData.AllDirectories[dir.DirId] = dir;
            }

            var files = SerializedFileSystem;
            fileSystemData.AllFiles = STable.Create<int, HashFile>(files.ImageFiles.Length + files.TextFiles.Length, true);
            for (int i = 0; i < files.TextFiles.Length; i++)
            {
                var txtFile = files.TextFiles[i];
                var file = OS.FileSystem.FileSystem.GetTextFileFromSerializedData(txtFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            for (int i = 0; i < files.ImageFiles.Length; i++)
            {
                var imgFile = files.ImageFiles[i];
                var file = OS.FileSystem.FileSystem.GetImageFileFromSerializedData(imgFile);
                fileSystemData.AllFiles[file.FileId] = file;
            }

            Global.FileSystemData = fileSystemData;
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

            programData.ArgNameHelper = SSet.Create<string>(10, true);

            Global.ProgramData = programData;
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
            var dirs = Global.FileSystemData.AllDirectories;
            for (int i = 0; i < dirs.Count; i++)
            {
                var dir = dirs[i];
                OS.FileSystem.FileSystem.CacheDirContent(dir);
            }
        }

        /// <summary>
        /// Cache all files content.
        /// </summary>
        public static void CacheFilesData()
        {
            var files = Global.FileSystemData.AllFiles;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                OS.FileSystem.FileSystem.CacheFileContents(file);
            }
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
    }
}