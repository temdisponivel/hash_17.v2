using SimpleCollections.Lists;

namespace HASH.OS.Programs
{
    /// <summary>
    /// Represents a program on our virtual OS.
    /// </summary>
    public class Program
    {
        public string Name;
        public string Description;

        public string[] Commands;
        public ProgramOption[] Options;

        public ProgramType ProgramType;
    }
}