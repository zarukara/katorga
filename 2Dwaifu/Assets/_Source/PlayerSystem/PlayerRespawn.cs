using UnityEngine;

namespace PlayerSystem
{
    public class PlayerRespawn : MonoBehaviour
    {
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
        }
    }
}