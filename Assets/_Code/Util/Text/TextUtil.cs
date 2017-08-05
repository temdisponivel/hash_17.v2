using System.Text;
using UnityEngine;

namespace HASH17.Util.Text
{
    /// <summary>
    /// Utility class for handling text.
    /// </summary>
    public static class TextUtil
    {
        #region Const

        #region NGUI

        public const string NGUIColorStringFormat = "[{0}]{1}[-]";
        public const string NGUIBoldStringFormat = "[b]{0}[/b]";
        public const string NGUIItalicStringFormat = "[i]{0}[/i]";
        public const string NGUIStrokeStringFormat = "[s]{0}[/s]";
        public const string NGUIUnderlineStringFormat = "[u]{0}[/u]";
        public const string NGUIIgnoreColorStringFormat = "[c]{0}[-]";

        #endregion

        #region Rich Text

        public const string RichTextColorStringFormat = "<color=#{0}>{1}</color>";
        public const string RichTextBoldStringFormat = "<b>{0}</b>";
        public const string RichTextItalicStringFormat = "<i>{0}</i>";

        #endregion

        #endregion

        #region NGUI-related

        /// <summary>
        /// Returns the colorized string using the NGUI format..
        /// </summary>
        public static string ApplyNGUIColor(string text, Color color)
        {
            var colorEncoded = ColorUtil.EncodeColor(color);
            return string.Format(NGUIColorStringFormat, colorEncoded, text);
        }

        /// <summary>
        /// Apply all modifiers to the given text using the NGUI format.
        /// Use TextModifers to create the modifiers parameter.
        /// </summary>
        public static string ApplyNGUIModifiers(string text, int modifiers)
        {
            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Bold))
                text = string.Format(NGUIBoldStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Italic))
                text = string.Format(NGUIItalicStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Ignorecolor))
                text = string.Format(NGUIIgnoreColorStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Stroke))
                text = string.Format(NGUIStrokeStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Underline))
                text = string.Format(NGUIUnderlineStringFormat, text);

            return text;
        }

        /// <summary>
        /// Returns the given string with all code removed - including color or any other modifier.
        /// </summary>
        public static string StripNGUIModifiersAndColor(string text)
        {
            return NGUIText.StripSymbols(text);
        }

        #endregion

        #region Rich Text

        /// <summary>
        /// Returns the colorized color using the Rich text format.
        /// </summary>
        public static string ApplyRichTextColor(string text, Color color)
        {
            var colorEncoded = ColorUtil.EncodeColor(color);
            return string.Format(RichTextColorStringFormat, colorEncoded, text);
        }

        public static string ApplyRichTextModifiers(string text, int modifiers)
        {
            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Bold))
                text = string.Format(RichTextBoldStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Italic))
                text = string.Format(RichTextItalicStringFormat, text);

#if DEB
            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Ignorecolor), "IGNORE COLOR IS NOT A VALID RICH TEXT MODIFIER!");
            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Stroke), "STROKE IS NOT A VALID RICH TEXT MODIFIER!");
            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Underline), "UNDERLINE IS NOT A VALID RICH TEXT MODIFIER!");
#endif

            return text;
        }

        #endregion

        #region Input

        /// <summary>
        /// Cleans the inputted text and returns the cleaned string.
        /// </summary>
        public static string CleanInputText(string rawInput)
        {
            rawInput = rawInput.Replace("\n", string.Empty);
            rawInput = StripNGUIModifiersAndColor(rawInput);
            return rawInput;
        }

        #endregion
    }
}