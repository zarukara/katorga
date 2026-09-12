using InputSystem;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float downwardGravity = 2f;
        [SerializeField] private float upwardGravity = 2f;
        [SerializeField] private float maxVerticalSpeed = 5f;

        private PlayerInputReader _inputReader;
        private Rigidbody2D _rigidbody;

        private bool _isMovementEnabled;

        [Inject]
        public void Construct(PlayerInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_isMovementEnabled)
                return;

            UpdateGravity();
            ClampVerticalSpeed();
        }

        public void EnableMovement()
        {
            _isMovementEnabled = true;
            _rigidbody.simulated = true;
        }

        public void DisableMovement()
        {
            _isMovementEnabled = false;

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
        }

        private void UpdateGravity()
        {
            _rigidbody.gravityScale = _inputReader.IsJumpPressed
                ? -upwardGravity
                : downwardGravity;
        }

        private void ClampVerticalSpeed()
        {
            float clampedY = Mathf.Clamp(
                _rigidbody.velocity.y,
                -maxVerticalSpeed,
                maxVerticalSpeed
            );

            _rigidbody.velocity = new Vector2(
                _rigidbody.velocity.x,
                clampedY
            );
        }
    }
}