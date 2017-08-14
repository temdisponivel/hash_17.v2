using System;
using UnityEngine;

namespace HASH.Terminal.TextEntry
{
    /// <summary>
    /// Class that holds data of a single text entity on the terminal.
    /// </summary>
    public class SingleTextEntry : MonoBehaviour
    {
        public const TextEntryType EntryType = TextEntryType.Single;
        public UIWidget ParentWidget;
        public UILabel TextComponent;
    }
}