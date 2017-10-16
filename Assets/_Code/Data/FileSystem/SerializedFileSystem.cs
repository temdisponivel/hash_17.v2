using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Class that holds the serialized hash info.
    /// </summary>
    [CreateAssetMenu(fileName = "SerializedFileSystem", menuName = "HASH/Serialized file system")]
    public class SerializedFileSystem : ScriptableObject
    {
        public SerializedHashDir[] Dirs;

        public SerializedHashFileText[] TextFiles;
        public SerializedHashFileImage[] ImageFiles;
    }
}