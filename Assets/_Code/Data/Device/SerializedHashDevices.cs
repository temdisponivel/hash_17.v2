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
    }

    [CreateAssetMenu(fileName = "SerializedDevices", menuName = "HASH/Serialized devices")]
    public class SerializedHashDevices : ScriptableObject
    {
        public string PlayerDeviceName;
        public string PlayerUserName;
        
        public SerializedHashDevice[] Devices;
    }
}