using System;
using UnityEngine;

namespace HASH
{
    [Serializable]
    public class DreamConfig
    {
        public string Scene;
        public Announcement DreamAnnouncement;
    }
    
    
    [Serializable]
    public class DreamReferences
    {
        [Header("Dreams scenes")] 
        public DreamConfig DreamOne;
    }
}