using UnityEngine;
using UnityEngine.AI;

namespace HASH
{
    public class DreamOnePlayerController : MonoBehaviour
    {
        public Transform CameraTrans;
        public Transform LockTarget;

        public float LockStrengh;

        public float MoveVelocity;
        public float TurnVelocity;

        public void Initiate()
        {
            LoopUtil.AddUpdate(OnUpdate);
        }

        public void StartLookingAtPhone()
        {
            LoopUtil.AddLateUpdate(OnLateUpdate);
        }

        public void Finish()
        {
            LoopUtil.RemoveUpdate(OnUpdate);
            LoopUtil.RemoveLateUpdate(OnLateUpdate);
        }

        void OnUpdate()
        {
            var horizontal = Input.GetAxis(Constants.InputNames.HorizontalAxis);
            var vertical = Input.GetAxis(Constants.InputNames.VerticalAxis);

            var horizontalDelta = horizontal * MoveVelocity * Time.deltaTime;
            var verticalDelta = vertical * MoveVelocity * Time.deltaTime;

            Vector3 desiredPos = transform.position;

            desiredPos = desiredPos + (transform.forward * verticalDelta);
            desiredPos = desiredPos + (transform.right * horizontalDelta);

            var turnHorizontalDelta = Input.GetAxisRaw(Constants.InputNames.MouseHorizontalAxis) * TurnVelocity * Time.deltaTime;
            var turnVerticalDelta = -(Input.GetAxisRaw(Constants.InputNames.MouseVerticalAxis) * TurnVelocity * Time.deltaTime);

            transform.Rotate(Vector3.up, turnHorizontalDelta);
            CameraTrans.Rotate(Vector3.right, turnVerticalDelta);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredPos, out hit, 1f, NavMesh.AllAreas))
                transform.position = hit.position;
        }

        void OnLateUpdate()
        {
            var telephone = DreamOneController.CurrentState.References.Telephone;
            LockRotation(telephone.transform, transform);
            LockRotation(telephone.Model, CameraTrans);
        }

        private void LockRotation(Transform a, Transform b)
        {
            var dir = (a.position - b.position).normalized;
            var newForward = Vector3.LerpUnclamped(b.forward, dir, LockStrengh * Time.deltaTime);
            b.forward = newForward;
        }
    }
}