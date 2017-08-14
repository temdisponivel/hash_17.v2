using HASH.OS.FileSystem;
using HASH.OS.Shell;
using HASH.Terminal;
using HASH.Util.Input;
using HASH.Util.Text;

namespace HASH.Util
{
    /// <summary>
    /// Class that holds global state.
    /// </summary>
    public static class Global
    {
        public static DebugUtil.DebugCondition DebugCondition;
        public static InputListener InputListener;

        public static TerminalReferences TerminalReferences;
        public static FileSystemData FileSystemData;
        
        public static ProgramsData ProgramData;
    }
}