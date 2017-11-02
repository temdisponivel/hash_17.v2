using SimpleCollections.Lists;
using SimpleCollections.Util;

namespace HASH
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