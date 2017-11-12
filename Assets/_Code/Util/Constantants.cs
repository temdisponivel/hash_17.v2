using System;
using UnityEngine;

namespace HASH
{
    [Serializable]
    public class HashColors
    {
        public Color Default = new Color(0.93f, 1f, 0.92f);
        public Color Success = new Color(0.35f, 1f, 0.35f);
        public Color Error = new Color(1f, 0.37f, 0.4f);
        public Color Warning = new Color(1f, 0.99f, 0.49f);
    }
    
    [Serializable]
    public class InputNames
    {
        public string InteractButton = "Interact";
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public string MouseHorizontalAxis = "Mouse X";
        public string MouseVerticalAxis = "Mouse Y";
    }

    public static class Constants
    {
        public static HashColors Colors = new HashColors();
        public static InputNames InputNames = new InputNames();
    }

    public static class TagsAndLayers
    {
        public const string Player = "Player";
    }
}