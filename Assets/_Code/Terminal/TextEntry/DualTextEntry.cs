using System;
using UnityEngine;

namespace HASH17.Terminal.TextEntry
{
    /// <summary>
    /// Class that holds data of a dual (two labels) text entry on the terminal.
    /// </summary>
    public class DualTextEntry : MonoBehaviour
    {
        public const TextEntryType EntryType = TextEntryType.Dual;
        public UILabel LeftTextComponent;
        public UILabel RightTextComponent;
    }
}