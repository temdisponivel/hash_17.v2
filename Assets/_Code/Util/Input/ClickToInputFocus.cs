using UnityEngine;

namespace HASH17.Util.Input
{
    /// <summary>
    /// Receives a click input and focus on the terminal.
    /// </summary>
    public class ClickToInputFocus : MonoBehaviour
    {
        // Called by NGUI's UI Root
        private void OnClick()
        {
            Global.TerminalData.Input.isSelected = true;
        }
    }
}