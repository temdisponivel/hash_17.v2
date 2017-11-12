using UnityEngine;

namespace HASH
{
    public enum WindowType
    {
        TextWindow,
        ImageWindow,
    }
    
    public class Window
    {
        public int WindowId;
        public WindowType Type;
        public WindowComponent SceneWindow;
        public object WindowContent;

        public WindowState State;
    }

    public class WindowState
    {
        public bool CanBeClosed;
        public bool CanBeMoved;
        public bool CanBeResized;
    }

    public struct WindowResizingProperties
    {
        public Vector2 PreviousPosition;
        public Vector2 PreviousSize;
    }
}