namespace HASH
{
    public static class SSHProgram
    {
        public const string IpArgName = "ip";
        public const string UserNameArgName = "u";
        public const string PasswordArgName = "p";
        
        public static CommandLineArgValidationOption[] Validations;
        
        public static CommandLineArgValidationOption IpValidation;
        public static CommandLineArgValidationOption UserNameValidation;
        public static CommandLineArgValidationOption PasswordValidation;

        public static void Setup()
        {
            IpValidation = new CommandLineArgValidationOption();
            IpValidation.ArgumentName = IpArgName;
            IpValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique | ArgRequirement.ValueRequired;
            
            UserNameValidation = new CommandLineArgValidationOption();
            UserNameValidation.ArgumentName = UserNameArgName;
            UserNameValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique;
            
            PasswordValidation = new CommandLineArgValidationOption();
            PasswordValidation.ArgumentName = PasswordArgName;
            PasswordValidation.Requirements = ArgRequirement.Required | ArgRequirement.Unique;
            
            Validations = new[] {IpValidation, UserNameValidation, PasswordValidation};
        }
        
        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;

            if (CommandLineUtil.ValidateArguments(options.ParsedArguments, Validations))
            {
                var ip = CommandLineUtil.FindArgumentByName(options.ParsedArguments, IpArgName).Value;
                var userName = CommandLineUtil.FindArgumentByName(options.ParsedArguments, UserNameArgName).Value;
                var password = CommandLineUtil.FindArgumentByName(options.ParsedArguments, PasswordArgName).Value;

                var device = DeviceUtil.FindDeviceByIp(ip);
                if (device == null)
                {
                    var msg = string.Format("No device with ip '{0}' was found.", ip);
                    msg = TextUtil.Error(msg);
                    TerminalUtil.ShowText(msg);
                }
                else
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
            }
            else
            {
                TerminalUtil.ShowText("ERROR, PLEASE USE HELP. DUMB ASS");
            }
        }
    }
}