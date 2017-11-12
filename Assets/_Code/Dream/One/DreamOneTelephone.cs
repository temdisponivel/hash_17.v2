using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HASH
{
    public class DreamOneTelephone : MonoBehaviour
    {
        public float InteractionRadius;
        public float DelayForPhoneRing;
        public AudioSource TelephoneAudioSource;
        public Transform Model;

        public void Initialize()
        {
            LoopUtil.AddUpdate(OnUpdate);

            TelephoneAudioSource.loop = true;
            TelephoneAudioSource.PlayDelayed(DelayForPhoneRing);
            LoopUtil.RunCoroutine(LookAtPhone(DelayForPhoneRing + .3f));
        }

        public void Finish()
        {
            LoopUtil.RemoveUpdate(OnUpdate);

            TelephoneAudioSource.Stop();
        }

        void OnUpdate()
        {
            if (Input.GetButtonDown(Constants.InputNames.InteractButton))
            {
                var player = DreamOneController.CurrentState.References.PlayerController;
                if (GameObjectUtil.IsCloserThan(player.transform, transform, InteractionRadius))
                    DreamOneController.OnInteractWithPhone();
            }
        }

        IEnumerator LookAtPhone(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            var playerController = DreamOneController.CurrentState.References.PlayerController;
            playerController.StartLookingAtPhone();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Model.position, InteractionRadius);
        }
#endif
    }
}