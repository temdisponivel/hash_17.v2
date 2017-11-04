using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
{
    public static class CrackerProgram
    {
        public const float PercentualThresholdToSuccess = .75f;
        public const int TextPerBatch = 10;

        public const string HintArgName = "a";
        public const string DeviceArgName = "d";
        public const string UsernameArgName = "u";
        public const string FileArgName = "f";

        public static CommandLineArgValidationOption[] Validations;
        public static CommandLineArgValidationOption HintValidation;
        public static CommandLineArgValidationOption DeviceValidation;
        public static CommandLineArgValidationOption UsernameValidation;
        public static CommandLineArgValidationOption FileValidation;

        public static void Setup()
        {
            HintValidation = new CommandLineArgValidationOption();
            HintValidation.ArgumentName = HintArgName;
            HintValidation.Requirements = ArgRequirement.Required | ArgRequirement.ValueRequired;

            DeviceValidation = new CommandLineArgValidationOption();
            DeviceValidation.ArgumentName = DeviceArgName;
            DeviceValidation.Requirements = ArgRequirement.ValueRequired;

            UsernameValidation = new CommandLineArgValidationOption();
            UsernameValidation.ArgumentName = UsernameArgName;
            UsernameValidation.Requirements = ArgRequirement.ValueRequired;

            FileValidation = new CommandLineArgValidationOption();
            FileValidation.ArgumentName = FileArgName;
            FileValidation.Requirements = ArgRequirement.ValueRequired;

            Validations = new[] {HintValidation, DeviceValidation, FileValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;

            var args = options.ParsedArguments;
            if (CommandLineUtil.ValidateArguments(args, Validations))
            {
                var hintArg = CommandLineUtil.FindArgumentByName(args, HintArgName);
                Pair<string, string> deviceArg;
                Pair<string, string> fileArg;

                var deviceExists = CommandLineUtil.TryGetArgumentByName(args, DeviceArgName, out deviceArg);
                var fileExists = CommandLineUtil.TryGetArgumentByName(args, FileArgName, out fileArg);
                if (deviceExists && fileExists)
                {
                    var msg = "You need to supply either a file or a device, never both.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
                else
                {
                    if (deviceExists)
                    {
                        Pair<string, string> usernameArg;
                        var usernameExists = CommandLineUtil.TryGetArgumentByName(args, UsernameArgName, out usernameArg);
                        if (usernameExists)
                            ExecuteOnDevice(deviceArg, usernameArg, hintArg);
                        else
                        {
                            var msg = "If you are targeting a device, please supply a username for whose password should be discovered.";
                            msg = TextUtil.Error(msg);
                            TerminalUtil.ShowText(msg);
                        }
                    }
                    else if (fileExists)
                        ExecuteOnFile(fileArg, hintArg);
                }
            }
            else
            {
                var msg = "ERROR";
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
        }

        public static void ExecuteOnDevice(Pair<string, string> deviceArg, Pair<string, string> username, Pair<string, string> hintArg)
        {
            var deviceTarget = deviceArg.Value;
            var device = DeviceUtil.FindDeviceByIpOrName(deviceTarget);
            if (device == null)
            {
                var msg = string.Format("No device found with IP or Name equal to '{0}'", deviceTarget);
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
            else
            {
                var hasSSH = DeviceUtil.HasProgram(device, ProgramType.SSH);
                if (hasSSH)
                {
                    var user = DeviceUtil.FindUserByName(device, username.Value);
                    if (user == null)
                    {
                        var msg = string.Format("The device '{0}' has no user with name '{1}'.", deviceTarget, username.Value);
                        msg = TextUtil.Error(msg);
                        TerminalUtil.ShowText(msg);
                    }
                    else
                    {
                        var password = user.Password;
                        LoopUtil.RunCoroutine(ExecuteOnPassword(password, hintArg.Value, success =>
                        {
                            if (success)
                            {
                                var msg = "Password found! The password for '{0}' is '{1}'.";
                                msg = string.Format(msg, user.Username, user.Password);
                                msg = TextUtil.Success(msg);
                                TerminalUtil.ShowText(msg);
                            }
                            else
                            {
                                var msg = string.Format(
                                    "Unable to find the password of user '{0}' with the given arguments.\nPlease try another set of hints.",
                                    user.Username);
                                msg = TextUtil.Error(msg);
                                TerminalUtil.ShowText(msg);
                            }
                        }));
                    }
                }
                else
                {
                    var msg = "The device '{0}' is not running a SSH instance. We have no way to validate password attempts.";
                    msg = string.Format(msg, deviceTarget);
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }
        }

        public static void ExecuteOnFile(Pair<string, string> fileArg, Pair<string, string> hintArg)
        {
            var filePath = fileArg.Value;
            HashFile file = FileSystem.FindFileByPath(filePath);
            if (file == null)
            {
                var msg = string.Format("No file found at '{0}'.", filePath);
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
            else
            {
                var password = file.Password;
                LoopUtil.RunCoroutine(ExecuteOnPassword(password, hintArg.Value, (success) =>
                {
                    if (success)
                    {
                        var msg = string.Format("Password found!\nThe password of '{0}' is: {1}", filePath, file.Password);
                        msg = TextUtil.Success(msg);
                        TerminalUtil.ShowText(msg);
                    }
                    else
                    {
                        var msg = "Unable to find the password of '{0}' with the given arguments. Please try another set of hints.";
                        msg = string.Format(msg, filePath);
                        msg = TextUtil.Error(msg);
                        TerminalUtil.ShowText(msg);
                    }
                }));
            }
        }

        public static IEnumerator ExecuteOnPassword(string password, string rawGuess, Action<bool> callback)
        {
            var guessArray = rawGuess.Split(';');
            var guess = string.Concat(guessArray);

            TerminalUtil.BlockPlayerInput("[BLOCKED WHILE RUNNING CRACKER]");

            var percentage = GetEqualPercentage(password, guessArray);

            var msg = "Generating passowords based on given hints... This might take a few seconds.";
            msg = TextUtil.Warning(msg);
            
            TerminalUtil.ShowText(msg);

            float time = Mathf.Log10(guess.Length);
            time = Mathf.LerpUnclamped(0, 3, time);
            yield return new WaitForSecondsRealtime(time);

            var random = new System.Random(guess.Length / guessArray.Length);
            var minCount = (guessArray.Length * guessArray.Length);
            var passwordsCount = random.Next(minCount, Mathf.CeilToInt((float) (minCount * Math.PI)));
            msg = string.Format("{0} passwords generated.", passwordsCount);
            
            TerminalUtil.ShowText(msg);

            msg = string.Format("Trying password {0}/{1}", 1, passwordsCount);
            TerminalUtil.ShowText(msg);
            
            for (int i = 1; i < passwordsCount; i++)
            {
                msg = string.Format("Trying password {0}/{1}", i + 1, passwordsCount);
                TerminalUtil.ChangeLastText(msg);
                yield return new WaitForSecondsRealtime(.01f);
            }

            TerminalUtil.UnblockPlayerInput();
            yield return null;

            var success = percentage >= PercentualThresholdToSuccess;
            callback(success);
        }
        
        public static float GetEqualPercentage(string realPassword, string[] guessArray)
        {
            // if one of the guesses is exactly equal to the password, it was cracked
            if (Array.Exists(guessArray, g => string.Equals(g, realPassword)))
                return 1;
            
            var guess = string.Concat(guessArray);
            
            float count;
            count = 0f;
            for (int i = 0; i < realPassword.Length; i++)
            {
                if (guess.Contains(realPassword[i].ToString()))
                    count += 1f;
            }

            var realPasswordPercentage = count / realPassword.Length;

            count = 0;
            for (int i = 0; i < guess.Length; i++)
            {
                if (!realPassword.Contains(guess[i].ToString()))
                    count += 0.5f;
            }

            var guessPercentage = count / guess.Length;

            return realPasswordPercentage - guessPercentage;
        }
    }
}