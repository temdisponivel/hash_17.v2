using System;
using System.Collections.Generic;
using HASH.Game;
using UnityEngine;

namespace HASH17.Util
{
    /// <summary>
    /// Utility class for handling stuff related to game objects.
    /// Such as: getting childs of transform, getting components, etc.
    /// </summary>
    public static class GameObjectUtil
    {
        #region Properties

        // cache to prevent garbage generation
        private static readonly Comparison<IInitializable> _initializableComparer = CompareInitilizable;

        #endregion

        #region Transform

        /// <summary>
        /// Returns a list of all initializable immediate-only children.
        /// </summary>
        public static List<IInitializable> GetAllInitializableChildren(Transform transform)
        {
            var result = new List<IInitializable>(1); // initialized to 1 because in most cases there will be none child
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var initializable = transform.GetChild(i).GetComponent<IInitializable>();
                if (initializable != null)
                    result.Add(initializable);
            }
            return result;
        }

        /// <summary>
        /// Initializes the children of this transform through the IInitializable interface.
        /// This will returned the ordered list of children.
        /// </summary>
        public static List<IInitializable> InitializeAllChildren(Transform transform)
        {
            var children = GetAllInitializableChildren(transform);
            children.Sort(_initializableComparer);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Initialize();
            }
            return children;
        }

        /// <summary>
        /// Returns the default comparison result of a and b.
        /// If a has a order greater than b's order, the returned value is positive,
        /// if a has a order smaller than b's order, the returned value is negative,
        /// if a and b has equal orders, the returned value is zero.
        /// </summary>
        private static int CompareInitilizable(IInitializable a, IInitializable b)
        {
            return a.GetOrder() - b.GetOrder();
        }

        #endregion
    }
}