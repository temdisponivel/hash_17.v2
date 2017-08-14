using System.Collections;
using HASH.Data;
using HASH.OS.Programs;
using HASH.OS.Shell;
using HASH.Terminal;
using HASH.Util;
using HASH.Util.Input;
using UnityEngine;

namespace HASH.Game
{
    /// <summary>
    /// Behaviour that holds references to useful components and sets everything up.
    /// </summary>
    public class GameStarter : MonoBehaviour
    {
        public InputListener InputListener;
        public TerminalComponent TerminalComponent;
        public DebugUtil.DebugCondition DebugCondition;
        
        IEnumerator Start()
        {
            yield return InitializeGame();
        }

        /// <summary>
        /// Initialized the game.
        /// </summary>
        public IEnumerator InitializeGame()
        {
#if DEB
            if (InputListener == null)
                DebugUtil.Error("INPUT LISTENER IS NULL. PLEASE BAKE THE GAME HOLDER!");
            if (TerminalComponent == null)
                DebugUtil.Error("TERMINAL COMPONENT IS NULL. PLEASE BAKE THE GAME HOLDER!");
#endif

            Global.DebugCondition = DebugCondition;

            yield return DataUtil.Load();
            DataUtil.ProcessLoadedData();

            InputListener.Initialize();
            TerminalComponent.Initialize();
        }

#if UNITY_EDITOR && DEB
        [ContextMenu("BAKE")]
        void Bake()
        {
            InputListener = FindObjectOfType<InputListener>();
            DebugUtil.Assert(InputListener == null, "DID NOT FOUND INPUTLISTENER. IS IT ACTIVE ON THE SCENE?");

            TerminalComponent = FindObjectOfType<TerminalComponent>();
            DebugUtil.Assert(TerminalComponent == null, "DID NOT FOUND TERMINALCOMPONENT. IS IT ACTIVE ON THE SCENE?");
        }
#endif
    }
}