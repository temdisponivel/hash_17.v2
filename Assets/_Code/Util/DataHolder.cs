using HASH;

namespace HASH
{
    /// <summary>
    /// Class that holds global state.
    /// </summary>
    public static class DataHolder
    {
        public static DebugUtil.DebugCondition DebugCondition;
        public static InputListener InputListener;

        public static TerminalReferences TerminalReferences;
        public static FileSystemData FileSystemData;
        
        public static ProgramsData ProgramData;
    }
}