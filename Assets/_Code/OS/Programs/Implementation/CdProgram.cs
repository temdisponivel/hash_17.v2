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
        /// <summary>
        /// Enumerates the arguments of the CD program.
        /// </summary>
        public enum Args
        {
            Path,
        }

        public static CommandLineArgValidationOption<Args>[] ValidationOptions;
        public static string[] KnownArgs;
        public static TextTableLine DirContentHeader;

        /// <summary>
        /// Executes the CD program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions options)
        {
            bool argumentsOk, knownArgs;
            CommandLineUtil.FullArgValidation(options.ParsedArguments, ValidationOptions, KnownArgs, out knownArgs, out argumentsOk);
            ValidateErrorsWithArgs(ValidationOptions);

            Pair<string, string> command;
            CommandLineUtil.TryGetArgumentByName(options.ParsedArguments, "", out command);
            var path = command.Value;
            var dir = FileSystem.FindDirByPath(path);
            FileSystem.ChangeDir(dir);
        }

        private static void ValidateErrorsWithArgs(CommandLineArgValidationOption<Args>[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var result = (int)arg.ValidationResult;
                var empty = (int)ArgValidationResult.EmptyValue;
                if (MathUtil.ContainsFlag(result, empty))
                {
                    Debug.Log("EMPTY");
                }
            }
        }
        
        public static void Setup()
        {
            ValidationOptions = new CommandLineArgValidationOption<Args>[1];
            var pathOpt = new CommandLineArgValidationOption<Args>();

            pathOpt.AditionalData = Args.Path;
            pathOpt.ArgumentName = string.Empty;
            pathOpt.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;

            ValidationOptions[0] = pathOpt;

            KnownArgs = new[] { "", };

            DirContentHeader = new TextTableLine();
            var items = SList.Create<TextTableColumn>(3);

            DirContentHeader.ItemsSeparator = " | ";
            DirContentHeader.SeparatorModifier.Color = Color.cyan;
            DirContentHeader.MaxLineSize = Global.TerminalReferences.MaxLineWidthInChars;
            DirContentHeader.Items = items;
            DirContentHeader.MaxLineSize = 100;
            DirContentHeader.MaxLineSizeIsForced = true;

            var typeItem = new TextTableColumn();
            typeItem.Text = "TYPE";
            typeItem.Size = typeItem.Text.Length;
            typeItem.WrapMode = WrapTextMode.Clamp;
            typeItem.Align = TextTableAlign.Left;
            typeItem.ModifyTextOptions.Color = Color.blue;
            typeItem.WeightOnLine = .1f;
            typeItem.PaddingChar = ' ';
            SList.Add(items, typeItem);

            typeItem = new TextTableColumn();
            typeItem.Text = "PATH";
            typeItem.Size = typeItem.Text.Length;
            typeItem.WrapMode = WrapTextMode.Clamp;
            typeItem.Align = TextTableAlign.Left;
            typeItem.ModifyTextOptions.Color = Color.green;
            typeItem.WeightOnLine = .5f;
            typeItem.PaddingChar = ' ';
            SList.Add(items, typeItem);

            typeItem = new TextTableColumn();
            typeItem.Text = "STATUS";
            typeItem.Size = typeItem.Text.Length;
            typeItem.WrapMode = WrapTextMode.Clamp;
            typeItem.Align = TextTableAlign.Left;
            typeItem.ModifyTextOptions.Color = Color.magenta;
            typeItem.WeightOnLine = .4f;
            typeItem.PaddingChar = ' ';
            SList.Add(items, typeItem);

            TextUtil.FormatLineConsideringWeightsAndSize(DirContentHeader);
        }
    }
}