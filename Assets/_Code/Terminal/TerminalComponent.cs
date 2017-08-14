using HASH.Game;
using HASH.Util;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.Terminal
{
    /// <summary>
    /// Component to serialize a terminal data.
    /// </summary>
    public class TerminalComponent : MonoBehaviour
    {
        public TerminalReferences References;

        public void Initialize()
        {
            DebugUtil.Log("TERMINAL COMPONENT INITIALIZED!", Color.green, DebugUtil.DebugCondition.Info);
            References.CommandCache = SList.Create<string>(50);
            References.AvailableCommands = SList.Create<string>(20);
            References.BatchEntries = SList.Create<TextBatchEntry>(10);
            Global.TerminalReferences = References;
            TerminalUtil.FocusOnInput();
            TerminalUtil.CalculateMaxCharLenght();
        }

        #region Callbacks [CALLED BY EDITOR STUFF THROUGH NGUI OR INPUT LISTENER]

        public void OnInputChanged()
        {
            var text = References.Input.value;

            if (text.EndsWith("\n"))
                StartCoroutine(TerminalUtil.HandlePlayerInput(text));
        }

        public void OnInputSubimit()
        {
            StartCoroutine(TerminalUtil.HandlePlayerInput(References.Input.value));
        }

        public void UpPressed()
        {
            TerminalUtil.ChangeToCommandCacheBufferIfNeeded();
            TerminalUtil.NavigateCommandBuffer(1, false);
        }

        public void DownPressed()
        {
            TerminalUtil.ChangeToCommandCacheBufferIfNeeded();
            TerminalUtil.NavigateCommandBuffer(-1, false);
        }

        public void EscPressed()
        {
            TerminalUtil.ClearInputText();
            TerminalUtil.ResetCommandBufferIndex();
        }

        public void TabPressed()
        {
            // If we changed to the available command buffer, 
            // we need to make sure it has the right options, so we fill the buffer
            if (TerminalUtil.ChangeToAvailableCommandsBufferIfNeeded())
                TerminalUtil.FillAvailableCommandBuffer();
            
            var shiftPressed = Input.GetKey(KeyCode.LeftShift);
            if (shiftPressed)
                TerminalUtil.NavigateCommandBuffer(-1, true);
            else
                TerminalUtil.NavigateCommandBuffer(1, true);
        }

        #endregion
    }
}