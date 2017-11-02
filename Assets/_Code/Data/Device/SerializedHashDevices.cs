using System;
using HASH.Story;
using UnityEngine;

namespace HASH
{
    [Serializable]
    public class SerializedHashDevice
    {
        public string DeviceName;
        public string IpAddress;
        
        public SerializedHashUser[] Users;
        public SerializedProgram[] Programs;
        public SerializedFileSystem FileSystem;

        public HashStory.Condition Condition;

        public void OnValidate()
        {
            DebugUtil.Assert(string.IsNullOrEmpty(DeviceName), "PLEASE DEFINE A DEVICE NAME");
        }
    }

    [CreateAssetMenu(fileName = "SerializedDevices", menuName = "HASH/Serialized devices")]
    public class SerializedHashDevices : ScriptableObject
    {
        public string PlayerDeviceName;
        public string PlayerUserName;
        
        public SerializedHashDevice[] Devices;

        private void OnValidate()
        {
            DebugUtil.Assert(string.IsNullOrEmpty(PlayerDeviceName), "PLEASE DEFINE A MAIN DEVICE!");
            DebugUtil.Assert(string.IsNullOrEmpty(PlayerUserName), "PLEASE DEFINE A MAIN USER!");
        }
    }
}