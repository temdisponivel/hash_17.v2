using System.Net;
using UnityEngine;

namespace HASH
{
    public class DreamOnePaperMessage : MonoBehaviour
    {
        public AudioSource Sound;
        public float InteractionRadius;

        public void Initialize()
        {
            Sound.Play();
            LoopUtil.AddUpdate(OnUpdate);
        }

        public void Finish()
        {
            Sound.Stop();
            LoopUtil.RemoveUpdate(OnUpdate);
        }

        public void OnUpdate()
        {
            if (Input.GetButtonDown(Constants.InputNames.InteractButton))
            {
                var player = DreamOneController.CurrentState.References.PlayerController;
                var camera = player.CameraTrans;

                var dir = (transform.position - camera.position).normalized;
                var dot = Vector3.Dot(dir, camera.forward);
                if (dot > .9f)
                {
                    if (GameObjectUtil.IsCloserThan(player.transform, transform, InteractionRadius))
                        DreamOneController.InteractedWithPaperMessage();                    
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, InteractionRadius);
        }
#endif
    }
}