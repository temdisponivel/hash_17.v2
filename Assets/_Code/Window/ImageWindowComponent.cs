using UnityEngine;

namespace HASH
{
    public class ImageWindowComponent : MonoBehaviour
    {
        public UIWidget MainWidget;
        public UITexture ImageHolder;

        public bool UpdateImageBlendFactor;
        
        [Range(0, 1)]
        public float EncryptedImageBlendFactor;
    }
}