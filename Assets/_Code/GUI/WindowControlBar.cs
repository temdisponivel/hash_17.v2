using HASH.Window;
using UnityEngine;

namespace HASH.GUI
{
    public class WindowControlBar : MonoBehaviour
    {
        public WindowComponent Window;
        public UIWidget ControlBox;
        public UIDragObject DragObject;

        void OnHover(bool hover)
        {
            var window = WindowUtil.GetWindowFromWindowComponent(Window);

            if (hover)
            {
                if (window.CanBeMoved)
                    GUIUtil.SetCursorTexture(CursorTexture.Move);
                else
                    GUIUtil.SetCursorTexture(CursorTexture.Block);
            }
            else
                GUIUtil.SetCursorTexture(CursorTexture.Normal);
        }

#if UNITY_EDITOR

        void Reset()
        {
            Window = GetComponentInParent<WindowComponent>();
            ControlBox = GetComponent<UIWidget>();
            DragObject = GetComponent<UIDragObject>();
        }

#endif
    }
}