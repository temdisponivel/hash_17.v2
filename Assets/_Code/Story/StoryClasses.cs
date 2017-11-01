using System;
using Ink.Runtime;
using UnityEngine;

namespace HASH.Story
{
    public static class HashStory
    {
        // EXTERNAL FUNC

        public static class ExternalFuncNames
        {
            public const string GET_CURRENT_DAY_FUNC_NAME = "GET_CURRENT_DAY";
            public const string GET_MY_EMAIL_FUNC_NAME = "GET_MY_EMAIL";
            public const string GET_MY_NAME_FUNC_NAME = "GET_MY_NAME";
            
            public const string GET_DAY_ONE_DREAM_FUNC_NAME = "GET_DAY_ONE_DREAM";
            public const string GET_DAY_ONE_SAW_EMAIL_BEFORE_LOG_FUNC_NAME = "GET_DAY_ONE_SAW_EMAIL_BEFORE_LOG";
            
            public const string GET_DAY_TWO_DREAM_FUNC_NAME = "GET_DAY_TWO_DREAM";
            public const string GET_DAY_TWO_SAW_EMAIL_BEFORE_LOG_FUNC_NAME = "GET_DAY_TWO_SAY_EMAIL_BEFORE_LOG";
        }
        
        // MAIN STATE

        public enum StoryDays : short
        {
            One = 1,
            Two = 2,
        }

        public static class MainState
        {
            public static StoryDays CurrentDay;
            public static string MyEmail;
            public static string MyName;
        }

        // DAY ONE

        public enum DayOneDreams : short
        {
            SUSPICIOUS_DREAM = 1,
            TRUSTFULL_DREAM = 2,
            BORED_DREAM = 3,
        }
        
        public static class DayOneState
        {
            public static DayOneDreams  ChoosenDream;
            public static bool SawEmailBeforeLog;
        }

        // DAY TWO

        public enum DayTwoDreams : short
        {
            I_YELL = 1,
            SOMEONE_YELL = 1,
        }

        public static class DayTwoState
        {
            public static DayTwoDreams ChoosenDream;
            public static bool SawEmailBeforeLog;
        }

        // DAY THREE

        public static class DayThreeState
        {
            // Nothing yet
        }
    }
}