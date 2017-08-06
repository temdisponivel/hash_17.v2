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
            //TestFile();

            TestPaths();
        }

        void TestPaths()
        {
            var paths = new[] { "/alo", "/dir1/", "../dir1/dir2/", "../dir3/" };

            for (int i = 0; i < 2; i++)
            {
                var pathName = paths[i];
                var dir = FileSystem.FindDirByPath(pathName);
                if (dir != null)
                    Debug.Log(pathName + " FOUND");
                else
                    Debug.Log(pathName + " NOT FOUND");
            }

            Global.FileSystemData.CurrentDir = FileSystem.FindDirByPath(paths[1]);

            for (int i = 2; i < paths.Length; i++)
            {
                var pathName = paths[i];
                var dir = FileSystem.FindDirByPath(pathName);
                if (dir != null)
                    Debug.Log(pathName + " FOUND");
                else
                    Debug.Log(pathName + " NOT FOUND");
            }
        }

        void TestDir()
        {
            var textUtil = new TextUtilData();
            textUtil.BuilderHelper = new StringBuilder(1024);
            Global.TextUtilData = textUtil;


            var data = new FileSystemData();
            data.PathStackHelper = SList.Create<string>(10);
            data.AllFiles = STable.Create<int, HashFile>(10, true);
            Global.FileSystemData = data;

            var dir0 = CreateDir(0, "/", -1);

            Global.FileSystemData.RootDir = dir0;
            Global.FileSystemData.CurrentDir = dir0;

            var dir1 = CreateDir(1, "dir1", 0);
            var dir2 = CreateDir(2, "dir2", 1);
            var dir3 = CreateDir(3, "dir3", 0);

            data.AllDirectories = STable.Create<int, HashDir>(3, true);
            STable.Add(data.AllDirectories, dir0.DirId, dir0, true);
            STable.Add(data.AllDirectories, dir1.DirId, dir1, true);
            STable.Add(data.AllDirectories, dir2.DirId, dir2, true);
            STable.Add(data.AllDirectories, dir3.DirId, dir3, true);

            var allDirectories = data.AllDirectories;
            foreach (var dir in allDirectories)
                FileSystem.CacheDirContent(dir.Value);
        }

        HashDir CreateDir(int id, string name, int parentId)
        {
            var dir = new HashDir();
            dir.DirId = id;
            dir.Name = name;
            dir.Childs = SList.Create<HashDir>(1);
            dir.ChildsDirId = SList.Create<int>(1);
            dir.ParentDirId = parentId;
            dir.Files = SList.Create<HashFile>(1);
            dir.FilesId = SList.Create<int>(1);
            return dir;
        }

        void TestFile()
        {
            var file = new HashFile();
            file = new HashFile();
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