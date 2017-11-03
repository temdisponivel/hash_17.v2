using System;
using System.Diagnostics;
using HASH;
using JetBrains.Annotations;

namespace HASH
{
    /// <summary>
    /// Class that handles the shell of our virtual OS.
    /// </summary>
    public static class Shell
    {
        /// <summary>
        /// Interprets, find the program and execute the command of the given command line.
        /// </summary>
        public static bool RunCommandLine(string commandLine)
        {
            var programOpt = GetProgramExecutionOptions(commandLine);
            if (programOpt.ProgramReference == null)
                return false;
            else
            {
                var entryPoint = GetProgramExecutionEntryPoint(programOpt.ProgramReference);
                entryPoint(programOpt);
                return true;
            }
        }

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
            var programs = DataHolder.DeviceData.CurrentDevice.AllPrograms;
            for (int i = 0; i < programs.Count; i++)
            {
                var prog = programs[i];
                for (int j = 0; j < prog.Commands.Length; j++)
                {
                    var command = prog.Commands[j];
                    if (string.Equals(command, commandName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (ProgramUtil.IsProgramAvailable(prog))
                            return prog;
                    }
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
            if (!IsCommandLineValid(rawCommandLine))
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

        /// <summary>
        /// Returns the method used to execute the given program.
        /// </summary>
        public static Action<ProgramExecutionOptions> GetProgramExecutionEntryPoint(Program program)
        {
            switch (program.ProgramType)
            {
                case ProgramType.Cd:
                    return CdProgram.Execute;
                case ProgramType.Dir:
                    return DirProgram.Execute;
                case ProgramType.Clear:
                    return ClearProgram.Execute;
                case ProgramType.Open:
                    return OpenProgram.Execute;
                case ProgramType.Cracker:
                    return CrackerProgram.Execute;
                case ProgramType.Help:
                    return HelpProgram.Execute;
                case ProgramType.SSH:
                    return SSHProgram.Execute;
                case ProgramType.GSMWatcher:
                    return GSMWatcherProgram.Execute;
                case ProgramType.Map:
                    return MapProgram.Execute;
                default:
                    DebugUtil.Assert(true, "PROGRAM TYPE HAS NO RELATED CLASS. " + program.ProgramType);
                    break;
            }

            return null;
        }

        public static Action GetProgramBufferFillerMethod(Program program)
        {
            switch (program.ProgramType)
            {
                case ProgramType.Cd:
                    return CdProgram.FillCommandBuffer;
                case ProgramType.Dir:
                    return DirProgram.FillCommandBuffer;
                case ProgramType.Open:
                    return OpenProgram.FillCommandBuffer;
                case ProgramType.Cracker:
                    return CrackerProgram.FillCommandBuffer;
                default:
                    DebugUtil.Warning(string.Format("Program '{0}' doesn't have a fill buffer function.", program.ProgramType));
                    break;
            }

            return null;
        }
    }
}