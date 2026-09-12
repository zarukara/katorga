using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(TrailRenderer))]
    public class PlayerTrail : MonoBehaviour
    {
        [SerializeField] private float trailMoveSpeed = 4f;

        private TrailRenderer _trailRenderer;
        private bool _isActive;

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            DisableTrail();
        }

        private void LateUpdate()
        {
            if (!_isActive)
                return;

            MoveTrailBackwards();
        }

        public void EnableTrail()
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;

            _isActive = true;
        }

        public void DisableTrail()
        {
            _isActive = false;

            _trailRenderer.emitting = false;
            _trailRenderer.Clear();
        }

        private void MoveTrailBackwards()
        {
            int positionCount = _trailRenderer.positionCount;

            if (positionCount <= 1)
                return;

            float moveDistance =
                trailMoveSpeed * Time.deltaTime;

            for (int i = 0; i < positionCount - 1; i++)
            {
                Vector3 position =
                    _trailRenderer.GetPosition(i);

                position.x -= moveDistance;

                _trailRenderer.SetPosition(
                    i,
                    position
                );
            }

            _trailRenderer.SetPosition(
                positionCount - 1,
                transform.position
            );
        }
    }
}