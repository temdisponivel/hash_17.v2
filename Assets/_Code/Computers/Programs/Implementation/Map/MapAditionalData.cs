using UnityEngine;

namespace HASH
{
    [CreateAssetMenu(fileName = "MapAditionalData", menuName = "HASH/Programs/Map aditional data")]
    public class MapAditionalData : ScriptableObject
    {
        public Texture2D MapTexture;
        public GameObject MarkerGameObject;

        public Rect MapDimentions;
    }
}