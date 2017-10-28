using HASH;
using HASH.GUI;
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
        public static void ShowSingleText(GUIReferences data, string text)
        {
            var singleTextObject =
                NGUITools.AddChild(data.TextTable.gameObject, data.SingleText.gameObject);
            var singleText = singleTextObject.GetComponent<SingleTextEntry>();
            singleText.TextComponent.text = text;
            singleText.transform.SetAsFirstSibling();

            var widget = singleText.ParentWidget;
            widget.leftAnchor.target = data.ScrollView.transform;
            widget.rightAnchor.target = data.ScrollView.transform;

            singleTextObject.SetActive(true);

            var textEntry = new TerminalEntry();
            textEntry.EntryType = TerminalEntryType.SingleText;
            textEntry.SceneObject = singleTextObject;

            SList.Push(DataHolder.TerminalData.AllEntries, textEntry);
        }

        /// <summary>
        /// Shows a dual text at the bottom of the terminal.
        /// </summary>
        public static void ShowDualText(GUIReferences data, string leftText, string rightText)
        {
            var dualTextObject = NGUITools.AddChild(data.TextTable.gameObject, data.DualText.gameObject);
            var dualText = dualTextObject.GetComponent<DualTextEntry>();
            dualText.LeftTextComponent.text = leftText;
            dualText.RightTextComponent.text = rightText;
            dualText.transform.SetAsFirstSibling();

            var widget = dualText.ParentWidget;
            widget.leftAnchor.target = data.ScrollView.transform;
            widget.rightAnchor.target = data.ScrollView.transform;

            dualTextObject.SetActive(true);

            var textEntry = new TerminalEntry();
            textEntry.EntryType = TerminalEntryType.SingleText;
            textEntry.SceneObject = dualTextObject;

            SList.Push(DataHolder.TerminalData.AllEntries, textEntry);
        }

        #endregion

        #region Input

        /// <summary>
        /// Shows the command from the command buffer accordingly to the index on TerminalData.
        /// </summary>
        public static void ShowCommandFromBuffer(TerminalData data, GUIReferences references)
        {
            var command = data.CurrentCommandBuffer[data.CurrentCommandBufferIndex];
            ShowTextOnInput(references, command);
        }

        /// <summary>
        /// Shows the given text on the input.
        /// </summary>
        public static void ShowTextOnInput(GUIReferences data, string text)
        {
            data.Input.value = text;
        }

        #endregion

        #region Table / scroll

        /// <summary>
        /// Updates the scroll view position and bounds.
        /// </summary>
        public static void UpdateScroll(GUIReferences data)
        {
            data.ScrollView.ResetPosition();
            data.ScrollView.UpdatePosition();
            data.ScrollView.UpdateScrollbars(true);
        }

        /// <summary>
        /// Calls reposition on the table.
        /// </summary>
        public static void UpdateTable(GUIReferences data)
        {
            data.TextTable.Reposition();
        }

        #endregion

        #region Image

        public static void ShowImage(Texture texture, int width, int height)
        {
            var data = DataHolder.GUIReferences;

            var imageObject = NGUITools.AddChild(data.TextTable.gameObject, data.Image.gameObject);
            var image = imageObject.GetComponent<ImageEntry>();
            image.Image.mainTexture = texture;
            image.Image.aspectRatio = width / (float) height;
            image.Image.width = width;
            image.Image.height = height;
            
            imageObject.transform.SetAsFirstSibling();
            imageObject.SetActive(true);

            var entry = new TerminalEntry();
            entry.EntryType = TerminalEntryType.Image;
            entry.SceneObject = imageObject;
            
            SList.Push(DataHolder.TerminalData.AllEntries, entry);
        }

        #endregion
    }
}