using HASH;

namespace HASH
{
    /// <summary>
    /// Class that holds information about a batched text.
    /// </summary>
    public struct TextBatchEntry
    {
        public TerminalEntryType EntryType;
        public string[] Texts;
    }
}