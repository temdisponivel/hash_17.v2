namespace HASH
{
    public static class SSHProgram
    {
        public const string TargetArgName = "t";
        public const string UserNameArgName = "u";
        public const string PasswordArgName = "p";

        public static CommandLineArgValidationOption[] Validations;

        public static CommandLineArgValidationOption TargetValidation;
        public static CommandLineArgValidationOption UserNameValidation;
        public static CommandLineArgValidationOption PasswordValidation;

        public static void Setup()
        {
            TargetValidation = new CommandLineArgValidationOption();
            TargetValidation.ArgumentName = TargetArgName;
            TargetValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;

            UserNameValidation = new CommandLineArgValidationOption();
            UserNameValidation.ArgumentName = UserNameArgName;
            UserNameValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique;

            PasswordValidation = new CommandLineArgValidationOption();
            PasswordValidation.ArgumentName = PasswordArgName;
            PasswordValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique;

            Validations = new[] {TargetValidation, UserNameValidation, PasswordValidation};
        }

        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;

            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                var target = CommandLineUtil.FindArgumentByName(options.ParsedArguments, TargetArgName).Value;
                var userName = CommandLineUtil.FindArgumentByName(options.ParsedArguments, UserNameArgName).Value;
                var password = CommandLineUtil.FindArgumentByName(options.ParsedArguments, PasswordArgName).Value;

                var device = DeviceUtil.FindDeviceByIpOrName(target);
                if (device == null)
                {
                    var msg = string.Format("No device found with IP or Name equal to '{0}'.", target);
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
                else
                    RunOnDevice(device, userName, password);
            }
            else
            {
                TerminalUtil.ShowText("ERROR, PLEASE USE HELP. DUMB ASS");
            }
        }

        public static void RunOnDevice(HashDevice device, string userName, string password)
        {
            var hasSSH = DeviceUtil.HasProgram(device, ProgramType.SSH);
            if (hasSSH)
            {
                if (DeviceUtil.TryLogin(device, userName, password))
                {
                    var user = DeviceUtil.FindUserByName(device, userName);
                    DeviceUtil.ChangeDevice(device, user);

                    var msg = string.Format("Successfully logged into '{0}'", device.DeviceName);
                    msg = TextUtil.Success(msg);
                    TerminalUtil.ShowText(msg);
                }
                else
                {
                    var msg = "Username or password invalid.";
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
            }
            else
            {
                var msg = "The device '{0}' is not running a SSH instance, therefore cannot be accessed using a SSH.";
                msg = string.Format(msg, device.IpAddress);
                msg = TextUtil.Error(msg);
                TerminalUtil.ShowText(msg);
            }
        }
    }
}