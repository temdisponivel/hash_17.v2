using HASH17.Util;
using HASH17.Terminal.TextEntry;
using SimpleCollections.Lists;
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
            singleText.transform.SetAsFirstSibling();

            var widget = singleText.ParentWidget;
            widget.leftAnchor.target = data.ScrollView.transform;
            widget.rightAnchor.target = data.ScrollView.transform;
        }

        /// <summary>
        /// Shows a dual text at the bottom of the terminal.
        /// </summary>
        public static void ShowDualText(TerminalData data, string leftText, string rightText)
        {
            var singleTextObject = NGUITools.AddChild(data.TextTable.gameObject, data.DualText.gameObject);
            var dualText = singleTextObject.GetComponent<DualTextEntry>();
            dualText.LeftTextComponent.text = leftText;
            dualText.RightTextComponent.text = rightText;
            dualText.transform.SetAsFirstSibling();

            var widget = dualText.ParentWidget;
            widget.leftAnchor.target = data.ScrollView.transform;
            widget.rightAnchor.target = data.ScrollView.transform;
        }

        #endregion

        #region Input

        /// <summary>
        /// Shows the command from the command cache accordingly to the index on TerminalData.
        /// </summary>
        public static void ShowCommandFromCache(TerminalData data)
        {
            var command = data.CommandCache[data.CurrentCommandCacheIndex];
            ShowTextOnInput(data, command);
        }

        /// <summary>
        /// Shows the given text on the input.
        /// </summary>
        public static void ShowTextOnInput(TerminalData data, string text)
        {
            data.Input.value = text;
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