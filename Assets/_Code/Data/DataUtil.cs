using System.Collections.Generic;
using System.IO;
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
        public const string DEVICES_PATH = "Data/SerializedDevices";
        public const string RESOURCE_FOLDER_PATH_IN_PROJECT = "Assets/Resources/FileSystem/";

        public static void LoadData()
        {
            var devices = LoadDevices();
            DataHolder.DeviceData = DeviceUtil.GetDeviceDataFromSerializedData(devices);
            CacheData();

            DeviceUtil.UpdateDeviceRelatedGUI();
        }

        private static SerializedHashDevices LoadDevices()
        {
            return Resources.Load<SerializedHashDevices>(DEVICES_PATH);
        }

        /// <summary>
        /// Cache files and dirs data.
        /// </summary>
        public static void CacheData()
        {
            var devices = DataHolder.DeviceData.AllDevices;
            for (int i = 0; i < devices.Count; i++)
            {
                var device = devices[i];
                DataHolder.DeviceData.CurrentDevice = device;

                var dirs = device.FileSystem.AllDirectories;
                foreach (var dir in dirs)
                    FileSystem.CacheDirContent(dir.Value);

                var files = device.FileSystem.AllFiles;
                foreach (var file in files)
                    FileSystem.CacheFileContents(file.Value);

                FileSystem.CacheRootDir();
            }

            DataHolder.DeviceData.CurrentDevice = DataHolder.DeviceData.PlayerDevice;
        }

        #region Baking

#if UNITY_EDITOR

        [MenuItem("HASH/Bake devices")]
        public static void BakeDevices()
        {
            var devices = LoadDevices();
            if (devices == null)
                Debug.LogFormat("NO DEVICES TO BAKE. PLEASE PUT A SerializedDevices AT: {0}", DEVICES_PATH);

            var allDevices = devices.Devices;
            for (int i = 0; i < allDevices.Length; i++)
                BakeFileSystemData(allDevices[i]);

            Debug.Log("Baking done!");
            EditorUtility.SetDirty(devices);
            Selection.activeObject = devices;
        }

        public static void BakeFileSystemData(SerializedHashDevice device)
        {
            var allDirs = new List<SerializedHashDir>();
            var allTextFiles = new List<SerializedHashFileText>();
            var allImageFiles = new List<SerializedHashFileImage>();

            var folders = Directory.GetDirectories(RESOURCE_FOLDER_PATH_IN_PROJECT + device.DeviceName);
            for (int i = 0; i < folders.Length; i++)
                FillDirsAndFiles(folders[i], -1, allDirs, allTextFiles, allImageFiles);

            // change the name of the root folder to be the unix-like
            var root = allDirs[0];
            root.Name = PathUtil.PathSeparator;
            allDirs[0] = root;

            var fileSystem = device.FileSystem;

            fileSystem.Dirs = allDirs.ToArray();
            fileSystem.ImageFiles = allImageFiles.ToArray();
            fileSystem.TextFiles = allTextFiles.ToArray();
        }

        private static void FillDirsAndFiles(
            string dir,
            int parentId,
            List<SerializedHashDir> allDirs,
            List<SerializedHashFileText> allTextFiles,
            List<SerializedHashFileImage> allImageFiles)
        {
            var parent = new SerializedHashDir();

            parent.Name = Path.GetFileName(dir).ToLower();
            parent.DirId = MathUtil.GetStringHash(dir);
            parent.ParentDirId = parentId;

            var childs = Directory.GetDirectories(dir);
            var parentDirPath = PathUtil.RemovePathPart(dir, RESOURCE_FOLDER_PATH_IN_PROJECT);
            var files = Resources.LoadAll<HashFileSO>("FileSystem/" + parentDirPath);

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
                    PathUtil.MatchPathFashion(ref fileDirPath, ref parentDirPath);
                    if (fileDirPath != parentDirPath)
                        continue;
                }

                if (fileType == HashFileType.Invalid)
                    continue;

                var hashFile = new SerializedHashFile();

                var name = file.name;
                hashFile.Name = name.ToLower();
                hashFile.FileId = MathUtil.GetStringHash(filePath);
                hashFile.ParentDirId = parent.DirId;
                hashFile.UserPermission = file.Permissions;
                hashFile.Status = file.Status;
                hashFile.Password = file.Password;
                hashFile.Condition = file.Condition;

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