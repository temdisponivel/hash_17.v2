using System.Text;
using HASH.OS.FileSystem;
using HASH.OS.FileSystem.FileTypes;
using HASH17.Util;
using HASH17.Util.Text;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace Assets._Code.__TRASH
{
    public class GeneralTests : MonoBehaviour
    {
        void Awake()
        {
            TestDir();
            TestFile();
        }

        void TestDir()
        {
            var textUtil = new TextUtilData();
            textUtil.BuilderHelper = new StringBuilder(1024);
            Global.TextUtilData = textUtil;

            var data = new FileSystemData();
            data.PathStackHelper = SList.Create<string>(10);
            Global.FileSystemData = data;

            var dir0 = new HashDir();
            dir0.Name = "/";
            dir0.DirId = 0;
            dir0.ParentDirId = -1;

            var dir1 = new HashDir();
            dir1.Name = "ALO1";
            dir1.DirId = 1;
            dir1.ParentDirId = dir0.DirId;

            var dir2 = new HashDir();
            dir2.Name = "ALO2";
            dir2.DirId = 2;
            dir2.ParentDirId = dir1.DirId;

            var dir3 = new HashDir();
            dir3.DirId = 3;
            dir3.Name = "ALO3";
            dir3.ParentDirId = dir2.DirId;

            data.AllDirectories = STable.Create<int, HashDir>(3, true);
            STable.Add(data.AllDirectories, dir0.DirId, dir0, true);
            STable.Add(data.AllDirectories, dir1.DirId, dir1, true);
            STable.Add(data.AllDirectories, dir2.DirId, dir2, true);
            STable.Add(data.AllDirectories, dir3.DirId, dir3, true);

            var allDirectories = data.AllDirectories;
            foreach (var dir in allDirectories)
                FileSystem.CacheDirFullPath(dir.Value);

            foreach (var dir in allDirectories)
                Debug.Log(dir.Value.FullPath);
        }

        void TestFile()
        {
            var file = new HashFile();
            file = new HashFile();
            file.Extension = "txt";
            file.Name = "read_me";
            file.ParentDirId = 0;
            file.FileId = 0;
            file.FileType = HashFileType.Text;

            var textFile = new TextFile();
            textFile.TextContentAssetPath = "ReadMe";
            textFile.File = file;
            file.Content = textFile;

            var data = Global.FileSystemData;
            data.AllFiles = STable.Create<int, HashFile>(10, true);
            data.AllFiles[file.FileId] = file;

            foreach (var f in data.AllFiles)
                FileSystem.CacheFileContents(f.Value);

            foreach (var f in data.AllFiles)
            {
                Debug.Log(f.Value.FullPath);
                var tf = f.Value.Content as TextFile;
                Debug.Log(tf.TextContent);
            }
        }
    }
}