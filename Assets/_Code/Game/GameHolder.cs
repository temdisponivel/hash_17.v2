using HASH.OS.Programs;
using HASH.OS.Shell;
using HASH17.Terminal;
using HASH17.Util;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.Game
{
    /// <summary>
    /// Behaviour that holds references to useful components and sets everything up.
    /// </summary>
    public class GameHolder : MonoBehaviour
    {
        public DebugUtil.DebugCondition DebugCondition;

#if DEB
        public bool SetupMockData;
#endif

        void Awake()
        {
            GameObjectUtil.InitializeAllChildren(transform);
            Global.DebugCondition = DebugCondition;

#if DEB
            if (SetupMockData)
            {
                var programData = new ProgramsData();
                programData.AllPrograms = SList.Create<Program>(1);
                programData.ArgNameSetHelper = SSet.Create<string>(1, true);
                var cdProgram = new Program();
                cdProgram.ProgramType = ProgramType.Cd;
                cdProgram.Commands = new string[2] { "cd", "alo" };
                SList.Add(programData.AllPrograms, cdProgram);

                Global.ProgramData = programData;

                var terminalData = new TerminalData();
                
            }
#endif
        }
    }
}