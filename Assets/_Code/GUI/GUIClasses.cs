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
        [Header("TERMINAL")]
        public SingleTextEntry SingleText;
        public DualTextEntry DualText;

        public ImageEntry Image;

        public UIRoot UIRoot;
        public UIPanel MainPanel;
        
        public UITable TextTable;
        public UIScrollView ScrollView;

        public UIInput Input;
        public UILabel CurrentPath;
        
        public InputListener InputListener;
        
        [Header("WINDOW")]
        public TerminalComponent TerminalComponent;
        
        public UIPanel WindowsPanel;
        public UIPanel FocusedWindowsPanel;
        
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