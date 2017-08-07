using System;
using HASH.OS.Programs;
using HASH17.Util;
using SimpleCollections.Lists;
using SimpleCollections.Util;

namespace HASH.OS.Shell
{
    /// <summary>
    /// Class that handles the shell of our virtual OS.
    /// </summary>
    public static class Shell
    {
        /// <summary>
        /// Handles a command line.
        /// </summary>
        public static bool IsCommandLineValid(string rawCommandLine)
        {
            var command = CommandLineUtil.GetCommandName(rawCommandLine);
            if (string.IsNullOrEmpty(command))
                return false;
            var program = FindProgramByCommand(command);
            if (program == null)
                return false;
            return true;
        }

        /// <summary>
        /// Finds the program that corresponds to the given commandName.
        /// If none is found, null is returned.
        /// </summary>
        public static Program FindProgramByCommand(string commandName)
        {
            var data = Global.ProgramData;
            var programs = data.AllPrograms;
            for (int i = 0; i < programs.Count; i++)
            {
                var prog = programs[i];
                for (int j = 0; j < prog.Commands.Length; j++)
                {
                    var command = prog.Commands[j];
                    if (string.Equals(command, commandName, StringComparison.InvariantCultureIgnoreCase))
                        return prog;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the program execution options for the given commandLine.
        /// If the commandline is not valid, returns default.
        /// </summary>
        public static ProgramExecutionOptions GetProgramExecutionOptions(string rawCommandLine)
        {
            if (IsCommandLineValid(rawCommandLine))
                return default(ProgramExecutionOptions);

            var command = CommandLineUtil.GetCommandName(rawCommandLine);
            var rawArguments = CommandLineUtil.RemoveCommandFromCommandLine(rawCommandLine);
            var program = FindProgramByCommand(command);

            var options = new ProgramExecutionOptions();

            options.ProgramReference = program;
            options.RawCommandLine = rawCommandLine;
            options.ParsedArguments = CommandLineUtil.GetArgumentsFromCommandLine(rawArguments);

            return options;
        }
    }
}