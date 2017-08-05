using System.Collections.Generic;
using HASH.Game;
using UnityEngine;

namespace Assets._Code.__TRASH
{
    public static class TrashUtil
    {
        public static List<IInitializable> GetAllInitilizableChildren(Transform trans)
        {
            var childs = new List<IInitializable>();

            // Get all imediate children that are IInitializable and initializes them
            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                var childInitializable = child.GetComponent<IInitializable>();
                childs.Add(childInitializable);
            }

            return childs;
        }

        public static void InitializeAllChildrenInOrder(Transform transform)
        {
            var childs = GetAllInitilizableChildren(transform);
            childs.Sort((a, b) => a.GetOrder() - b.GetOrder());
            for (int i = 0; i < childs.Count; i++)
                childs[i].Initialize();
            
        }
    }
}