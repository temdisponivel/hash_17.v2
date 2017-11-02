using System;
using System.Collections.Generic;
using UnityEngine;

namespace HASH.Story
{
    public static class StoryUtil
    {
        public const string GET_VAR_VALUE_FUNC_NAME = "GET_VAR_VALUE";
        
        public static void Init()
        {
            HashStory.StoryVars.GlobalVars = new Dictionary<string, object>();
            LoadStoryState();
            UpdateGlobalVars();
        }

        public static void LoadStoryState()
        {
            // TODO: load from save file

            HashStory.MainState.CurrentDay = HashStory.StoryDays.One;
            HashStory.MainState.MyEmail = "seventeen@dpkr.com.br";
            HashStory.MainState.MyName = "Seventeen";

            HashStory.DayOneState.ChoosenDream = HashStory.DayOneDreams.SUSPICIOUS_DREAM;
            HashStory.DayOneState.SawEmailBeforeLog = true;

            HashStory.DayTwoState.ChoosenDream = HashStory.DayTwoDreams.SOMEONE_YELL;
            HashStory.DayTwoState.SawEmailBeforeLog = true;
        }

        public static void UpdateGlobalVars()
        {
            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.CURRENT_DAY] = (int) HashStory.MainState.CurrentDay;
            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.MY_EMAIL] = HashStory.MainState.MyEmail;
            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.MY_NAME] = HashStory.MainState.MyName;

            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.DAY_ONE_DREAM] = (int) HashStory.DayOneState.ChoosenDream;
            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.DAY_ONE_SAW_EMAIL_BEFORE_LOG] = HashStory.DayOneState.SawEmailBeforeLog;

            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.DAY_TWO_DREAM] = (int) HashStory.DayTwoState.ChoosenDream;
            HashStory.StoryVars.GlobalVars[HashStory.StoryVars.DAY_TWO_SAW_EMAIL_BEFORE_LOG] = HashStory.DayTwoState.SawEmailBeforeLog;
        }
        
        public static void BindExternalFunctions(Ink.Runtime.Story story)
        {
            story.BindExternalFunction(GET_VAR_VALUE_FUNC_NAME, (Func<string, object>) GetVarValue);
        }
        
        public static void UpdateDayThree()
        {
            // nothing yet
        }

        // INK-CALLED FUNCTIONS
        public static object GetVarValue(string var)
        {
            object value;
            if (!HashStory.StoryVars.GlobalVars.TryGetValue(var, out value))
                DebugUtil.Error(string.Format("Did not found variable: {0} on story state.", var));

            return value;
        }
    }
}