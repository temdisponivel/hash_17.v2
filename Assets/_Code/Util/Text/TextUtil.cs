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
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);

            var colorEncoded = ColorUtil.EncodeColor(color);
            builder.AppendFormat(NGUIColorStringFormat, colorEncoded, text);
            return builder.ToString();
        }

        /// <summary>
        /// Apply all modifiers to the given text using the NGUI format.
        /// Use TextModifers to create the modifiers parameter.
        /// </summary>
        public static string ApplyNGUIModifiers(string text, int modifiers)
        {
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Bold))
                builder.AppendFormat(NGUIBoldStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Italic))
                builder.AppendFormat(NGUIItalicStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Ignorecolor))
                builder.AppendFormat(NGUIIgnoreColorStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Stroke))
                builder.AppendFormat(NGUIStrokeStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Underline))
                builder.AppendFormat(NGUIUnderlineStringFormat, text);

            return builder.ToString();
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
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);

            var colorEncoded = ColorUtil.EncodeColor(color);
            builder.AppendFormat(RichTextColorStringFormat, colorEncoded, text);
            return builder.ToString();
        }

        public static string ApplyRichTextModifiers(string text, int modifiers)
        {
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Bold))
                builder.AppendFormat(RichTextBoldStringFormat, text);

            if (MathUtil.ContainsFlag(modifiers, TextModifiers.Italic))
                builder.AppendFormat(RichTextItalicStringFormat, text);

            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Ignorecolor), "IGNORE COLOR IS NOT A VALID RICH TEXT MODIFIER!");
            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Stroke), "STROKE IS NOT A VALID RICH TEXT MODIFIER!");
            DebugUtil.Assert(MathUtil.ContainsFlag(modifiers, TextModifiers.Underline), "UNDERLINE IS NOT A VALID RICH TEXT MODIFIER!");

            return builder.ToString();
        }

        #endregion

        #region Input

        /// <summary>
        /// Cleans the inputted text and returns the cleaned string.
        /// </summary>
        public static string CleanInputText(string rawInput)
        {
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);
            builder.Append(rawInput);
            builder.Replace("\n", string.Empty);
            return StripNGUIModifiersAndColor(builder.ToString());
        }

        #endregion

        #region Builder

        /// <summary>
        /// Formats the given string using our internal string builder to prevent garbage generation.
        /// </summary>
        public static string Format(string text, params object[] parameters)
        {
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);
            builder.AppendFormat(text, parameters);
            return builder.ToString();
        }

        /// <summary>
        /// Remove all contents from the given string builder.
        /// </summary>
        public static void ClearBuilder(StringBuilder builder)
        {
            builder.Remove(0, builder.Length);
        }

        #endregion
    }
}