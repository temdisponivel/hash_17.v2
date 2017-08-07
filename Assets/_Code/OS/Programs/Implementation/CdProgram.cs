using HASH.OS.Shell;

namespace HASH.OS.Programs.Implementation
{
    /// <summary>
    /// Executes the CD program.
    /// CD stands for change directory and that's what this program does.
    /// </summary>
    public static class CdProgram
    {
        /// <summary>
        /// Enumerates the arguments of the CD program.
        /// </summary>
        public enum Args
        {
            Path,
        }

        public static readonly CommandLineArgValidationOption<Args>[] ValidationOptions;
        public static readonly string[] KnownArgs;

        static CdProgram()
        {
            ValidationOptions = new CommandLineArgValidationOption<Args>[1];
            var pathOpt = new CommandLineArgValidationOption<Args>();

            pathOpt.AditionalData = Args.Path;
            pathOpt.ArgumentName = string.Empty;
            pathOpt.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;

            ValidationOptions[0] = pathOpt;

            KnownArgs = new[] {"",};
        }

        /// <summary>
        /// Executes the CD program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions options)
        {
            bool argumentsOk, knownArgs;
            var result = CommandLineUtil.FullArgValidation(options.ParsedArguments, ValidationOptions, KnownArgs,
                out knownArgs, out argumentsOk);

            if (!result)
            {
                if (!argumentsOk)
                {
                    UnityEngine.Debug.Log("ARGUMENTS NOT OK");
                }

                if (!knownArgs)
                {
                    var unknown = CommandLineUtil.GetUnknownArguments(options.ParsedArguments, KnownArgs);
                    for (int i = 0; i < unknown.Count; i++)
                    {
                        UnityEngine.Debug.Log("UNKNOWN: " + unknown[i].Key);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("OK");
            }
        }
    }
}