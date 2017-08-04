using System;
using HASH17.Terminal.TextEntry;
using HASH17.Util;
using HASH17.Util.Text;
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
            var colorizedText = TextUtil.ApplyNGUIColor(text, color);
            var modifiedText = TextUtil.ApplyNGUIModifiers(colorizedText, modifiers);
            ShowText(modifiedText);
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