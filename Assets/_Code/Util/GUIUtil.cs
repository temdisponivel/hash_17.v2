using System;
using HASH.GUI;
using UnityEngine;

namespace HASH
{
    public static class GUIUtil
    {
        public static void SetCursorToDefault()
        {
            SetCursorTexture(CursorTexture.Normal);
        }

        public static void SetCursorTexture(CursorTexture cursorTexture)
        {
            Texture2D texture = GetTextureFromCursorTexture(cursorTexture);
            float width = 0;
            float height = 0;
            if (cursorTexture != CursorTexture.Normal)
            {
                width = texture.width / 2f;
                height = texture.height / 2f;
            }
            Cursor.SetCursor(texture, new Vector2(width, height), CursorMode.Auto);
        }

        public static void SetCursorToWindowResizer(UIWidget.Pivot pivot)
        {
            var texture = GetCursorTextureFromWindowResizePivot(pivot);
            SetCursorTexture(texture);
        }

        public static CursorTexture GetCursorTextureFromWindowResizePivot(UIWidget.Pivot pivot)
        {
            switch (pivot)
            {
                case UIWidget.Pivot.TopLeft:
                    return CursorTexture.ResizeTopLeft;
                case UIWidget.Pivot.Top:
                    return CursorTexture.ResizeVertical;
                case UIWidget.Pivot.TopRight:
                    return CursorTexture.ResizeTopRight;
                case UIWidget.Pivot.Left:
                    return CursorTexture.ResizeHorizontal;
                case UIWidget.Pivot.Center:
                    return CursorTexture.Normal;
                case UIWidget.Pivot.Right:
                    return CursorTexture.ResizeHorizontal;
                case UIWidget.Pivot.BottomLeft:
                    return CursorTexture.ResizeBottomLeft;
                case UIWidget.Pivot.Bottom:
                    return CursorTexture.ResizeVertical;
                case UIWidget.Pivot.BottomRight:
                    return CursorTexture.ResizeBottomRight;
                default:
                    return CursorTexture.Normal;
            }
        }

        public static Texture2D GetTextureFromCursorTexture(CursorTexture cursorTexture)
        {
            var textures = DataHolder.GUIReferences.CursorTextures;
            for (int i = 0; i < textures.Length; i++)
            {
                var texture = textures[i];
                if (texture.CursorTexture == cursorTexture)
                    return texture.Texture;
            }

            return null;
        }

        public static void UpdateButtonCursor(HashButton button)
        {
            if (button.IgnoreClick)
                button.OnHoverCursor = CursorTexture.Block;
            else
                button.OnHoverCursor = CursorTexture.Normal;
        }
        
        public static void UpdateScrollBar(UIScrollView scroll)
        {
            var backgroundWidget = scroll.verticalScrollBar.backgroundWidget;
            var foregroundWidget = scroll.verticalScrollBar.foregroundWidget;

            if (backgroundWidget)
                foregroundWidget.height = backgroundWidget.height;
            
            scroll.UpdateScrollbars(true);
            scroll.UpdatePosition();
        }
    }
}