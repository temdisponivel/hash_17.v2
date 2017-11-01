using System;
using UnityEngine;

namespace HASH.Story
{
    public static class StoryUtil
    {
        public static void Init()
        {
            LoadStoryState();
        }

        public static void LoadStoryState()
        {
            // TODO: load from save file

            HashStory.MainState.CurrentDay = HashStory.StoryDays.One;
            HashStory.MainState.MyEmail = "seventeen@dpkr.com.br";
            HashStory.MainState.MyName = "Seventeen";

            HashStory.DayOneState.ChoosenDream = HashStory.DayOneDreams.BORED_DREAM;
            HashStory.DayOneState.SawEmailBeforeLog = true;

            HashStory.DayTwoState.ChoosenDream = HashStory.DayTwoDreams.SOMEONE_YELL;
            HashStory.DayTwoState.SawEmailBeforeLog = true;
        }
        
        public static void BindExternalFunctions(Ink.Runtime.Story story)
        {
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_CURRENT_DAY_FUNC_NAME, (Func<object>) GetCurrentDay);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_MY_NAME_FUNC_NAME, (Func<object>) GetMyName);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_MY_EMAIL_FUNC_NAME, (Func<object>) GetMyEmail);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_DAY_ONE_DREAM_FUNC_NAME, (Func<object>) GetDayOneDream);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_DAY_ONE_SAW_EMAIL_BEFORE_LOG_FUNC_NAME, (Func<object>) GetDayOneSawEmailBeforeLog);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_DAY_TWO_DREAM_FUNC_NAME, (Func<object>) GetDayTwoDream);
            story.BindExternalFunction(HashStory.ExternalFuncNames.GET_DAY_TWO_SAW_EMAIL_BEFORE_LOG_FUNC_NAME, (Func<object>) GetDayTwoSawEmailBeforeLog);
        }
        
        public static void UpdateDayThree()
        {
            // nothing yet
        }
        
        // INK-CALLED FUNCTIONS
        
        // MAIN STATE
        
        public static object GetCurrentDay()
        {
            return (int) HashStory.MainState.CurrentDay;
        }

        public static object GetMyName()
        {
            return HashStory.MainState.MyName;
        }
        
        public static object GetMyEmail()
        {
            return HashStory.MainState.MyEmail;
        }
        
        // DAY ONE

        public static object GetDayOneDream()
        {
            return (int) HashStory.DayOneState.ChoosenDream;
        }

        public static object GetDayOneSawEmailBeforeLog()
        {
            return HashStory.DayOneState.SawEmailBeforeLog;
        }
        
        // DAY TWO

        public static object GetDayTwoDream()
        {
            return (int) HashStory.DayTwoState.ChoosenDream;
        }
        
        public static object GetDayTwoSawEmailBeforeLog()
        {
            return HashStory.DayTwoState.SawEmailBeforeLog;
        }
    }
}