using UnityEngine;

namespace PlayerSystem
{
    public sealed class PlayerRespawn : MonoBehaviour
    {
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void ResetToSpawnPosition()
        {
            transform.position = _startPosition;
        }
    }
}
