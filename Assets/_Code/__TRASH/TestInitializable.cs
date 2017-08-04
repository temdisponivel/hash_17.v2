using HASH.Game;
using UnityEngine;

namespace Assets._Code.__TRASH
{
    public class TestInitializable : MonoBehaviour, IInitializable
    {
        public int GetOrder()
        {
            return transform.GetSiblingIndex();
        }

        void Awake()
        {
            Debug.LogWarning("AWAKE: " + name, this);
        }
        
        public void Initialize()
        {
            Debug.Log("INITILIZED: " + name, this);
            TrashUtil.InitializeAllChildrenInOrder(transform);
            gameObject.SetActive(true);
        }
    }
}