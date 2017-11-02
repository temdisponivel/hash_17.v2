using System;
using System.Collections.Generic;
using System.Security.Policy;
using Ink.Runtime;
using UnityEngine;

namespace HASH.Story
{
    public static class HashStory
    {
        [Serializable]
        public class Condition
        {
            public StoryDays MinimalDays;
        }
        
        // EXTERNAL FUNC

        public static class StoryVars
        {
            public static Dictionary<string, object> GlobalVars;
            
            public const string CURRENT_DAY = "CURRENT_DAY";
            public const string MY_EMAIL = "MY_EMAIL";
            public const string MY_NAME = "MY_NAME";
            
            public const string DAY_ONE_DREAM = "DAY_ONE_DREAM";
            public const string DAY_ONE_SAW_EMAIL_BEFORE_LOG = "GET_DAY_ONE_SAW_EMAIL_BEFORE_LOG";
            
            public const string DAY_TWO_DREAM = "DAY_TWO_DREAM";
            public const string DAY_TWO_SAW_EMAIL_BEFORE_LOG = "DAY_TWO_SAY_EMAIL_BEFORE_LOG";
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
            public static string MyName;
            public static string MyEmail;
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
            SOMEONE_YELL = 2,
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