using System;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Class that holds data of a dual (two labels) text entry on the terminal.
    /// </summary>
    public class DualTextEntry : MonoBehaviour
    {
        public const TerminalEntryType EntryType = TerminalEntryType.DualText;
        public UIWidget ParentWidget;
        public UILabel LeftTextComponent;
        public UILabel RightTextComponent;
    }
}