using HASH.Story;
using SimpleCollections.Lists;

namespace HASH
{
    public class HashUser
    {
        public string Username;
        public string Password;
    }
    
    public class DeviceData
    {
        public HashDevice PlayerDevice;

        public HashUser CurrentUser;
        public HashDevice CurrentDevice;
        
        public SimpleList<HashDevice> AllDevices;
    }

    public class HashDevice
    {
        public string DeviceName;
        public string IpAddress;

        public SimpleList<Program> AllPrograms;
        public SimpleList<HashUser> AllUsers;
        public FileSystemData FileSystem;

        public HashStory.Condition Condition;
    }
}