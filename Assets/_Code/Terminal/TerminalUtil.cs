using System;
using System.Collections;
using System.Runtime.CompilerServices;
using HASH;
using SimpleCollections.Lists;
using UnityEngine;
using UnityEngine.UI;

namespace HASH
{
    /// <summary>
    /// Class that performs more complex operations on top of the terminal.
    /// This class will also wrapper Terminal's boilerplate code.
    /// </summary>
    public static class TerminalUtil
    {
        #region Batching

        /// <summary>
        /// Starts a text batch.
        /// The text will only be shown on the screen after you call EndTextBatch.
        /// </summary>
        public static void StartTextBatch()
        {
            var data = DataHolder.TerminalData;

            DebugUtil.Assert(data.Batching, "THERE'S ALREADY A TEXT BATCH HAPPENING!");

            data.Batching = true;
        }

        /// <summary>
        /// Ends and executes the current text batch.
        /// </summary>
        public static void EndTextBatch()
        {
            var data = DataHolder.TerminalData;

            DebugUtil.Assert(!data.Batching, "THERE'S NO TEXT BATCH HAPPENING!");

            data.Batching = false;
            while (data.BatchEntries.Count > 0)
            {
                var entry = SList.Dequeue(data.BatchEntries);
                switch (entry.EntryType)
                {
                    case TerminalEntryType.SingleText:
                        ShowText(entry.Texts[0]);
                        break;
                    case TerminalEntryType.DualText:
                        ShowDualText(entry.Texts[0], entry.Texts[1]);
                        break;
                    default:
                        DebugUtil.Log(string.Format("THE TEXT ENTRY TYPE {0} IS NOT IMPLEMENTED!", entry.EntryType),
                            Color.red, DebugUtil.DebugCondition.Always, DebugUtil.LogType.Info);
                        break;
                }
            }

            UpdateTableAndScroll();
        }

        #endregion

        #region Text

        /// <summary>
        /// Shows a single text on the terminal using the default terminal data.
        /// </summary>
        public static void ShowText(string text)
        {
            var references = DataHolder.GUIReferences;
            var data = DataHolder.TerminalData;
            if (data.Batching)
            {
                var entry = new TextBatchEntry();
                entry.EntryType = TerminalEntryType.SingleText;
                entry.Texts = new string[] {text};
                SList.Enqueue(data.BatchEntries, entry);
            }
            else
                Terminal.ShowSingleText(references, text);
        }

        /// <summary>
        /// Shows a dual text on the terminal using the default terminal data.
        /// </summary>
        public static void ShowDualText(string leftText, string rightText)
        {
            var references = DataHolder.GUIReferences;
            var data = DataHolder.TerminalData;
            if (data.Batching)
            {
                var entry = new TextBatchEntry();
                entry.EntryType = TerminalEntryType.DualText;
                entry.Texts = new string[] {leftText, rightText};
                SList.Enqueue(data.BatchEntries, entry);
            }
            else
                Terminal.ShowDualText(references, leftText, rightText);
        }

        /// <summary>
        /// Applies the color to the text and then calls ShowText.
        /// </summary>
        public static void ShowColorizedText(string text, Color color)
        {
            var colorizedText = TextUtil.ApplyNGUIColor(text, color);
            ShowText(colorizedText);
        }

        /// <summary>
        /// Applies the color to the text and then calls ShowText.
        /// </summary>
        public static void ShowColorizedDualText(string leftText, Color leftColor, string rightText, Color rightColor)
        {
            var leftColorizedText = TextUtil.ApplyNGUIColor(leftText, leftColor);
            var rightcolorizedText = TextUtil.ApplyNGUIColor(rightText, rightColor);
            ShowDualText(leftColorizedText, rightcolorizedText);
        }

        /// <summary>
        /// Apply the modifiers and then call ShowText.
        /// Use TextModifiers to create the modifiers.
        /// </summary>
        public static void ShowModifiedText(string text, int modifiers)
        {
            var modifiedText = TextUtil.ApplyNGUIModifiers(text, modifiers);
            ShowText(modifiedText);
        }

        /// <summary>
        /// Apply the modifiers and then call ShowText.
        /// Use TextModifiers to create the modifiers.
        /// </summary>
        public static void ShowModifiedDualText(string leftText, int leftModifers, string rightText, int rightModifiers)
        {
            var leftModifiedText = TextUtil.ApplyNGUIModifiers(leftText, leftModifers);
            var rightModifiedText = TextUtil.ApplyNGUIModifiers(rightText, rightModifiers);
            ShowDualText(leftModifiedText, rightModifiedText);
        }

        /// <summary>
        /// Colorize the text, then modify the text and then call ShowText.
        /// </summary>
        public static void ShowModifiedAndColorizedText(string text, Color color, int modifiers)
        {
            var modifiedText = TextUtil.ApplyNGUIModifiers(text, modifiers);
            var colorizedText = TextUtil.ApplyNGUIColor(modifiedText, color);
            ShowText(colorizedText);
        }

        /// <summary>
        /// Colorize the text, then modify the text and then call ShowText.
        /// </summary>
        public static void ShowModifiedAndColorizedDualText(string leftText, Color leftColor, int leftModifiers,
            string rightText, Color rightColor, int rightModifiers)
        {
            var leftColorizedText = TextUtil.ApplyNGUIColor(leftText, leftColor);
            var leftModifiedText = TextUtil.ApplyNGUIModifiers(leftColorizedText, leftModifiers);

            var rightColorizedText = TextUtil.ApplyNGUIColor(rightText, rightColor);
            var rightModifiedText = TextUtil.ApplyNGUIModifiers(rightColorizedText, rightModifiers);

            ShowDualText(leftModifiedText, rightModifiedText);
        }

        public static void UpdateCurrentPathLabel()
        {
            var line = CreatePlayerInputLine(string.Empty);
            DataHolder.GUIReferences.CurrentPath.text = line.FormattedText;
        }

        #endregion

        #region Image

        public static void ShowImage(Texture texture)
        {
            Terminal.ShowImage(texture, texture.width, texture.height);
        }

        #endregion

        #region Remove Text

        public static void RemoveTextEntries(int quantity, TerminalEntryRemoveType removeType)
        {
            var allEntries = DataHolder.TerminalData.AllEntries;
            quantity = Math.Min(quantity, allEntries.Count);

            if (quantity == 0)
                return;

            switch (removeType)
            {
                case TerminalEntryRemoveType.OlderEntries:
                    for (int i = 0; i < quantity; i++)
                    {
                        var item = SList.Dequeue(allEntries);
                        GameObject.Destroy(item.SceneObject);
                    }
                    break;
                case TerminalEntryRemoveType.NewerEntries:
                    for (int i = 0; i < quantity; i++)
                    {
                        var item = SList.Pop(allEntries);
                        GameObject.Destroy(item.SceneObject);
                    }
                    break;
                default:
                    DebugUtil.Error(string.Format("TERMINAL ENTRY REMOVE TYPE '{0}' NOT IMPLEMENTED!", removeType));
                    break;
            }

            UpdateTableAndScroll();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Clears the input text.
        /// </summary>
        public static void ClearInputText()
        {
            Terminal.ShowTextOnInput(DataHolder.GUIReferences, string.Empty);
        }

        /// <summary>
        /// Change the focus to the input component.
        /// </summary>
        public static void FocusOnInput()
        {
            DataHolder.GUIReferences.Input.selectAllTextOnFocus = false;
        }

        /// <summary>
        /// Calculates and stores the max char lenght on the terminal data.
        /// </summary>
        public static void CalculateMaxCharLenght()
        {
            var references = DataHolder.GUIReferences;
            var font = references.Input.label.bitmapFont;
            var width = font.defaultSize;
            var screenWidth = references.ScrollView.bounds.size.x;

            var data = DataHolder.TerminalData;
            data.MaxLineWidthInChars = Mathf.FloorToInt(screenWidth / width);
        }

        /// <summary>
        /// Handles a player input.
        /// </summary>
        public static IEnumerator HandlePlayerInput(string rawInput)
        {
            var text = TextUtil.CleanInputText(rawInput);
            if (string.IsNullOrEmpty(text))
                yield break;

            DebugUtil.Log("[Terminal Util] PLAYER INPUT: " + text, Color.green, DebugUtil.DebugCondition.Verbose,
                DebugUtil.LogType.Info);

            var line = CreatePlayerInputLine(text);

            ShowText(line.FormattedText);

            if (!Shell.RunCommandLine(text))
            {
                var msg = string.Format("'{0}' is not a valid command. If you need help, input 'help'", text);
                msg = TextUtil.Error(msg);
                ShowText(msg);
            }
            
            UpdateTableAndScroll();
            ClearInputText();
            FocusOnInput();
            CacheCommand(DataHolder.TerminalData, text);
            ResetCommandBufferIndex();                
        }

        /// <summary>
        /// Caches the command on the terminal data.
        /// This will enable the user to up/down typed commands.
        /// </summary>
        public static void CacheCommand(TerminalData data, string command)
        {
            SList.Insert(data.CommandCache, command, 0);
        }

        /// <summary>
        /// Navigates through the command buffer.
        /// Positive direction means up, negative directions means down.
        /// If wrapsAround is true, this will go from end to start and start to end,
        /// if not, it'll clamp to the buffer size.
        /// </summary>
        public static void NavigateCommandBuffer(int direction, bool wrapsAround)
        {
            var references = DataHolder.GUIReferences;
            var data = DataHolder.TerminalData;
            var buffer = data.CurrentCommandBuffer;
            if (buffer.Count > 0)
            {
                var index = data.CurrentCommandBufferIndex + direction;
                if (wrapsAround)
                {
                    // Since index cannot ever be smaller than -1 (because we will defined it as position right down here)
                    // we can just define the index as the last element of the buffer
                    if (index < 0)
                        index = buffer.Count - 1;

                    index = index % buffer.Count;
                }
                else
                    index = Mathf.Clamp(index, 0, buffer.Count - 1);

                data.CurrentCommandBufferIndex = index;
                Terminal.ShowCommandFromBuffer(data, references);
            }
        }

        /// <summary>
        /// Resets the command cache index to make the cache navigation starts from the bottom.
        /// </summary>
        public static void ResetCommandBufferIndex()
        {
            var data = DataHolder.TerminalData;

            // Set to -1 because when the player presses the key again, we need this index to go to 0
            // if we set to 0, it will go either to 1 (if pressed up) or zero
            // setting to -1 makes it go to 0 regardless of up or down
            data.CurrentCommandBufferIndex = -1;
        }

        /// <summary>
        /// Fills the available command buffer with the current available options.
        /// </summary>
        public static void UpdateCommandBuffer()
        {
            var references = DataHolder.GUIReferences;
            var data = DataHolder.TerminalData;
            SList.Clear(data.AvailableCommands);

            var currentText = references.Input.value;

            var programName = CommandLineUtil.GetCommandName(currentText);
            var program = Shell.FindProgramByCommand(programName);

            Action bufferFiller;
            if (program == null)
                bufferFiller = null;
            else
                bufferFiller = Shell.GetProgramBufferFillerMethod(program);

            if (bufferFiller == null)
                FileSystem.FilleCommandBufferWithFileSystem(FillBufferFileSystemOptions.IncludeAll);
            else
                bufferFiller();

            ChangeToAvailableCommandsBuffer();
        }

        public static void ShowAllCommandBufferOptions()
        {
            StartTextBatch();

            var data = DataHolder.TerminalData;

            int index = data.CurrentCommandBufferIndex;

            ResetCommandBufferIndex();
            for (int i = 0; i < data.CurrentCommandBuffer.Count; i++)
            {
                var option = data.CurrentCommandBuffer[i];
                ShowText(option);
            }

            data.CurrentCommandBufferIndex = index;

            EndTextBatch();
        }

        /// <summary>
        /// Checks to see if the current buffer is equals to the commands cache buffer and, if not
        /// changes the current buffer to the commands cache buffer and resets the buffer index.
        /// </summary>
        public static void ChangeToCommandCacheBuffer()
        {
            var data = DataHolder.TerminalData;
            data.CurrentCommandBuffer = data.CommandCache;
        }

        /// <summary>
        /// Checks to see if the current buffer is equals to the available commands buffer and, if not
        /// changes the current buffer to the available commands buffer and resets the buffer index.
        /// Returns true if it changed the current buffer, false otherwise.
        /// </summary>
        public static void ChangeToAvailableCommandsBuffer()
        {
            var data = DataHolder.TerminalData;
            data.CurrentCommandBuffer = data.AvailableCommands;
        }

        #endregion

        #region Scroll/Table

        /// <summary>
        /// This will update the table and then the scroll - in order.
        /// </summary>
        public static void UpdateTableAndScroll()
        {
            var data = DataHolder.GUIReferences;
            Terminal.UpdateTable(data);
            Terminal.UpdateScroll(data);
        }

        #endregion

        #region Tabling

        private static TextTableLine CreatePlayerInputLine(string command)
        {
            var path = DataHolder.FileSystemData.CurrentDir.FullPath;
            var pathColumn = CreatePlayerInputColumn(path, .65f, Constants.Colors.Success, TextModifiers.Italic);
            var commandColumn = CreatePlayerInputColumn(command, .35f, Constants.Colors.Default, TextModifiers.None);

            var items = SList.Create<TextTableColumn>(2);
            SList.Add(items, pathColumn);
            SList.Add(items, commandColumn);

            var line = new TextTableLine();
            line.Items = items;


            line.MaxLineSizeIsForced = false;
            line.MaxLineSize = DataHolder.TerminalData.MaxLineWidthInChars;

            line.ItemsSeparator = "> ";
            line.SeparatorModifier.Color = Constants.Colors.Success;
            line.SeparatorModifier.Modifiers = TextModifiers.None;

            TextUtil.FormatLineConsideringWeightsAndSize(line);

            return line;
        }

        private static TextTableColumn CreatePlayerInputColumn(
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
            item.WrapMode = WrapTextMode.AddDots;
            item.ModifyTextOptions.Modifiers = textModifiers;
            item.Size = text.Length;

            return item;
        }

        #endregion
    }
}