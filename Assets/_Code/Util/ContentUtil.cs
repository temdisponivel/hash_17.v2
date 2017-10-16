using UnityEngine;

namespace HASH
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
            var asset = Resources.Load<T>(assetPath);
            DebugUtil.Assert(asset == null, string.Format("No asset of type '{0}' found at path '{1}'.", typeof(T).Name, assetPath));
            return asset;
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