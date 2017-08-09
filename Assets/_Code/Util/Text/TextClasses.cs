using SimpleCollections.Lists;
using UnityEngine;

namespace HASH17.Util.Text
{
    /// <summary>
    /// Represents a line on a text table.
    /// </summary>
    public class TextTableLine
    {
        public SimpleList<TextTableItem> Items;

        public string ItemsSeparator;
        public ModifyTextOptions SeparatorModifier;

        public int MaxLineSize;

        public bool AddSeparatorOnStart;
        public bool AddSeparatorOnEnd;

        public bool MaxLineSizeIsForced;

        public string FormattedText;
    }

    /// <summary>
    /// Represents a item on a text line.
    /// </summary>
    public class TextTableItem
    {
        public string Text;
        public string ModifiedText;
        public ModifyTextOptions ModifyTextOptions;
        public int Size;
        public char PaddingChar;
        public TextTableAlign Align;
        public WrapTextMode WrapMode;

        public float WeightOnLine;
    }

    /// <summary>
    /// Wrappers formatting options of a text.
    /// </summary>
    public struct ModifyTextOptions
    {
        public bool UseRichText;
        public int Modifiers;
        public Color? Color;
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

    /// <summary>
    /// Enumerates all possible text modifiers.
    /// </summary>
    public static class TextModifiers
    {
        public const int Bold = 1 << 0;
        public const int Italic = 1 << 1;
        public const int Stroke = 1 << 2;
        public const int Underline = 1 << 3;
        public const int Ignorecolor = 1 << 4;
    }
}