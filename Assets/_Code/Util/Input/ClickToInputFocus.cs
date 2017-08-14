using UnityEngine;

namespace HASH.Util.Input
{
    /// <summary>
    /// Receives a click input and focus on the terminal.
    /// </summary>
    public class ClickToInputFocus : MonoBehaviour
    {
        // Called by NGUI's UI Root
        private void OnClick()
        {
            Global.TerminalReferences.Input.isSelected = true;
        }
    }
}