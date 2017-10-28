using HASH;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Receives a click input and focus on the terminal.
    /// </summary>
    public class ClickToInputFocus : MonoBehaviour
    {
        // Called by NGUI's UI Root
        private void OnClick()
        {
            DataHolder.GUIReferences.Input.isSelected = true;
        }
    }
}