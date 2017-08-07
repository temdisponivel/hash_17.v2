using HASH17.Util;
using HASH17.Util.Text;
using SimpleCollections.Lists;
using SimpleCollections.Util;

namespace HASH.OS.Shell
{
    /// <summary>
    /// Class that performs operations related to the command line (like parsing arguments, etc).
    /// </summary>
    public static class CommandLineUtil
    {
        #region Properties

        public const char ArgumentPrefix = '-';

        #endregion

        /// <summary>
        /// Returns the name of the command on the given commandLine.
        /// The command name is the text from the start of the command line all the way to the first special character.
        /// If command line is empty or null, empty is returned.
        /// </summary>
        public static string GetCommandName(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                return string.Empty;

            var builder = Global.TextUtilData.BuilderHelper;
            TextUtil.ClearBuilder(builder);
            for (int i = 0; i < commandLine.Length; i++)
            {
                var letter = commandLine[i];

                if (char.IsLetter(letter))
                    builder.Append(letter);
                else
                    break;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Removes the command from the command line and returns the result.
        /// If command line is null or empty, return empty.
        /// </summary>
        public static string RemoveCommandFromCommandLine(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                return string.Empty;

            var commandName = GetCommandName(commandLine);
            if (commandName.Length == commandLine.Length)
                return string.Empty;
            else
                return commandLine.Replace(commandName, string.Empty).Trim();
        }

        /// <summary>
        /// Parses the given command line and returns the arguments form is.
        /// The command line is expected to be without the command name.
        /// </summary>
        public static SimpleList<Pair<string, string>> GetArgumentsFromCommandLine(string commandLineWithoutCommand)
        {
            commandLineWithoutCommand = commandLineWithoutCommand.Trim();

            var result = SList.Create<Pair<string, string>>(3);

            var builder = Global.TextUtilData.BuilderHelper;
            TextUtil.ClearBuilder(builder);

            var argName = string.Empty;
            var argValue = string.Empty;

            const int lookForArgState = 0;
            const int readArgNameState = 1;
            const int readArgValueState = 2;

            int currentState = lookForArgState;

            // If it don't start with the argument prefix, it means that the parameter has no name, just value
            if (!commandLineWithoutCommand.StartsWith(ArgumentPrefix.ToString()))
                currentState = readArgValueState;

            var onQuote = false;
            var ignoreNextSpecial = false;
            for (int i = 0; i < commandLineWithoutCommand.Length; i++)
            {
                var letter = commandLineWithoutCommand[i];

                // Specials
                var isEmpty = letter == ' ' && !ignoreNextSpecial;
                var isQuote = letter == '"' && !ignoreNextSpecial;
                var isBackSlash = letter == '\\' && !ignoreNextSpecial;
                var isArgPrefix = letter == ArgumentPrefix && !ignoreNextSpecial;

                ignoreNextSpecial = false;

                if (isQuote)
                {
                    onQuote = !onQuote;
                    continue;
                }
                else if (isBackSlash)
                {
                    ignoreNextSpecial = true;
                    continue;
                }

                if (onQuote)
                    ignoreNextSpecial = true;

                switch (currentState)
                {
                    case lookForArgState:
                        if (isArgPrefix)
                            currentState = readArgNameState;
                        break;
                    case readArgNameState:
                        // if there's a space after the start of a arg, ignore the space
                        if (isEmpty && builder.Length == 0)
                            continue;

                        if (isEmpty)
                        {
                            argName = builder.ToString();
                            TextUtil.ClearBuilder(builder);
                            currentState = readArgValueState;

                            argValue = string.Empty;
                        }
                        else
                            builder.Append(letter);
                        break;
                    case readArgValueState:
                        if (isArgPrefix)
                        {
                            argValue = builder.ToString();
                            TextUtil.ClearBuilder(builder);

                            var pair = CreateArgPair(argName, argValue);
                            SList.Add(result, pair);

                            argName = string.Empty;
                            argValue = string.Empty;

                            currentState = readArgNameState;
                        }
                        else
                            builder.Append(letter);
                        break;
                }
            }

            // Process what might have been left on the command line
            switch (currentState)
            {
                case lookForArgState:
                    argName = string.Empty;
                    argValue = string.Empty;
                    break;
                case readArgNameState:
                    argName = builder.ToString();
                    break;
                case readArgValueState:
                    argValue = builder.ToString();
                    break;
            }

            // We can parameters with empty names but values or with empty values but names
            if (!(string.IsNullOrEmpty(argName) && string.IsNullOrEmpty(argValue)))
            {
                var finalPair = CreateArgPair(argName, argValue);
                SList.Add(result, finalPair);
            }

            TextUtil.ClearBuilder(builder);

            return result;
        }

        /// <summary>
        /// Shorthand for create an new pair with the given name and value.
        /// </summary>
        public static Pair<string, string> CreateArgPair(string argName, string argValue)
        {
            var pair = new Pair<string, string>();
            pair.Key = argName;
            pair.Value = argValue;
            return pair;
        }
    }
}