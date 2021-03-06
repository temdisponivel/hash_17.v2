﻿using System;
using JetBrains.Annotations;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
{
    public static class ClearProgram
    {
        public enum ClearMode
        {
            Top,
            Bottom,
        }

        public const string ModeArgName = "m";
        public const string CountArgName = "c";
        
        public static CommandLineArgValidationOption[] Validations;
        public static CommandLineArgValidationOption ModeValidation;
        public static CommandLineArgValidationOption CountValidation;

        public static void Setup()
        {
            Validations = new CommandLineArgValidationOption[2];

            ModeValidation = new CommandLineArgValidationOption();
            ModeValidation.ArgumentName = ModeArgName;
            ModeValidation.Requirements = ArgRequirement.Unique | ArgRequirement.ValueRequired;

            CountValidation = new CommandLineArgValidationOption();
            CountValidation.ArgumentName = CountArgName;
            CountValidation.Requirements = ArgRequirement.Unique | ArgRequirement.ValueRequired;

            Validations[0] = ModeValidation;
            Validations[1] = CountValidation;
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;
            
            bool everythingOk = true;
            int count = 0;
            ClearMode mode = ClearMode.Top;

            if (options.ParsedArguments.Count == 0)
                count = DataHolder.TerminalData.AllEntries.Count;
            else if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                Pair<string, string> modeArg;
                Pair<string, string> countArg;

                bool modePresent =
                    CommandLineUtil.TryGetArgumentByName(options.ParsedArguments, ModeArgName, out modeArg);
                bool countPresent =
                    CommandLineUtil.TryGetArgumentByName(options.ParsedArguments, CountArgName, out countArg);

                if (modePresent && !countPresent)
                {
                    var msg = "You need to supply the number of line to clear when using the mode argument.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                    everythingOk = false;
                }

                if (countPresent && !modePresent)
                {
                    var msg =
                        "You need to supply the mode argument when removing a specific quantity of lines.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                    everythingOk = false;
                }

                if (everythingOk)
                {
                    if (!MiscUtil.TryParseEnum(modeArg.Value, out mode))
                    {
                        var msg = string.Format("'{0}' is not a valid clear mode. It must be either 'top' or 'down'.",
                            modeArg.Value);
                        msg = TextUtil.Error(msg);
                        TerminalUtil.ShowText(msg);
                        everythingOk = false;
                    }
                    else if (!int.TryParse(countArg.Value, out count))
                    {
                        var msg = string.Format("'{0}' is not a valid number of lines!", countArg.Value);
                        msg = TextUtil.Error(msg);
                        TerminalUtil.ShowText(msg);
                        everythingOk = false;
                    }
                }
            }
            else
            {
                everythingOk = false;
                
                var modeResult = ModeValidation.ValidationResult;
                var countResult = CountValidation.ValidationResult;
                
                if (MathUtil.ContainsFlag((int) modeResult, (int) ArgValidationResult.EmptyValue))
                {
                    var msg = "The 'mode' argument must be either 'top' or 'down'.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                } 
                else if (MathUtil.ContainsFlag((int) modeResult, (int) ArgValidationResult.Duplicated))
                {
                    var msg = "Please supply only one 'mode' argument.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }

            if (everythingOk)
            {
                if (mode == ClearMode.Bottom)
                    TerminalUtil.RemoveTextEntries(count, TerminalEntryRemoveType.NewerEntries);
                else
                    TerminalUtil.RemoveTextEntries(count, TerminalEntryRemoveType.OlderEntries);
            }
        }
    }
}