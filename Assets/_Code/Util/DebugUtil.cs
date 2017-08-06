using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HASH17.Util.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HASH17.Util
{
    /// <summary>
    /// Utility class for debugging.
    /// </summary>
    public static class DebugUtil
    {
        /// <summary>
        /// If the given value is true, a error will be thrown and the game will break.
        /// </summary>
        [Conditional("DEB")]
        public static void Assert(bool value, string message)
        {
            if (value)
            {
                Debug.LogError(message);
                Debug.Break();
            }
        }

        /// <summary>
        /// Same as Assert but this will add the context objeto to the console.
        /// </summary>
        [Conditional("DEB")]
        public static void AssertContext(bool value, string message, UnityEngine.Object context)
        {
            if (value)
            {
                Debug.LogError(message, context);
                Debug.Break();
            }
        }

        /// <summary>
        /// Logs the given message with the given color.
        /// </summary>
        [Conditional("DEB")]
        public static void Log(string value, Color color, DebugCondition condition)
        {
            if (MathUtil.ContainsFlag((int) Global.DebugCondition, (int) condition))
            {
                var msg = TextUtil.ApplyRichTextColor(value, color);
                Debug.Log(msg);
            }
        }

        /// <summary>
        /// Same as Log but this will add the context to the log.
        /// </summary>
        [Conditional("DEB")]
        public static void LogContext(string value, Color color, UnityEngine.Object context, DebugCondition condition)
        {
            if (MathUtil.ContainsFlag((int) Global.DebugCondition, (int) condition))
            {
                var msg = TextUtil.ApplyRichTextColor(value, color);
                Debug.Log(msg, context);
            }
        }

        #region Inner Types

        /// <summary>
        /// Enumerates all possible debug conditions.
        /// </summary>
        [Flags]
        public enum DebugCondition
        {
            Info = 1 << 0,
            Verbose = 1 << 1,
            Always = Info | Verbose,
        }

        #endregion
    }
}