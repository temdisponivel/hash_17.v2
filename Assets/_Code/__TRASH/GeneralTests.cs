using System.Text;
using HASH;
using SimpleCollections.Hash;
using SimpleCollections.Lists;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Assets._Code.__TRASH
{
    public class GeneralTests : MonoBehaviour
    {
        void Awake()
        {
            //TestDir();
            //TestFile();
            //TestPaths();
            //TestCommandLine();
            //TestText();
        }

        #region Text

        void TestText()
        {
            string result = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                var list = SList.Create<TextTableColumn>(3);

                var textOpt = new TextTableColumn();
                textOpt.Align = TextTableAlign.Right;
                textOpt.Text = "THIS LEFT IS MY TEXT";
                textOpt.WrapMode = WrapTextMode.AddDots;
                textOpt.Size = textOpt.Text.Length - 10;
                textOpt.PaddingChar = '-';
                textOpt.WeightOnLine = .1f;
                textOpt.ModifyTextOptions.Color = Constants.Colors.Path;
                Debug.Log(TextUtil.FormatTableItem(textOpt));

                SList.Add(list, textOpt);

                textOpt = new TextTableColumn();
                textOpt.Align = TextTableAlign.Center;
                textOpt.Text = "THIS CENTER IS MY TEXT";
                textOpt.WrapMode = WrapTextMode.AddDots;
                textOpt.Size = textOpt.Text.Length - 10;
                textOpt.PaddingChar = '-';
                textOpt.WeightOnLine = .5f;
                textOpt.ModifyTextOptions.Color = Constants.Colors.Error;
                Debug.Log(TextUtil.FormatTableItem(textOpt));

                SList.Add(list, textOpt);

                textOpt = new TextTableColumn();
                textOpt.Align = TextTableAlign.Right;
                textOpt.Text = "THIS RIGHT IS MY TEXT";
                textOpt.WrapMode = WrapTextMode.AddDots;
                textOpt.Size = textOpt.Text.Length - 10;
                textOpt.PaddingChar = '-';
                textOpt.WeightOnLine = .4f;
                textOpt.ModifyTextOptions.Color = Color.yellow;
                Debug.Log(TextUtil.FormatTableItem(textOpt));

                SList.Add(list, textOpt);

                var line = new TextTableLine();
                line.Items = list;
                line.ItemsSeparator = " | ";
                line.AddSeparatorOnEnd = false;
                line.AddSeparatorOnStart = false;
                line.MaxLineSize = 100;
                line.MaxLineSizeIsForced = true;

                result += TextUtil.FormatLineConsideringWeightsAndSize(line) + "\n";
            }

            Debug.Log(result);
            Debug.Log(result.Length);
        }

        #endregion

        #region Commands

        public void TestCommandLine()
        {
            var programData = new ProgramsData();
            programData.AllPrograms = SList.Create<Program>(1);
            programData.ArgNameHelper = SSet.Create<string>(1, false);
            var cdProgram = new Program();
            cdProgram.ProgramType = ProgramType.Cd;
            cdProgram.Commands = new string[2] { "cd", "alo" };
            SList.Add(programData.AllPrograms, cdProgram);

            DataHolder.ProgramData = programData;
            
            var simpleCommand = "cd -p /path/ to/file.txt -c alo - d putamerda -e";
            var command = CommandLineUtil.GetCommandName(simpleCommand);
            var commandArgs = CommandLineUtil.RemoveCommandFromCommandLine(simpleCommand);
            Debug.Log(command);
            Debug.Log(commandArgs);

            var args = CommandLineUtil.GetArgumentsFromCommandLine(commandArgs);
            for (int i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                Debug.Log(arg.Key + ": " + arg.Value);
            }

            var options = Shell.GetProgramExecutionOptions(simpleCommand);
            Debug.Log(options.RawCommandLine);
            Debug.Log(options.ProgramReference);
            Debug.Log(options.ParsedArguments);

            var cdCommandLine = "cd alo -p path -p path";
            Shell.RunCommandLine(cdCommandLine);
        }

        #endregion

        #region Files

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

            DataHolder.FileSystemData.CurrentDir = FileSystem.FindDirByPath("dir1/dir2");

            for (int i = 2; i < paths.Length; i++)
            {
                var pathName = paths[i];
                var dir = FileSystem.FindDirByPath(pathName);
                if (dir != null)
                    Debug.Log(pathName + " FOUND");
                else
                    Debug.Log(pathName + " NOT FOUND");
            }

            var filePaths = new[] { "file1.txt", "file2.txt", "file3.txt" };
            for (int i = 0; i < filePaths.Length; i++)
            {
                var pathName = filePaths[i];
                var dir = FileSystem.FindFileByPath(pathName);
                if (dir != null)
                    Debug.Log(pathName + " FOUND - FILE");
                else
                    Debug.Log(pathName + " NOT FOUND - FILE");
            }
        }

        void TestDir()
        {
            var data = new FileSystemData();
            data.PathStackHelper = SList.Create<string>(10);
            data.AllFiles = STable.Create<int, HashFile>(10, true);
            DataHolder.FileSystemData = data;

            var dir0 = CreateDir(0, "/", -1);

            DataHolder.FileSystemData.RootDir = dir0;
            DataHolder.FileSystemData.CurrentDir = dir0;

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

            var file1 = CreateFile(0, "file1", HashFileType.Text, 0);
            var file2 = CreateFile(1, "file2", HashFileType.Text, 1);
            var file3 = CreateFile(2, "file3", HashFileType.Text, 2);
            data.AllFiles[file1.FileId] = file1;
            data.AllFiles[file2.FileId] = file2;
            data.AllFiles[file3.FileId] = file3;

            foreach (var dataAllFile in data.AllFiles)
                FileSystem.CacheFileContents(dataAllFile.Value);

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

        HashFile CreateFile(int id, string name, HashFileType type, int parentDirId)
        {
            var file = new HashFile();
            file.FileId = id;
            file.Name = name;
            file.FileType = type;
            if (type == HashFileType.Image)
                file.Content = new ImageFile();
            else
            {
                var txt = new TextFile();
                txt.TextContentAssetPath = "ReadMe";
                file.Content = txt;
            }
            file.ParentDirId = parentDirId;
            return file;
        }

        #endregion
    }
}