using System;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Class that holds data of a single text entity on the terminal.
    /// </summary>
    public class SingleTextEntry : MonoBehaviour
    {
        public const TerminalEntryType EntryType = TerminalEntryType.SingleText;
        public UIWidget ParentWidget;
        public UILabel TextComponent;
    }
}