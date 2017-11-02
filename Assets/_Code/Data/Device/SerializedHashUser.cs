using UnityEngine;

namespace HASH
{
    [CreateAssetMenu(fileName = "HashUser", menuName = "HASH/User")]
    public class SerializedHashUser : ScriptableObject
    {
        public string UserName;
        public string Password;
    }
}