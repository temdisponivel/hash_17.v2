using HASH17.Util;
using UnityEngine;

namespace HASH.Game
{
    /// <summary>
    /// Behaviour that holds references to useful components and sets everything up.
    /// </summary>
    public class GameHolder : MonoBehaviour
    {
#if DEB
        public DebugUtil.DebugCondition DebugCondition;
#endif

        void Awake()
        {
            GameObjectUtil.InitializeAllChildren(transform);
            Global.DebugCondition = DebugCondition;
        }
    }
}