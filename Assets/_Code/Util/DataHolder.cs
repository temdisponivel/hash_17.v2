using HASH;
using HASH.GUI;
using HASH.Story;
using HASH.Window;

namespace HASH
{
    /// <summary>
    /// Class that holds global state.
    /// </summary>
    public static class DataHolder
    {
        public static DebugUtil.DebugCondition DebugCondition;
        
        public static TerminalData TerminalData;
        public static FileSystemData FileSystemData;
        
        public static ProgramsData ProgramData;
        public static GUIReferences GUIReferences;
    }
}