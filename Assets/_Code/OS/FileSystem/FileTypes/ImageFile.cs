using UnityEngine;

namespace HASH.OS.FileSystem.FileTypes
{
    /// <summary>
    /// Represents a image file on our virtual OS.
    /// </summary>
    public class ImageFile
    {
        public HashFile File;

        public string ImageContentAssetPath;
        public Texture ImageContent;
    }
}