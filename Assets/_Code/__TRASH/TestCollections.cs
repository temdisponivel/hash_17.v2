using System;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleCollections.Hash;
using SimpleCollections.Lists;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets._Code.__TRASH
{
    [Serializable]
    public class StringList : SimpleList<int> { }

    [Serializable]
    public class IntDic : SimpleTable<int, string> { }

    public class TestCollections : MonoBehaviour
    {
        public SimpleList<int> list;
        //public IntDic Dic;

        void Awake()
        {
            //TestDic();
            TestSimpleList();
        }

        void TestDic()
        {
            Stopwatch stop = new Stopwatch();
            stop.Start();
            var dic = new Dictionary<int, string>(10);
            //var dic = STable.Create<int, string>(10, true);
            for (int i = 0; i < 100000; i++)
                dic[i] = i + "ALO";
            //stop.Start();
            for (int i = 0; i < 100000; i++)
            {
                string name;
                //if (STable.TryGetValue(dic, i, out name))
                if (dic.TryGetValue(i, out name))
                    name += name;
            }
            stop.Stop();
            Debug.Log(stop.ElapsedMilliseconds);
        }
        
        void TestSimpleList()
        {
            var stop = new Stopwatch();
            stop.Start();
            var list = SList.Create<int>(10);
            for (int i = 0; i < 1000000; i++)
                SList.Add(list, i);
            //var list = new List<int>(10);
            //for (int i = 0; i < 1000000; i++)
            //    list.Add(i);

            var sum = 0;
            for (int i = 0; i < 1000000; i++)
                sum += list[i];
            stop.Stop();
            Debug.Log(stop.ElapsedMilliseconds);
        }

        private void Log()
        {
            for (int i = 0; i < list.Count; i++)
                Debug.Log(list[i]);
        }
    }
}