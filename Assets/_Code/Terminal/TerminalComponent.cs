using HASH.Game;
using HASH17.Util;
using HASH17.Util.Text;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH17.Terminal
{
    /// <summary>
    /// Component to serialize a terminal data.
    /// </summary>
    public class TerminalComponent : MonoBehaviour, IInitializable
    {
        public TerminalData Data;

        void Awake()
        {
            Initialize();
        }

        #region Callbacks [CALLED BY EDITOR STUFF THROUGH NGUI OR INPUT LISTENER]

        public void OnInputChanged()
        {
            var text = Data.Input.value;

            if (text.EndsWith("\n"))
                TerminalUtil.HandlePlayerInput(text);
        }

        public void OnInputSubimit()
        {
            TerminalUtil.HandlePlayerInput(Data.Input.value);
        }
        
        public void UpPressed()
        {
            TerminalUtil.NavigateCommandCache(1);
        }

        public void DownPressed()
        {
            TerminalUtil.NavigateCommandCache(-1);
        }

        public void EscPressed()
        {
            TerminalUtil.ClearInputText();
            TerminalUtil.ResetCacheIndex();
        }

        #endregion

        public int GetOrder()
        {
            return 0;
        }

        public void Initialize()
        {
            DebugUtil.Log("TERMINAL COMPONENT INITIALIZED!", Color.green, DebugUtil.DebugCondition.Info);
            Data.CommandCache = SList.Create<string>(50);
            Global.TerminalData = Data;
            TerminalUtil.FocusOnInput();
        }
    }
}