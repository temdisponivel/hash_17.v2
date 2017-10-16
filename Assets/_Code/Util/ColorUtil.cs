using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Utility class for handling classes.
    /// </summary>
    public static class ColorUtil
    {
        /// <summary>
        /// Returns the encoded color. You can use this result to colorize NGUI strings.
        /// </summary>
        public static string EncodeColor(Color color)
        {
            return NGUIText.EncodeColor(color);
        }
    }
}