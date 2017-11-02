using System;
using HASH.Window;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
{
    public static class OpenProgram
    {
        public const string PathArgName = "";
        public const string TerminalArgName = "t";

        public static CommandLineArgValidationOption[] Validations;
        public static CommandLineArgValidationOption PathValidation;
        public static CommandLineArgValidationOption TerminalValidation;

        public static void Setup()
        {
            PathValidation = new CommandLineArgValidationOption();
            PathValidation.ArgumentName = PathArgName;
            PathValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            TerminalValidation = new CommandLineArgValidationOption();
            TerminalValidation.ArgumentName = TerminalArgName;
            TerminalValidation.Requirements = ArgRequirement.None;

            Validations = new[] {PathValidation, TerminalValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;

            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                Pair<string, string> pathArg;

                // no need to validate this because the argument is required
                CommandLineUtil.TryGetArgumentByName(options.ParsedArguments, PathArgName, out pathArg);
                var path = pathArg.Value;

                bool openOnTerminal = CommandLineUtil.ArgumentExists(options.ParsedArguments, TerminalArgName);

                HashFile file;
                HashDir dir;
                if (FileSystem.FileExistsAndIsAvailable(path, out file))
                {
                    switch (file.FileType)
                    {
                        case HashFileType.Text:
                            var textFile = file.Content as TextFile;
                            OpenTextFile(file, textFile, openOnTerminal);
                            break;
                        case HashFileType.Image:
                            var imageFile = file.Content as ImageFile;
                            OpenImageFile(file, imageFile, openOnTerminal);
                            break;
                        default:
                            DebugUtil.Error(string.Format("The open program can't open file type: {0}", file.FileType));
                            break;
                    }
                }
                else
                {
                    string msg;
                    if (FileSystem.DirExists(path, out dir))
                        msg = "The path '{0}' points to a directory. Please use 'cd {0}' to navigate to that directory.";
                    else
                        msg = "The path '{0}' points to nowhere. Please supply a valida path.";

                    msg = string.Format(msg, path);
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }
            else
            {
                string msg = "Please supply a file path.";
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
        }

        public static void OpenTextFile(HashFile file, TextFile textFile, bool openOnTerminal)
        {
            string textContent;
            if (file.Status == FileStatus.Encrypted)
            {
                ShowEncryptedFileMessage();
                
                textContent = textFile.EncryptedTextContent;
            }
            else
                textContent = FileSystem.GetTextFileContent(textFile);

            if (openOnTerminal)
                TerminalUtil.ShowText(textContent);
            else
            {
                string title = FileSystem.GetWindowTitleForFile(file);
                WindowUtil.CreateTextWindow(textContent, title);
            }
        }

        public static void OpenImageFile(HashFile file, ImageFile imageFile, bool openOnTerminal)
        {
            var imageContent = imageFile.ImageContent;
            
            if (file.Status == FileStatus.Encrypted)
                ShowEncryptedFileMessage();

            if (openOnTerminal)
                TerminalUtil.ShowImage(imageContent);
            else
            {
                string title = FileSystem.GetWindowTitleForFile(file);
                ImageWindowComponent imageWindow = WindowUtil.CreateImageWindow(imageContent, title);

                if (file.Status == FileStatus.Encrypted)
                {
                    var materialPrefab = DataHolder.GUIReferences.EncryptedImageMaterial;

                    var material = new Material(materialPrefab);
                    material.CopyPropertiesFromMaterial(materialPrefab);

                    imageWindow.ImageHolder.material = material;
                    imageWindow.UpdateImageBlendFactor = true;
                    imageWindow.EncryptedImageBlendFactor = 1f;
                }
            }
        }

        public static void FillCommandBuffer()
        {
            FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeFile);

            ProgramUtil.AddPrefixToCommandBuffer("open ");
        }

        public static void ShowEncryptedFileMessage()
        {
            var msg = "This file is encrypted, its encrypted content will be shown. You'll need to decrypted the file to see its real content.\nType 'help cracker' for more info.";
            msg = TextUtil.Warning(msg);
            TerminalUtil.ShowText(msg);
        }
    }
}