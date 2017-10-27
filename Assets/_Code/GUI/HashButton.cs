using System;
using UnityEngine;
using UnityEngine.Events;

namespace HASH.GUI
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(UIWidget))]
    public class HashButton : MonoBehaviour
    {
        public UITexture Texture;

        public Color NormalColor;
        public Color HoverColor;
        public Color PressedColor;

        public EventDelegate ClickCallback;
        public EventDelegate DoubleClickCallback;

        public event Action OnClickCallback;
        public event Action OnDoubleClickCallback;

        public CursorTexture OnHoverCursor;

        public bool IgnoreClick;

        // Called by NGUI
        void OnHover(bool isOver)
        {
            if (isOver)
                GUIUtil.SetCursorTexture(OnHoverCursor);
            else
                GUIUtil.SetCursorToDefault();
            
            if (!Texture)
                return;
            
            if (isOver)
                Texture.color = HoverColor;
            else
                Texture.color = NormalColor;
        }

        // Called by NGUI
        void OnPress(bool isDown)
        {
            if (IgnoreClick)
                return;
            
            if (isDown)
            {
                if (Texture)
                    Texture.color = PressedColor;
            }
            else
            {
                if (Texture)
                    Texture.color = NormalColor;
                
                // validation needed because the player can release the press outside this button
                if (UICamera.hoveredObject == gameObject)
                {
                    ClickCallback.Execute();

                    if (OnClickCallback != null)
                        OnClickCallback();
                }
            }
        }

        // Called by NGUI
        void OnDoubleClick()
        {
            if (IgnoreClick)
                return;
            
            DoubleClickCallback.Execute();
            if (OnDoubleClickCallback != null)
                OnDoubleClickCallback();
        }
    }
}