using System;
using System.Text;
using SimpleCollections.Lists;
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

        #region Tables

        /// <summary>
        /// Formats a table item accordingly to the given option.
        /// </summary>
        public static string FormatTableItem(TextTableItem item)
        {
            const int AddDotWrapCount = 3;

            string text = item.Text;

            var symbolLength = GetNGUISymbolsSize(text);
            item.Size += symbolLength; // compensate for possible color, bold and mofifiers

            if (text.Length > item.Size)
            {
                switch (item.WrapMode)
                {
                    case WrapTextMode.Clamp:
                        text = text.Substring(0, item.Size);
                        break;
                    case WrapTextMode.AddDots:
                        DebugUtil.Assert(text.Length <= 3, "TO USE ADD POINTS WRAP THE TEXT MUST BE AT LEAST 3 CHARS LONG");
                        text = text.Substring(0, item.Size - AddDotWrapCount);
                        text = text.PadRight(item.Size, '.');
                        break;
                }
            }

            var diff = item.Size - item.Text.Length;
            if (diff > 0)
            {
                switch (item.Align)
                {
                    case TextTableAlign.Left:
                        text = text.PadRight(item.Size, item.PaddingChar);
                        break;
                    case TextTableAlign.Center:
                        if (diff % 2 != 0)
                        {
                            diff++;
                            item.Size++; // need to add one here to compensate for the one padding we are adding
                        }
                        diff /= 2;
                        text = text.PadLeft(item.Size - diff, item.PaddingChar);
                        text = text.PadRight(item.Size, item.PaddingChar);
                        break;
                    case TextTableAlign.Right:
                        text = text.PadLeft(item.Size, item.PaddingChar);
                        break;
                }
            }

            return text;
        }

        /// <summary>
        /// Formats a table line accordingly to the given options.
        /// </summary>
        public static string FormatTableLine(TextTableLine line)
        {
            var builder = Global.TextUtilData.BuilderHelper;
            ClearBuilder(builder);
            var collumns = line.Items;
            string text;
            for (int i = 0; i < collumns.Count; i++)
            {
                var collumn = collumns[i];
                text = FormatTableItem(collumn);

                bool addSeparator = true;
                if (string.IsNullOrEmpty(line.ItemsSeparator))
                    addSeparator = false;
                else if (i == 0 && !line.AddSeparatorOnStart)
                    addSeparator = false;

                if (addSeparator)
                    text = string.Format("{0}{1}", line.ItemsSeparator, text);

                builder.Append(text);
            }

            if (line.AddSeparatorOnEnd)
                builder.Append(line.ItemsSeparator);

            return builder.ToString();
        }

        /// <summary>
        /// Shorthand for calling FormatTableLine and returning the length of the result.
        /// </summary>
        public static int CalculateTableLineLength(TextTableLine line, bool removeSymbolsLength)
        {
            var text = FormatTableLine(line);
            var symbolsLength = 0;
            if (removeSymbolsLength)
                symbolsLength = GetNGUISymbolsSize(text);
            return text.Length - symbolsLength;
        }

        /// <summary>
        /// Shorthand for calling FormatTableItem and retuning the length of the result.
        /// </summary>
        public static int CalculateTableItemLength(TextTableItem item, bool removeSymbolsLength)
        {
            var text = FormatTableItem(item);

            var symbolsLength = 0;
            if (removeSymbolsLength)
                symbolsLength = GetNGUISymbolsSize(text);

            return text.Length - symbolsLength;
        }

        /// <summary>
        /// Calculates and stores the sizes of the items of the given line accordingly to the maxLineSize and items weight.
        /// </summary>
        public static TextTableLine GetIdealTableItemSize(TextTableLine line)
        {
            int maxLineSize = line.MaxLineSize;

            int separatorLength = 0;
            if (!string.IsNullOrEmpty(line.ItemsSeparator))
                separatorLength = line.ItemsSeparator.Length;

            separatorLength -= GetNGUISymbolsSize(line.ItemsSeparator);

            maxLineSize -= separatorLength * (line.Items.Count - 1);

            if (line.AddSeparatorOnStart)
                maxLineSize -= separatorLength;
            if (line.AddSeparatorOnEnd)
                maxLineSize -= separatorLength;
            
            DebugUtil.Assert(maxLineSize <= 0, "THE MAX LINE SIZE IS NOT BIG ENOUGH TO FIT ANY OF THE ITEMS");

            for (int i = 0; i < line.Items.Count; i++)
            {
                var item = line.Items[i];
                var weight = item.WeightOnLine;
                item.Size = Mathf.RoundToInt(maxLineSize * weight);
                
                line.Items[i] = item;
            }

            return line;
        }

        /// <summary>
        /// Shorthand for calling GetIdealTableItemSize and FormatTableLine.
        /// </summary>
        public static string FormatConsideringWeightsAndSize(TextTableLine line)
        {
            line = GetIdealTableItemSize(line);
            return FormatTableLine(line);
        }

        /// <summary>
        /// Returns the total size of NGUI symbols on the string.
        /// Use this in order to compensate for stirng lenghts.
        /// </summary>
        public static int GetNGUISymbolsSize(string text)
        {
            var stripped = StripNGUIModifiersAndColor(text).Length;
            return text.Length - stripped;
        }

        #endregion
    }
}