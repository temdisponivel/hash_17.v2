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

        public event Action OnClickCallback;

        // Called by NGUI
        void OnHover(bool isOver)
        {
            if (isOver)
                Texture.color = HoverColor;
            else
                Texture.color = NormalColor;
        }

        // called by NGUI
        void OnPress(bool isDown)
        {
            if (isDown)
                Texture.color = PressedColor;
            else
            {
                Texture.color = NormalColor;
                
                // validation needed because the player can release the press outside this button
                if (UICamera.hoveredObject == gameObject)
                {
                    Debug.Log("CLICK");
                    
                    ClickCallback.Execute();

                    if (OnClickCallback != null)
                        OnClickCallback();
                }
            }
        }
    }
}