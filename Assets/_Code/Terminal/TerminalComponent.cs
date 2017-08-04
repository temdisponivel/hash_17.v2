using HASH17.Util;
using HASH17.Util.Text;
using UnityEngine;

namespace HASH17.Terminal
{
    /// <summary>
    /// Component to serialize a terminal data.
    /// </summary>
    public class TerminalComponent : MonoBehaviour
    {
        public TerminalData Data;

        private void Awake()
        {
            Global.TerminalData = Data;
        }

        public void OnInputChanged()
        {
            var text = Data.Input.value;

            if (text.EndsWith("\n"))
                HandlePlayerInput(text);
        }

        public void OnInputSubimit()
        {
            HandlePlayerInput(Data.Input.value);
        }

        private void HandlePlayerInput(string rawInput)
        {
            var text = TextUtil.CleanInputText(rawInput);

#if DEB
            DebugUtil.Log("[Terminal Component] PLAYER INPUT: " + text, Color.green, DebugUtil.DebugCondition.Verbose);
#endif

            TerminalUtil.ShowText(rawInput);
        }
    }
}