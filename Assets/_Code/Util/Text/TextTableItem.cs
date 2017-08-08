using SimpleCollections.Lists;

namespace HASH17.Util.Text
{
    /// <summary>
    /// Represents a item on a text line.
    /// </summary>
    public struct TextTableItem
    {
        public string Text;
        public int Size;
        public char PaddingChar;
        public TextTableAlign Align;
        public WrapTextMode WrapMode;

        public float WeightOnLine;
    }

    /// <summary>
    /// Represents a line on a text table.
    /// </summary>
    public struct TextTableLine
    {
        public SimpleList<TextTableItem> Items;
        public string ItemsSeparator;

        public int MaxLineSize;

        public bool AddSeparatorOnStart;
        public bool AddSeparatorOnEnd;
    }

    /// <summary>
    /// Enumerates all possible wrapping modes for a text table line.
    /// </summary>
    public enum WrapTextMode
    {
        Clamp,
        AddDots,
    }

    /// <summary>
    /// Enumerates possible alignments for a text table.
    /// </summary>
    public enum TextTableAlign
    {
        Left,
        Center,
        Right
    }
}