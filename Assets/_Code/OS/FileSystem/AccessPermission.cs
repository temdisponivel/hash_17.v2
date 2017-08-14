using System;

namespace HASH.OS.FileSystem
{
    /// <summary>
    /// Enumerates all possible access permissions.
    /// </summary>
    [Flags]
    public enum AccessPermission
    {
        None = 1 << 0,
        Read = 1 << 1,
        Write = 1 << 2,
        All = Read | Write,
    }
}