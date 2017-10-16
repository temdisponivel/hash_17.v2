using HASH;
using UnityEngine;

namespace Assets._Code.__TRASH
{
    public class TestInitializable : MonoBehaviour
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
            
            gameObject.SetActive(true);
        }
    }
}