using System.ComponentModel;
using HASH.OS.Shell;
using HASH.Terminal;
using HASH.Util;
using HASH.Util.Text;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.OS.Programs.Implementation
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

        public static readonly CommandLineArgValidationOption<Args>[] ValidationOptions;
        public static readonly string[] KnownArgs;
        public static readonly TextTableLine DirContentHeader;

        static CdProgram()
        {
            ValidationOptions = new CommandLineArgValidationOption<Args>[1];
            var pathOpt = new CommandLineArgValidationOption<Args>();

            pathOpt.AditionalData = Args.Path;
            pathOpt.ArgumentName = string.Empty;
            pathOpt.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;

            ValidationOptions[0] = pathOpt;

            KnownArgs = new[] {"",};

            DirContentHeader = new TextTableLine();
            var items = SList.Create<TextTableItem>(3);

            DirContentHeader.ItemsSeparator = " | ";
            DirContentHeader.SeparatorModifier.Color = Color.cyan;
            DirContentHeader.MaxLineSize = Global.TerminalReferences.MaxLineWidthInChars;
            DirContentHeader.Items = items;
            DirContentHeader.MaxLineSize = 100;
            DirContentHeader.MaxLineSizeIsForced = true;

            var typeItem = new TextTableItem();
            typeItem.Text = "TYPE";
            typeItem.Size = typeItem.Text.Length;
            typeItem.WrapMode = WrapTextMode.Clamp;
            typeItem.Align = TextTableAlign.Left;
            typeItem.ModifyTextOptions.Color = Color.blue;
            typeItem.WeightOnLine = .1f;
            typeItem.PaddingChar = ' ';
            SList.Add(items, typeItem);

            typeItem = new TextTableItem();
            typeItem.Text = "PATH";
            typeItem.Size = typeItem.Text.Length;
            typeItem.WrapMode = WrapTextMode.Clamp;
            typeItem.Align = TextTableAlign.Left;
            typeItem.ModifyTextOptions.Color = Color.green;
            typeItem.WeightOnLine = .5f;
            typeItem.PaddingChar = ' ';
            SList.Add(items, typeItem);

            typeItem = new TextTableItem();
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

        private static int i = 0;

        /// <summary>
        /// Executes the CD program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions options)
        {
            bool argumentsOk, knownArgs;
            var result = CommandLineUtil.FullArgValidation(options.ParsedArguments, ValidationOptions, KnownArgs,
                out knownArgs, out argumentsOk);

            UnityEngine.Debug.Log("ALO");

            if (i == 3)
                TerminalUtil.EndTextBatch();
            else if (i == 0)
                TerminalUtil.StartTextBatch();

            TerminalUtil.ShowText(DirContentHeader.FormattedText);
            i++;
        }
    }
}