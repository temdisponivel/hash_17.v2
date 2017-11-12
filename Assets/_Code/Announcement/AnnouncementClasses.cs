using System;

namespace HASH
{
    [Serializable]
    public class Announcement
    {
        public AnnouncementNode[] Nodes;
    }

    [Serializable]
    public class AnnouncementNode
    {
        public string Text;
        public float TimeOnScreen;
    }
}