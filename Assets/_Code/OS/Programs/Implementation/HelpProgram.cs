using System;
using System.Text;

namespace HASH
{
    public static class HelpProgram
    {
        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;
            
            if (options.ParsedArguments.Count > 0)
            {
                var programName = options.ParsedArguments[0].Value;
                var program = Shell.FindProgramByCommand(programName);
                if (program == null)
                {
                    var msg =
                        "'{0}' is not a valid program. You can use 'help' to see help for all programs or 'help <program_name>' to see help about a specific program";
                    msg = TextUtil.ApplyNGUIColor(msg, Constants.Colors.Error);
                    TerminalUtil.ShowText(msg);
                }
                else
                {
                    ShowHelpFor(program);
                    ShowProgramsUsage();
                }
            }
            else
            {
                TerminalUtil.StartTextBatch();

                var programs = DataHolder.ProgramData.AllPrograms;
                for (int i = 0; i < programs.Count; i++)
                {
                    var program = programs[i];
                    ShowHelpFor(program);
                }

                ShowProgramsUsage();

                TerminalUtil.EndTextBatch();
            }
        }

        public static void ShowHelpFor(Program program)
        {
            string helpMessage;
            if (string.IsNullOrEmpty(program.HelpText))
                helpMessage = BuildDynamicHelpMessage(program);
            else
                helpMessage = program.HelpText;

            TerminalUtil.ShowText(helpMessage);
        }

        public static string BuildDynamicHelpMessage(Program program)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("{0}", program.Name));

            if (program.Commands.Length > 0)
            {
                builder.AppendLine("    Commands:");
                for (int i = 0; i < program.Commands.Length; i++)
                {
                    builder.AppendLine(string.Format("        {0}", program.Commands[i]));
                }                
            }

            if (program.Options.Length > 0)
            {
                builder.AppendLine();
                builder.AppendLine("    Options:");
                for (int i = 0; i < program.Options.Length; i++)
                {
                    var opt = program.Options[i];
                    var option = opt.Option;
                    if (string.IsNullOrEmpty(option))
                        option = "<empty>";
                    builder.AppendLine(string.Format("        {0} -> {1}", option, opt.Description));
                }                
            }

            return builder.ToString();
        }

        public static void ShowProgramsUsage()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Program usage:");

            var usage = "    <program_comand> [<program_option> <program_option> <program_option>]";
            builder.AppendLine(usage);

            builder.AppendLine();
            builder.AppendLine("    Examples:");

            var example = "        help open";
            builder.AppendLine(example);
            
            example = "        open path/to/file -t";
            builder.AppendLine(example);
            
            example = "        cracker path/to/file -p password";
            builder.AppendLine(example);
            
            TerminalUtil.ShowText(builder.ToString());
        }
    }
}