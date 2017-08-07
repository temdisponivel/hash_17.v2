using HASH.OS.FileSystem;
using HASH.OS.Shell;
using HASH17.Terminal;
using HASH17.Util.Input;
using HASH17.Util.Text;

namespace HASH17.Util
{
    /// <summary>
    /// Class that holds global state.
    /// </summary>
    public static class Global
    {
        public static DebugUtil.DebugCondition DebugCondition;
        public static InputListener InputListener;

        public static TerminalData TerminalData;
        public static FileSystemData FileSystemData;

        public static TextUtilData TextUtilData;
        public static ProgramsData ProgramData;
    }
}