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
            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                Pair<string, string> pathArg;
                
                // no need to validate this because the argument is required
                CommandLineUtil.TryGetArgumentByName(options.ParsedArguments, PathArgName, out pathArg);
                var path = pathArg.Value;

                bool openOnTerminal = CommandLineUtil.ArgumentExists(options.ParsedArguments, TerminalArgName);
                
                HashFile file;
                HashDir dir;
                if (FileSystem.FileExists(path, out file))
                {
                    switch (file.FileType)
                    {
                        case HashFileType.Text:
                            var textFile = file.Content as TextFile;
                            var textContent = textFile.TextContent;

                            if (openOnTerminal)
                                TerminalUtil.ShowText(textContent);
                            else
                                WindowUtil.CreateTextWindow(textContent);
                            break;
                        case HashFileType.Image:
                            var imageFile = file.Content as ImageFile;
                            var imageContent = imageFile.ImageContent;

                            if (openOnTerminal)
                                TerminalUtil.ShowImage(imageContent);
                            else
                                WindowUtil.CreateImageWindow(imageContent);
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

        public static void FillCommandBuffer()
        {
            FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeFile);

            ProgramUtil.AddPrefixToCommandBuffer("open ");
        }
    }
}