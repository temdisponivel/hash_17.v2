﻿using System;
using UnityEngine;

namespace HASH
{
    [DisallowMultipleComponent]
    public class WindowClickListener : MonoBehaviour
    {
        public Action Callback;

        // called by NGUI
        void OnClick()
        {
            if (Callback != null)
                Callback();
        }
    }
}