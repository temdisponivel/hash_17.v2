using HASH17.Util;
using HASH17.Terminal.TextEntry;
using UnityEngine;

namespace HASH17.Terminal
{
    /// <summary>
    /// Class that handles the terminal behaviour.
    /// </summary>
    public static class Terminal
    {
        #region Text

        /// <summary>
        /// Shows a single text at the bottom of the terminal.
        /// </summary>
        public static void ShowSingleText(TerminalData data, string text)
        {
            var singleTextObject = NGUITools.AddChild(data.TextTable.gameObject, data.SingleText.gameObject);
            var singleText = singleTextObject.GetComponent<SingleTextEntry>();
            singleText.TextComponent.text = text;
        }

        /// <summary>
        /// Shows a dual text at the bottom of the terminal.
        /// </summary>
        public static void ShowDualText(TerminalData data, string leftText, string rightText)
        {
            var singleTextObject = NGUITools.AddChild(data.TextTable.gameObject, data.DualText.gameObject);
            var singleText = singleTextObject.GetComponent<DualTextEntry>();
            singleText.LeftTextComponent.text = leftText;
            singleText.RightTextComponent.text = rightText;
        }

#endregion

#region Table / scroll

        /// <summary>
        /// Updates the scroll view position and bounds.
        /// </summary>
        public static void UpdateScroll(TerminalData data)
        {
            data.ScrollView.UpdateScrollbars(true);
        }

        /// <summary>
        /// Calls reposition on the table.
        /// </summary>
        public static void UpdateTable(TerminalData data)
        {
            data.TextTable.Reposition();
        }

#endregion
    }
}