using SimpleCollections.Lists;

namespace HASH.OS
{
    /// <summary>
    /// Class that holds the information that represents our virtual operational system.
    /// </summary>
    public class HashSO
    {
        public HashUser LoggedUser;
        public SimpleList<HashUser> Users;
    }
}