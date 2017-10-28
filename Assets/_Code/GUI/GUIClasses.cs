using System;
using HASH.Window;
using UnityEngine;

namespace HASH.GUI
{
    [Serializable]
    public class CursorTextureConfig
    {
        public CursorTexture CursorTexture;
        public Texture2D Texture;
    }
    
    [Serializable]
    public class GUIReferences
    {
        public WindowComponent WindowPrefab;
        public TextWindowComponent TextWindowPrefab;
        public ImageWindowComponent ImageWindowPrefab;

        public CursorTextureConfig[] CursorTextures;
    }

    public enum CursorTexture
    {
        Normal,
        Move,
        Hand,
        Block,
        ResizeHorizontal,
        ResizeVertical,
        ResizeBottomLeft,
        ResizeBottomRight,
        ResizeTopLeft,
        ResizeTopRight,
        Loading,
    }

    public enum SetDepthMode
    {
        Increment,
        Decrement,
        Absolute,
    }
}