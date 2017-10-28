﻿using UnityEngine;

namespace HASH
{
    [CreateAssetMenu(fileName = "HashFile", menuName = "HASH/File")]
    public class HashFileSO : ScriptableObject
    {
        public PermissionPair[] Permissions;
        public HashFileType Type;
        public FileStatus Status;
        public Object Content;

        private void OnValidate()
        {
            if (Type == HashFileType.Text)
                DebugUtil.AssertContext((Content as TextAsset) == null, "Text file with a invalid text asset", this);
            else if (Type == HashFileType.Image)
                DebugUtil.AssertContext((Content as Texture2D) == null, "Image file with a invalid texture asset", this);
            else
                DebugUtil.AssertContext(true, "File with invalid type!", this);
        }
    }
}