using System.Collections;
using HASH;
using HASH.Window;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Behaviour that holds references to useful components and sets everything up.
    /// </summary>
    public class GameStarter : MonoBehaviour
    {
        public InputListener InputListener;
        public TerminalComponent TerminalComponent;
        public WindowReferences WindowReferences;
        public DebugUtil.DebugCondition DebugCondition;
        public HashColors Colors;
        
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

            Constants.Colors = Colors;
            
            DataHolder.DebugCondition = DebugCondition;
            DataHolder.WindowReferences = WindowReferences;

            yield return DataUtil.Load();
            DataUtil.ProcessLoadedData();

            InputListener.Initialize();
            TerminalComponent.Initialize();

            Cursor.lockState = CursorLockMode.Confined;

            DataHolder.FileSystemData.RootDir = FileSystem.GetRootDir();
            FileSystem.ChangeDir(DataHolder.FileSystemData.RootDir);
            
            SetupPrograms();
            
            WindowUtil.Initialize();
        }

        public void SetupPrograms()
        {
            CdProgram.Setup();
            DirProgram.Setup();
            ClearProgram.Setup();
            OpenProgram.Setup();
        }

#if DEB && UNITY_EDITOR
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