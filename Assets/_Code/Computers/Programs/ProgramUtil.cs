using HASH;
using HASH.Story;
using SimpleCollections.Lists;
using SimpleCollections.Util;
using UnityEditor;

namespace HASH
{
    /// <summary>
    /// Utility class for handling programs.
    /// </summary>
    public static class ProgramUtil
    {
        /// <summary>
        /// Returns a list of all the programs deserialized from the given serialized data.
        /// </summary>
        public static SimpleList<Program> GetAllProgramsFromSerializedData(SerializedProgram[] programs)
        {
            var result = SList.Create<Program>(programs.Length);
            for (int i = 0; i < programs.Length; i++)
            {
                var program = GetProgramFromSerializedData(programs[i]);
                SList.Add(result, program);
            }

            return result;
        }
        
        /// <summary>
        /// Creates and returns a program from the serialized data.
        /// </summary>
        public static Program GetProgramFromSerializedData(SerializedProgram serialized)
        {
            var prog = new Program();
            prog.Name = serialized.Name;
            prog.Description = serialized.Description;
            prog.Commands = serialized.Commands;
            prog.Condition = serialized.Condition;
                
            if (serialized.HelpMessage)
                prog.HelpText = serialized.HelpMessage.text;
            else
                prog.HelpText = string.Empty;
            
            prog.Options = new ProgramOption[serialized.Options.Length];
            for (int i = 0; i < serialized.Options.Length; i++)
                prog.Options[i] = GetExecutionOptionFromSerializedData(serialized.Options[i]);

            prog.ProgramType = serialized.ProgramType;
            return prog;
        }

        /// <summary>
        /// Creates and returns a program option from the given serialized data.
        /// </summary>
        public static ProgramOption GetExecutionOptionFromSerializedData(SerializedProgramOption serialized)
        {
            var opt = new ProgramOption();
            opt.Description = serialized.Description;
            opt.Option = serialized.Option;
            return opt;
        }

        public static void AddPrefixToCommandBuffer(string prefix)
        {
            var data = DataHolder.TerminalData;
            for (int i = 0; i < data.CurrentCommandBuffer.Count; i++)
            {
                var option = data.CurrentCommandBuffer[i];
                data.CurrentCommandBuffer[i] = option.Insert(0, prefix);
            }
        }
        
        public static void SetupPrograms()
        {
            CdProgram.Setup();
            DirProgram.Setup();
            ClearProgram.Setup();
            OpenProgram.Setup();
            CrackerProgram.Setup();
        }

        public static bool ShowHelpIfNeeded(ProgramExecutionOptions options)
        {
            if (options.ParsedArguments.Count > 0)
            {
                if (CommandLineUtil.ArgumentExists(options.ParsedArguments, "h"))
                {
                    HelpProgram.ShowHelpFor(options.ProgramReference);
                    return true;
                }
            }

            return false;
        }

        public static SimpleList<Program> GetAvailablePrograms(HashDevice device)
        {
            var programs = SList.Create<Program>(device.AllPrograms.Count);
            for (int i = 0; i < device.AllPrograms.Count; i++)
            {
                var program = device.AllPrograms[i];
                if (IsProgramAvailable(program))
                    SList.Add(programs, program);
            }

            return programs;
        }

        public static bool IsProgramAvailable(Program program)
        {
            return StoryUtil.EvaluateCondition(program.Condition);
        }
    }
}