using System;
using HASH;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Component to serialize a terminal data.
    /// </summary>
    public class TerminalComponent : MonoBehaviour
    {
        [NonSerialized]
        public float LastTabPressedTime;

        public const float DoubleTabPressedInterval = .3f; 

        public void Initialize()
        {
            DebugUtil.Log("TERMINAL COMPONENT INITIALIZED!", Color.green, DebugUtil.DebugCondition.Info, DebugUtil.LogType.Info);
            
            DataHolder.TerminalData = new TerminalData();
            DataHolder.TerminalData.CommandCache = SList.Create<string>(50);
            DataHolder.TerminalData.AvailableCommands = SList.Create<string>(20);
            DataHolder.TerminalData.BatchEntries = SList.Create<TextBatchEntry>(10);
            DataHolder.TerminalData.AllEntries = SList.Create<TerminalEntry>(100);
            TerminalUtil.FocusOnInput();
            TerminalUtil.CalculateMaxCharLenght();
        }

        #region Callbacks [CALLED BY EDITOR STUFF THROUGH NGUI OR INPUT LISTENER]

        public void OnInputChanged()
        {
            var text = DataHolder.GUIReferences.Input.value;
            
            if (string.IsNullOrEmpty(text))
                TerminalUtil.ChangeToCommandCacheBuffer();
            else if (text.Contains("\n"))
                StartCoroutine(TerminalUtil.HandlePlayerInput(text));
            else
                TerminalUtil.UpdateCommandBuffer();
        }

        public void OnInputSubimit()
        {
            StartCoroutine(TerminalUtil.HandlePlayerInput(DataHolder.GUIReferences.Input.value));
        }

        public void UpPressed()
        {
            TerminalUtil.ChangeToCommandCacheBuffer();
            TerminalUtil.NavigateCommandBuffer(1, false);
        }

        public void DownPressed()
        {
            TerminalUtil.ChangeToCommandCacheBuffer();
            TerminalUtil.NavigateCommandBuffer(-1, false);
        }

        public void EscPressed()
        {
            TerminalUtil.ClearInputText();
        }

        public void TabPressed()
        {
            TerminalUtil.ChangeToAvailableCommandsBuffer();
            
            if (Time.time - LastTabPressedTime <= DoubleTabPressedInterval)
            {
                TerminalUtil.ShowAllCommandBufferOptions();
                LastTabPressedTime = 0;
            }
            else
            {
                LastTabPressedTime = Time.time;
            
                var shiftPressed = Input.GetKey(KeyCode.LeftShift);
                if (shiftPressed)
                    TerminalUtil.NavigateCommandBuffer(-1, true);
                else
                    TerminalUtil.NavigateCommandBuffer(1, true);
            }
        }

        public void F1Pressed()
        {
            TerminalUtil.ToggleTerminal();
        }

        #endregion
    }
}