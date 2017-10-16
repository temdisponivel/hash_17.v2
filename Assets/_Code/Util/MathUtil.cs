using System;

namespace HASH
{
    /// <summary>
    /// Utility class for handling math.
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// Returns true if the flags (the thing encoding current flags) constains the flags mask (the specific desired flag).
        /// </summary>
        public static bool ContainsFlag(int flags, int flagMask)
        {
            return (flags & flagMask) == flagMask;
        }

        public static int GetStringHash(string data)
        {
            var result = 0;
            for (int i = 0; i < data.Length; i++)
                result += data[i];
            return result;
        }
    }
}