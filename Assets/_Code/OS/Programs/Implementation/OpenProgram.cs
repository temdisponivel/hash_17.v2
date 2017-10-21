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
                    var textFile = file.Content as TextFile;
                    var content = textFile.TextContent;
                    TerminalUtil.ShowText(content);
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