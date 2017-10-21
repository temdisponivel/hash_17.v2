using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using HASH;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Executes the dir program.
    /// Dir lists the contents of a directory.
    /// </summary>
    public static class DirProgram
    {
        public static Color HeaderColor = Color.green;
        public static Color LineColor = Color.blue;

        public static TextTableLine HeaderLine;
        public static CommandLineArgValidationOption<string>[] ArgValidationOptions;

        /// <summary>
        /// Executes the dir program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions option)
        {
            HashDir currentDir;

            if (option.ParsedArguments.Count == 0)
                currentDir = Global.FileSystemData.CurrentDir;
            else
            {
                DebugUtil.Assert(!CommandLineUtil.ValidateArguments(option.ParsedArguments, ArgValidationOptions),
                    "INVALID PATH");
                var desiredDirPath = option.ParsedArguments[0].Value;
                if (!FileSystem.DirExists(desiredDirPath, out currentDir))
                {
                    var msg = string.Format("No directory found at path: '{0}'", desiredDirPath);
                    TerminalUtil.ShowColorizedText(msg, Color.red);
                    return;
                }
            }

            var childs = currentDir.Childs;
            var files = currentDir.Files;

            if (childs.Count == 0 && files.Count == 0)
                TerminalUtil.ShowColorizedText("EMPTY DIRECTORY!", LineColor);
            else
            {
                TerminalUtil.StartTextBatch();

                TerminalUtil.ShowText(HeaderLine.FormattedText);

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var line = CreateLine(file.Name, "FILE", LineColor, TextModifiers.Italic);
                    TerminalUtil.ShowText(line.FormattedText);
                }

                for (int i = 0; i < childs.Count; i++)
                {
                    var child = childs[i];
                    var line = CreateLine(child.Name, "DIRECTORY", LineColor, TextModifiers.Italic);
                    TerminalUtil.ShowText(line.FormattedText);
                }

                TerminalUtil.EndTextBatch();
            }
        }

        public static void FillCommandBuffer()
        {
            FileSystem.FillCommandBufferWithAvailableDirectories();
            
            var data = Global.TerminalReferences;
            for (int i = 0; i < data.CurrentCommandBuffer.Count; i++)
            {
                var option = data.CurrentCommandBuffer[i];
                data.CurrentCommandBuffer[i] = option.Insert(0, "dir ");
            }
        }

        public static void Setup()
        {
            HeaderLine = CreateLine("NAME", "TYPE", HeaderColor, TextModifiers.Bold | TextModifiers.Underline);

            var pathArgValidation = new CommandLineArgValidationOption<string>();

            pathArgValidation.ArgumentName = "Path";
            pathArgValidation.Requirements = ArgRequirement.ValueRequired;

            ArgValidationOptions = new[] {pathArgValidation};
        }

        public static TextTableLine CreateLine(
            string nameColumnText,
            string typeColumnText,
            Color color,
            int textModifiers)
        {
            var nameColumnItem = CreateColumn(nameColumnText, .6f, color, textModifiers);
            var typeColumnItem = CreateColumn(typeColumnText, .4f, color, textModifiers);

            var items = SList.Create<TextTableColumn>(2);
            SList.Add(items, nameColumnItem);
            SList.Add(items, typeColumnItem);

            var line = new TextTableLine();
            line.Items = items;
            line.ItemsSeparator = string.Empty;
            line.MaxLineSize = Global.TerminalReferences.MaxLineWidthInChars;
            line.MaxLineSizeIsForced = true;

            line.ItemsSeparator = " | ";
            line.SeparatorModifier.Color = color;
            line.SeparatorModifier.Modifiers = TextModifiers.Bold;

            TextUtil.FormatLineConsideringWeightsAndSize(line);

            return line;
        }

        public static TextTableColumn CreateColumn(
            string text,
            float weight,
            Color color,
            int textModifiers)
        {
            var item = new TextTableColumn();

            item.Align = TextTableAlign.Left;
            item.Text = text;
            item.ModifyTextOptions.Color = color;
            item.PaddingChar = ' ';
            item.WeightOnLine = weight;
            item.WrapMode = WrapTextMode.Clamp;
            item.ModifyTextOptions.Modifiers = textModifiers;

            return item;
        }
    }
}