using HASH;
using UnityEngine;

namespace HASH.GUI
{
    public class WindowResizer : MonoBehaviour
    {
        public WindowComponent Window;
        public UIDragResize DragResize;

        // Called by NGUI
        void OnDrag(Vector2 delta)
        {
            WindowUtil.KeepWindowInsideScreen(Window);
        }

        void OnHover(bool hover)
        {
            var window = WindowUtil.GetWindowFromWindowComponent(Window);
            if (hover)
            {
                if (window.State.CanBeResized)
                    GUIUtil.SetCursorToWindowResizer(DragResize.pivot);
                else
                    GUIUtil.SetCursorTexture(CursorTexture.Block);
            }
            else
                GUIUtil.SetCursorToDefault();
        }

#if UNITY_EDITOR

        void Reset()
        {
            Window = GetComponentInParent<WindowComponent>();
            DragResize = GetComponent<UIDragResize>();
        }

#endif
    }
}