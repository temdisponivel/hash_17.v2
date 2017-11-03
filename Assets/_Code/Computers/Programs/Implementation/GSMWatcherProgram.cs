using UnityEngine;

namespace HASH
{
    public static class GSMWatcherProgram
    {
        public static string DevicesData;
        
        public static void Setup(Object aditionalContent)
        {
            var gsmWatcherDevices = aditionalContent as TextAsset;
            DebugUtil.Assert(gsmWatcherDevices == null, "The aditional content of the GSM Watche is not a text watcher.");
            DevicesData = gsmWatcherDevices.text;
        }
        
        public static void Execute(ProgramExecutionOptions options)
        {
            TerminalUtil.ShowText(DevicesData);
        }
    }
}