using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    public class LoopUtil : MonoBehaviour
    {
        private static LoopUtil Instance;
        
        public static SimpleList<Action> UpdateCallbacks;
        public static SimpleList<Action> LateUpdateCallbacks;

        public static void Init()
        {
            DebugUtil.Assert(Instance != null, "LoopUtil.Init being called multiple times!");
            
            Instance = new GameObject("LOOP-UTIL").AddComponent<LoopUtil>();
            UpdateCallbacks = SList.Create<Action>(5);
            LateUpdateCallbacks = SList.Create<Action>(3);
        }

        void Update()
        {
            for (int i = 0; i < UpdateCallbacks.Count; i++)
                UpdateCallbacks[i]();
        }

        void LateUpdate()
        {
            for (int i = 0; i < LateUpdateCallbacks.Count; i++)
                LateUpdateCallbacks[i]();
        }

        public static void AddUpdate(Action callback)
        {
            SList.Add(UpdateCallbacks, callback);
        }

        public static void RemoveUpdate(Action callback)
        {
            SList.Remove(UpdateCallbacks, callback);
        }
        
        public static void AddLateUpdate(Action callback)
        {
            SList.Add(LateUpdateCallbacks, callback);
        }

        public static void RemoveLateUpdate(Action callback)
        {
            SList.Remove(LateUpdateCallbacks, callback);
        }

        public static Coroutine CallForever(Action callback, float timeInterval)
        {
            return Instance.StartCoroutine(CallForeverEnumerator(callback, timeInterval));
        }

        public static Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
        }

        public static IEnumerator CallForeverEnumerator(Action callback, float timeInterval)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInterval);
                callback();
            }
        }
    }
}