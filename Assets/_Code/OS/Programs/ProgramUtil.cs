using HASH;

namespace HASH
{
    /// <summary>
    /// Utility class for handling programs.
    /// </summary>
    public static class ProgramUtil
    {
        /// <summary>
        /// Creates and returns a program from the serialized data.
        /// </summary>
        public static Program GetProgramFromSerializedData(SerializedProgram serialized)
        {
            var prog = new Program();
            prog.Name = serialized.Name;
            prog.Description = serialized.Description;
            prog.Commands = serialized.Commands;

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
    }
}