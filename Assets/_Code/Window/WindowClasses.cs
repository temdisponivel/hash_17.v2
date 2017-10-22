using UnityEngine;

namespace HASH.Window
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
    }

    public struct WindowResizingProperties
    {
        public Vector2 PreviousPosition;
        public Vector2 PreviousSize;
    }
}