using SimpleCollections.Lists;
using SimpleCollections.Util;

namespace HASH.OS.Programs
{
    /// <summary>
    /// Holds options passed to program executions.
    /// </summary>
    public struct ProgramExecutionOptions
    {
        public Program ProgramReference;
        public string RawCommandLine;
        public SimpleList<Pair<string, string>> ParsedArguments;
    }
}