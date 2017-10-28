using System;
using HASH;
using UnityEngine;
using UnityEngine.Events;

namespace HASH
{
    /// <summary>
    /// Enumerates possible inputs to hear.
    /// </summary>
    public enum InputTypeToListen
    {
        ButtonDown,
        ButtonUp,
        Button,
        KeyDown,
        KeyUp,
        Key,
    }

    /// <summary>
    /// Helds configuration of a input event.
    /// </summary>
    [Serializable]
    public class InputEventConfig
    {
        public KeyCode KeyCode;
        public string ButtonName;
        public InputTypeToListen InputType;
        public UnityEvent Callback;
    }

    /// <summary>
    /// Class that hears for input and calls a bunch of callbacks.
    /// </summary>
    public class InputListener : MonoBehaviour
    {
        public bool Listen;
        public InputEventConfig[] EventsToListen;
        
        private void Update()
        {
            if (!Listen)
                return;

            for (int i = 0; i < EventsToListen.Length; i++)
            {
                var ev = EventsToListen[i];
                switch (ev.InputType)
                {
                    case InputTypeToListen.ButtonDown:
                        if (UnityEngine.Input.GetButtonDown(ev.ButtonName))
                            ev.Callback.Invoke();
                        break;
                    case InputTypeToListen.ButtonUp:
                        if (UnityEngine.Input.GetButtonUp(ev.ButtonName))
                            ev.Callback.Invoke();
                        break;
                    case InputTypeToListen.Button:
                        if (UnityEngine.Input.GetButton(ev.ButtonName))
                            ev.Callback.Invoke();
                        break;
                    case InputTypeToListen.KeyDown:
                        if (UnityEngine.Input.GetKeyDown(ev.KeyCode))
                            ev.Callback.Invoke();
                        break;
                    case InputTypeToListen.KeyUp:
                        if (UnityEngine.Input.GetKeyUp(ev.KeyCode))
                            ev.Callback.Invoke();
                        break;
                    case InputTypeToListen.Key:
                        if (UnityEngine.Input.GetKey(ev.KeyCode))
                            ev.Callback.Invoke();
                        break;
                }
            }
        }
    }
}