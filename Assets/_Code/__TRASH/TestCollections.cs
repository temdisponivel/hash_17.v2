using System;
using SimpleCollections.Lists;
using UnityEngine;

namespace Assets._Code.__TRASH
{
    [Serializable]
    public class StringList : SimpleList<int> { }

    public class TestCollections : MonoBehaviour
    {
        public SimpleList<int> list;

        void Awake()
        {
            TestSimpleList();
        }

        void TestSimpleList()
        {
            list = SList.Create<int>(5);
            for (int i = 1; i <= 5; i++)
                SList.Add(list, i);
            Log();

            var newList = SList.Create<int>(4);
            SList.CopyRange(list, 1, newList, 0, newList.Capacity);
            //SList.CopyRange(list, 2, newList, 0, 4);

            SList.RemoveFirst(list);
            Debug.Log("REMOVE");

            list = newList;

            Log();

            return;

            SList.Remove(list, 2);
            SList.Delete(list, (value) => value == 3);
            Debug.Log("CONSTAINS 99: " + SList.Contains(list, 99));
            Log();
            Debug.Log("CAPACITY: " + list.Capacity);
            SList.Clear(list);
            Log();
            SList.EnsureCapacity(list, 300);
            Debug.Log("CAPACITY: " + list.Capacity);
            for (int i = 0; i < list.Capacity; i++)
                list[i] = i;
            Log();
        }

        private void Log()
        {
            for (int i = 0; i < list.Count; i++)
                Debug.Log(list[i]);
        }
    }
}