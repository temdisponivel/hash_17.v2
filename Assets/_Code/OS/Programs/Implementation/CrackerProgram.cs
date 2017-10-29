using SimpleCollections.Util;

namespace HASH
{
    public static class CrackerProgram
    {
        public const string PathArgName = "";
        public const string PasswordArgName = "p";

        public static CommandLineArgValidationOption[] Validations;

        public static CommandLineArgValidationOption PathValidation;
        public static CommandLineArgValidationOption PasswordValidation;

        public static void Setup()
        {
            PathValidation = new CommandLineArgValidationOption();
            PathValidation.ArgumentName = PathArgName;
            PathValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            PasswordValidation = new CommandLineArgValidationOption();
            PasswordValidation.ArgumentName = PasswordArgName;
            PasswordValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            Validations = new[] {PasswordValidation, PathValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                Pair<string, string> pathArg = CommandLineUtil.GetArgumentByName(options.ParsedArguments, PathArgName);
                Pair<string, string> passwordArg = CommandLineUtil.GetArgumentByName(options.ParsedArguments, PasswordArgName);

                var path = pathArg.Value;
                var password = passwordArg.Value;

                HashFile file = FileSystem.FindFileByPath(path);
                if (file == null)
                {
                    var msg = string.Format("The path '{0}' is not a valid file path.", path);
                    ShowErrorMessage(msg);
                }
                else
                {
                    if (file.Status != FileStatus.Encrypted)
                    {
                        var msg = string.Format("The file '{0}' is not encrypt. You can open it using 'open {0}'", path);
                        msg = TextUtil.ApplyNGUIColor(msg, Constants.Colors.Success);
                        TerminalUtil.ShowText(msg);
                    }
                    else
                    {
                        var filePassword = file.Password;
                        if (string.Equals(password, filePassword))
                        {
                            var msg = string.Format("File '{0}' decrypted successfully. Use 'open {0}' to open the file.", path);
                            msg = TextUtil.ApplyNGUIColor(msg, Constants.Colors.Success);
                            TerminalUtil.ShowText(msg);
                            FileSystem.ChangeFileStatus(file, FileStatus.Normal);
                        }
                        else
                        {
                            var msg = "Invalid password.";
                            ShowErrorMessage(msg);
                        }                        
                    }
                }
            }
            else
            {
                var pathResult = (int)PathValidation.ValidationResult;
                var passwordResult = (int) PasswordValidation.ValidationResult;
                if (MathUtil.ContainsFlag(pathResult, (int) ArgValidationResult.NotFound) || 
                    MathUtil.ContainsFlag(pathResult, (int) ArgValidationResult.EmptyValue))
                {
                    var msg = "You need to supply a path. Please use 'help cracker' for more info.";
                    ShowErrorMessage(msg);
                }
                else if (MathUtil.ContainsFlag(passwordResult, (int) ArgValidationResult.NotFound) || 
                         MathUtil.ContainsFlag(passwordResult, (int) ArgValidationResult.EmptyValue))
                {
                    var msg = "You need to supply a password. Please use 'help cracker' for more info.";
                    ShowErrorMessage(msg);
                }
            }
        }

        public static void FillCommandBuffer()
        {
            FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeFile);

            ProgramUtil.AddPrefixToCommandBuffer("open ");
        }

        public static void ShowErrorMessage(string msg)
        {
            msg = TextUtil.Error(msg);
            TerminalUtil.ShowText(msg);
        }
    }
}