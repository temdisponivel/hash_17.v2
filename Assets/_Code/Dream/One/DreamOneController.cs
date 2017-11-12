using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace HASH
{
    public class DreamOneState
    {
        public DreamOneReferences References;
        public Action Callback;
    }

    public static class DreamOneController
    {
        public static DreamOneState CurrentState;

        public static void Run(DreamOneReferences dreamOneReferences, Action callback)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            
            CurrentState = new DreamOneState();
            CurrentState.Callback = callback;
            CurrentState.References = dreamOneReferences;

            Camera.SetupCurrent(CurrentState.References.Camera);

            dreamOneReferences.PlayerController.Initiate();
            CurrentState.References.Telephone.Initialize();
        }

        // Called by DreamOneTelephone
        public static void OnInteractWithPhone()
        {
            CurrentState.References.Telephone.Finish();
            CurrentState.References.PlayerController.Finish();

            CurrentState.Callback();
            Cursor.visible = true;
        }
    }
}