using HASH;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
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
        public static void ShowSingleText(TerminalReferences references, string text)
        {
            var singleTextObject =
                NGUITools.AddChild(references.TextTable.gameObject, references.SingleText.gameObject);
            var singleText = singleTextObject.GetComponent<SingleTextEntry>();
            singleText.TextComponent.text = text;
            singleText.transform.SetAsFirstSibling();

            var widget = singleText.ParentWidget;
            widget.leftAnchor.target = references.ScrollView.transform;
            widget.rightAnchor.target = references.ScrollView.transform;

            singleTextObject.SetActive(true);

            var textEntry = new TerminalTextEntry();
            textEntry.Text = new[] {text};
            textEntry.EntryType = TextEntryType.Single;
            textEntry.SceneObject = singleTextObject;

            SList.Push(DataHolder.TerminalReferences.AllEntries, textEntry);
        }

        /// <summary>
        /// Shows a dual text at the bottom of the terminal.
        /// </summary>
        public static void ShowDualText(TerminalReferences references, string leftText, string rightText)
        {
            var dualTextObject = NGUITools.AddChild(references.TextTable.gameObject, references.DualText.gameObject);
            var dualText = dualTextObject.GetComponent<DualTextEntry>();
            dualText.LeftTextComponent.text = leftText;
            dualText.RightTextComponent.text = rightText;
            dualText.transform.SetAsFirstSibling();

            var widget = dualText.ParentWidget;
            widget.leftAnchor.target = references.ScrollView.transform;
            widget.rightAnchor.target = references.ScrollView.transform;

            dualTextObject.SetActive(true);

            var textEntry = new TerminalTextEntry();
            textEntry.Text = new[] {leftText, rightText};
            textEntry.EntryType = TextEntryType.Single;
            textEntry.SceneObject = dualTextObject;

            SList.Push(DataHolder.TerminalReferences.AllEntries, textEntry);
        }

        #endregion

        #region Input

        /// <summary>
        /// Shows the command from the command buffer accordingly to the index on TerminalData.
        /// </summary>
        public static void ShowCommandFromBuffer(TerminalReferences references)
        {
            var command = references.CurrentCommandBuffer[references.CurrentCommandBufferIndex];
            ShowTextOnInput(references, command);
        }

        /// <summary>
        /// Shows the given text on the input.
        /// </summary>
        public static void ShowTextOnInput(TerminalReferences references, string text)
        {
            references.Input.value = text;
        }

        #endregion

        #region Table / scroll

        /// <summary>
        /// Updates the scroll view position and bounds.
        /// </summary>
        public static void UpdateScroll(TerminalReferences references)
        {
            references.ScrollView.UpdateScrollbars(true);
        }

        /// <summary>
        /// Calls reposition on the table.
        /// </summary>
        public static void UpdateTable(TerminalReferences references)
        {
            references.TextTable.Reposition();
        }

        #endregion
    }
}