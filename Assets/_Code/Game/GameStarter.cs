using System.Collections;
using HASH.GUI;
using HASH.Story;
using HASH.Window;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Behaviour that holds data to useful components and sets everything up.
    /// </summary>
    public class GameStarter : MonoBehaviour
    {
        public GUIReferences GUIReferences;
        public DebugUtil.DebugCondition DebugCondition;
        public HashColors Colors;
        
        void Start()
        {
            InitializeGame();
        }

        /// <summary>
        /// Initialized the game.
        /// </summary>
        public void InitializeGame()
        {        
#if DEB
            if (GUIReferences.InputListener == null)
                DebugUtil.Error("INPUT LISTENER IS NULL. PLEASE BAKE THE GAME HOLDER!");
            if (GUIReferences.TerminalComponent == null)
                DebugUtil.Error("TERMINAL COMPONENT IS NULL. PLEASE BAKE THE GAME HOLDER!");
#endif
            
            Constants.Colors = Colors;
            DataHolder.DebugCondition = DebugCondition;
            DataHolder.GUIReferences = GUIReferences;
            
            DataHolder.GUIReferences.TerminalComponent.Initialize();
            StoryUtil.Init();
            LoopUtil.Init();
            WindowUtil.Initialize();
            DataUtil.LoadData();
            ProgramUtil.SetupPrograms();
            GUIUtil.SetCursorToDefault();
        }

#if DEB && UNITY_EDITOR
        [ContextMenu("BAKE")]
        void Bake()
        {
            GUIReferences.InputListener = FindObjectOfType<InputListener>();
            DebugUtil.Assert(GUIReferences.InputListener == null, "DID NOT FOUND INPUTLISTENER. IS IT ACTIVE ON THE SCENE?");

            GUIReferences.TerminalComponent = FindObjectOfType<TerminalComponent>();
            DebugUtil.Assert(GUIReferences.TerminalComponent == null, "DID NOT FOUND TERMINALCOMPONENT. IS IT ACTIVE ON THE SCENE?");
        }
#endif
    }
}