﻿using UnityEngine;

namespace HASH.OS.FileSystem.FileTypes
{
    /// <summary>
    /// Represents a text file on our virtual OS.
    /// </summary>
    public class TextFile
    {
        public HashFile File;

        public string TextContentAssetPath;
        public string TextContent;
    }
}