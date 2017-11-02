using HASH;
using SimpleCollections.Lists;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Executes the CD program.
    /// CD stands for change directory and that's what this program does.
    /// </summary>
    public static class CdProgram
    {
        public static CommandLineArgValidationOption[] Validations;
        public static CommandLineArgValidationOption PathValidation;
        
        public static void Setup()
        {
            PathValidation = new CommandLineArgValidationOption();
            PathValidation.ArgumentName = string.Empty;
            PathValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;
            
            Validations = new CommandLineArgValidationOption[1];
            Validations[0] = PathValidation;
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;
            
            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                var path = options.ParsedArguments[0].Value;

                HashDir dir;
                HashFile file;

                if (FileSystem.DirExists(path, out dir))
                    FileSystem.ChangeDir(dir);
                else if (FileSystem.FileExistsAndIsAvailable(path, out file))
                {
                    var msg = string.Format("The path '{0}' points to a file. Use 'open {0}' to open this file.", path);
                    msg = TextUtil.Warning(msg);
                    TerminalUtil.ShowText(msg);
                }
                else
                {
                    var msg = string.Format("The path '{0}' points nowhere. Please supply a valid path.", path);
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }
            else
            {
                string msg = null;
                var result = PathValidation.ValidationResult;
                if (MathUtil.ContainsFlag((int) result, (int) ArgValidationResult.EmptyValue))
                    msg = "Please supply a path.";
                else if (MathUtil.ContainsFlag((int) result, (int) ArgValidationResult.NotFound))
                    msg = "Please supply a path.";
                
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
        }
        
        public static void FillCommandBuffer()
        {
            FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeDir);
            ProgramUtil.AddPrefixToCommandBuffer("cd ");
        }
    }
}