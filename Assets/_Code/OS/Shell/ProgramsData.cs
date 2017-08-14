using HASH.OS.Programs;
using SimpleCollections.Hash;
using SimpleCollections.Lists;

namespace HASH.OS.Shell
{
    /// <summary>
    /// Holds all the program-related data.
    /// </summary>
    public class ProgramsData
    {
        public SimpleList<Program> AllPrograms;

        public SimpleSet<string> ArgNameHelper;
    }
}