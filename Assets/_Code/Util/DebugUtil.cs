using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HASH.Util.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HASH.Util
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
        public static void Log(string value, Color color, DebugCondition condition, LogType type = LogType.Info)
        {
            if (MathUtil.ContainsFlag((int) condition, (int)Global.DebugCondition))
            {
                var msg = TextUtil.ApplyRichTextColor(value, color);

                switch (type)
                {
                    case LogType.Info:
                        Debug.Log(msg);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(msg);
                        break;
                    case LogType.Error:
                        Debug.LogError(msg);
                        break;
                }
            }
        }

        /// <summary>
        /// Logs an error to the console.
        /// </summary>
        [Conditional("DEB")]
        public static void Error(string message)
        {
            Log(message, Color.red, DebugCondition.Verbose, LogType.Error);
        }

        /// <summary>
        /// Logs an error to the console.
        /// </summary>
        [Conditional("DEB")]
        public static void Warning(string message)
        {
            Log(message, Color.yellow, DebugCondition.Always, LogType.Warning);
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
            Info = 1 << 1,
            Verbose = 1 << 2,
            Always = Info | Verbose,
        }

        /// <summary>
        /// Enumerates all possible log types.
        /// </summary>
        public enum LogType
        {
            Info,
            Warning,
            Error,
        }

        #endregion
    }
}