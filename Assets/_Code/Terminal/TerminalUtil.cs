﻿using System;
using HASH17.Terminal.TextEntry;
using HASH17.Util;
using HASH17.Util.Text;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH17.Terminal
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
            var data = Global.TerminalData;
#if DEB
            DebugUtil.Assert(data.Batching, "THERE'S ALREADY A TEXT BATCH HAPPENING!");
#endif
            data.Batching = true;
        }

        /// <summary>
        /// Ends and executes the current text batch.
        /// </summary>
        public static void EndTextBatch()
        {
            var data = Global.TerminalData;
#if DEB
            DebugUtil.Assert(!data.Batching, "THERE'S NO TEXT BATCH HAPPENING!");
#endif
            data.Batching = false;
            while (data.BatchEntries.Count > 0)
            {
                var entry = data.BatchEntries.Pop();
                switch (entry.EntryType)
                {
                    case TextEntryType.Single:
                        ShowText(entry.Texts[0]);
                        break;
                    case TextEntryType.Dual:
                        ShowDualText(entry.Texts[0], entry.Texts[1]);
                        break;
#if DEB
                    default:
                        DebugUtil.Log(string.Format("THE TEXT ENTRY TYPE {0} IS NOT IMPLEMENTED!", entry.EntryType), Color.red, DebugUtil.DebugCondition.Always);
                        break;
#endif
                }
            }
        }

        #endregion

        #region Text

        /// <summary>
        /// Shows a single text on the terminal using the default terminal data.
        /// </summary>
        public static void ShowText(string text)
        {
            var data = Global.TerminalData;
            if (data.Batching)
            {
                var entry = new TextBatchEntry();
                entry.EntryType = TextEntryType.Single;
                entry.Texts = new string[] { text };
                data.BatchEntries.Push(entry);
            }
            else
                Terminal.ShowSingleText(data, text);
        }

        /// <summary>
        /// Shows a dual text on the terminal using the default terminal data.
        /// </summary>
        public static void ShowDualText(string leftText, string rightText)
        {
            var data = Global.TerminalData;
            if (data.Batching)
            {
                var entry = new TextBatchEntry();
                entry.EntryType = TextEntryType.Dual;
                entry.Texts = new string[] { leftText, rightText };
                data.BatchEntries.Push(entry);
            }
            else
            {
                Terminal.ShowDualText(data, leftText, rightText);
                Terminal.UpdateScroll(data);

            }
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

        #endregion

        #region Commands

        /// <summary>
        /// Clears the input text.
        /// </summary>
        public static void ClearInputText()
        {
            Terminal.ShowTextOnInput(Global.TerminalData, string.Empty);
        }

        /// <summary>
        /// Change the focus to the input component.
        /// </summary>
        public static void FocusOnInput()
        {
            Global.TerminalData.Input.selectAllTextOnFocus = false;
        }

        /// <summary>
        /// Handles a player input.
        /// </summary>
        public static void HandlePlayerInput(string rawInput)
        {
            var text = TextUtil.CleanInputText(rawInput);

#if DEB
            DebugUtil.Log("[Terminal Util] PLAYER INPUT: " + text, Color.green, DebugUtil.DebugCondition.Verbose);
#endif

            ShowText(rawInput);

            UpdateTableAndScroll();
            ClearInputText();
            FocusOnInput();

            CacheCommand(Global.TerminalData, text);
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
            var data = Global.TerminalData;
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
                Terminal.ShowCommandFromBuffer(data);
            }
        }

        /// <summary>
        /// Resets the command cache index to make the cache navigation starts from the bottom.
        /// </summary>
        public static void ResetCommandBufferIndex()
        {
            var data = Global.TerminalData;

            // Set to -1 because when the player presses the key again, we need this index to go to 0
            // if we set to 0, it will go either to 1 (if pressed up) or zero
            // setting to -1 makes it go to 0 regardless of up or down
            data.CurrentCommandBufferIndex = -1;
        }

        /// <summary>
        /// Fills the available command buffer with the current available options.
        /// </summary>
        public static void FillAvailableCommandBuffer()
        {
            // TODO: real stuff

            var data = Global.TerminalData;
            SList.Clear(data.AvailableCommands);

            var currentText = data.Input.value;
            SList.Add(data.AvailableCommands, string.Format("{0}{1}", currentText, "DOIDOMEMO 1"));
            SList.Add(data.AvailableCommands, string.Format("{0}{1}", currentText, "DOIDOMEMO 2"));
            SList.Add(data.AvailableCommands, string.Format("{0}{1}", currentText, "DOIDOMEMO 3"));
            SList.Add(data.AvailableCommands, string.Format("{0}{1}", currentText, "DOIDOMEMO 4"));
            SList.Add(data.AvailableCommands, string.Format("{0}{1}", currentText, "DOIDOMEMO 5"));
        }

        /// <summary>
        /// Checks to see if the current buffer is equals to the commands cache buffer and, if not
        /// changes the current buffer to the commands cache buffer and resets the buffer index.
        /// </summary>
        public static void ChangeToCommandCacheBufferIfNeeded()
        {
            var data = Global.TerminalData;
            if (data.CurrentCommandBuffer != data.CommandCache)
            {
                data.CurrentCommandBuffer = data.CommandCache;
                ResetCommandBufferIndex();
            }
        }

        /// <summary>
        /// Checks to see if the current buffer is equals to the available commands buffer and, if not
        /// changes the current buffer to the available commands buffer and resets the buffer index.
        /// Returns true if it changed the current buffer, false otherwise.
        /// </summary>
        public static bool ChangeToAvailableCommandsBufferIfNeeded()
        {
            var data = Global.TerminalData;
            if (data.CurrentCommandBuffer != data.AvailableCommands)
            {
                data.CurrentCommandBuffer = data.AvailableCommands;
                ResetCommandBufferIndex();

                return true;
            }
            else
                return false;
        }

        #endregion

        #region Scroll/Table

        /// <summary>
        /// This will update the table and then the scroll - in order.
        /// </summary>
        public static void UpdateTableAndScroll()
        {
            var data = Global.TerminalData;
            Terminal.UpdateTable(data);
            Terminal.UpdateScroll(data);
        }

        #endregion
    }
}