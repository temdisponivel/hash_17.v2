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
        public static CommandLineArgValidationOption[] ArgValidationOptions;

        public static void Setup()
        {
            HeaderLine = CreateLine("NAME", "TYPE", "STATUS", HeaderColor, TextModifiers.Bold | TextModifiers.Underline);

            var pathArgValidation = new CommandLineArgValidationOption();

            pathArgValidation.ArgumentName = string.Empty;
            pathArgValidation.Requirements = ArgRequirement.ValueRequired;

            ArgValidationOptions = new[] {pathArgValidation};
        }

        /// <summary>
        /// Executes the dir program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;
            
            HashDir currentDir = null;

            if (options.ParsedArguments.Count == 0)
                currentDir = DataHolder.FileSystemData.CurrentDir;
            else
            {
                var desiredDirPath = options.ParsedArguments[0].Value;
                if (!FileSystem.DirExists(desiredDirPath, out currentDir))
                {
                    string msg;

                    HashFile file;
                    if (FileSystem.FileExistsAndIsAvailable(desiredDirPath, out file))
                        msg = string.Format("The path '{0}' points to a file. Use 'open {0}' to open this file.",
                            desiredDirPath);
                    else
                        msg = string.Format("The path '{0}' points nowhere. Please supply a valid path.",
                            desiredDirPath);

                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                    return;
                }
            }

            var childs = currentDir.Childs;
            var files = FileSystem.GetAvailableFilesFromDir(currentDir);

            if (childs.Count == 0 && files.Count == 0)
                TerminalUtil.ShowColorizedText("EMPTY DIRECTORY!", LineColor);
            else
            {
                TerminalUtil.StartTextBatch();

                TerminalUtil.ShowText(HeaderLine.FormattedText);

                for (int i = 0; i < childs.Count; i++)
                {
                    var child = childs[i];
                    var line = CreateLine(child.Name, "DIRECTORY", string.Empty, LineColor, TextModifiers.Italic);
                    TerminalUtil.ShowText(line.FormattedText);
                }
                
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];

                    var status = FileSystem.GetStatusString(file.Status);
                    var line = CreateLine(file.FullName, "FILE", status, LineColor, TextModifiers.Italic);
                    TerminalUtil.ShowText(line.FormattedText);
                }

                TerminalUtil.EndTextBatch();
            }
        }

        public static void FillCommandBuffer()
        {
            FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeDir);
            ProgramUtil.AddPrefixToCommandBuffer("dir ");
        }

        public static TextTableLine CreateLine(
            string nameColumnText,
            string typeColumnText,
            string statusColumnText,
            Color color,
            int textModifiers)
        {
            var nameColumnItem = CreateColumn(nameColumnText, .6f, color, textModifiers);
            var typeColumnItem = CreateColumn(typeColumnText, .2f, color, textModifiers);
            var statusColumnItem = CreateColumn(statusColumnText, .2f, color, textModifiers);

            var items = SList.Create<TextTableColumn>(2);
            SList.Add(items, nameColumnItem);
            SList.Add(items, typeColumnItem);
            SList.Add(items, statusColumnItem);

            var line = new TextTableLine();
            line.Items = items;
            line.ItemsSeparator = string.Empty;
            line.MaxLineSize = DataHolder.TerminalData.MaxLineWidthInChars;
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