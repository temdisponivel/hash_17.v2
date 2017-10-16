using HASH;

namespace HASH
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