using System;
using HASH.Window;

namespace HASH
{
    public static class OpenProgram
    {
        public static CommandLineArgValidationOption<object>[] Validations;
        public static CommandLineArgValidationOption<object> PathValidation;

        public static void Setup()
        {
            PathValidation = new CommandLineArgValidationOption<object>();
            PathValidation.ArgumentName = string.Empty;
            PathValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            Validations = new[] {PathValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                var path = options.ParsedArguments[0].Value;
                HashFile file;
                HashDir dir;
                if (FileSystem.FileExists(path, out file))
                {
                    switch (file.FileType)
                    {
                        case HashFileType.Text:
                            var textFile = file.Content as TextFile;
                            var textContent = textFile.TextContent;

                            // TODO: remove this shit and uncomment line
                            WindowUtil.CreateTextWindow(textContent);
                            //TerminalUtil.ShowText(textContent);
                            break;
                        case HashFileType.Image:
                            var imageFile = file.Content as ImageFile;
                            var imageContent = imageFile.ImageContent;

                            WindowUtil.CreateImageWindow(imageContent);
                            // TerminalUtil.ShowImage(imageContent);
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