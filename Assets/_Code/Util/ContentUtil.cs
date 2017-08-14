using UnityEngine;

namespace HASH.Util
{
    /// <summary>
    /// Class that helps with loading content.
    /// </summary>
    public static class ContentUtil
    {
        /// <summary>
        /// Calls resource load with the given path.
        /// </summary>
        public static T Load<T>(string assetPath) where T : UnityEngine.Object
        {
            return Resources.Load<T>(assetPath);
        }

        /// <summary>
        /// Unloads the given content from memory.
        /// </summary>
        public static void Unload<T>(T content) where T : UnityEngine.Object
        {
            Resources.UnloadAsset(content);
        }
    }
}