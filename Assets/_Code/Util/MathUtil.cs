using System;

namespace HASH17.Util
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
    }
}