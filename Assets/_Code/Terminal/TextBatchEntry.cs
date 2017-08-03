using HASH17.Terminal.TextEntry;

namespace HASH17.Terminal
{
    /// <summary>
    /// Class that holds information about a batched text.
    /// </summary>
    public struct TextBatchEntry
    {
        public TextEntryType EntryType;
        public string[] Texts;
    }
}