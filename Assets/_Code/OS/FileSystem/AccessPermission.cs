using System;

namespace HASH.OS.FileSystem
{
    /// <summary>
    /// Enumerates all possible access permissions.
    /// </summary>
    [Flags]
    public enum AccessPermission
    {
        Read = 1 << 0,
        Write = 1 << 1,
        All = Read | Write,
    }
}