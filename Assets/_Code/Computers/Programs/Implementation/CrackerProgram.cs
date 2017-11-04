using UnityEngine;

namespace HASH
{
    public static class CrackerProgram
    {
        public const string HintArgName = "h";
        public const string DeviceArgName = "d";
        public const string FileArgName = "f";


        public static void Execute(ProgramExecutionOptions options)
        {
            if (ProgramUtil.ShowHelpIfNeeded(options))
                return;
            
            
        }

        public static float GetEqualPercentage(string realPassword, string guess)
        {
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