using System;

namespace HASH17.Util.Text
{
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