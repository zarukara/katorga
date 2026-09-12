using InputSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [FormerlySerializedAs("downwardGravity")]
        [SerializeField, Min(0f)] private float downwardGravityScale = 2f;
        [FormerlySerializedAs("upwardGravity")]
        [SerializeField, Min(0f)] private float upwardGravityScale = 2f;
        [FormerlySerializedAs("maxVerticalSpeed")]
        [Tooltip("Velocity limit before Unity applies gravity during the physics step.")]
        [SerializeField, Min(0f)] private float prePhysicsSpeedLimit = 5f;

        private IPlayerInput _inputReader;
        private Rigidbody2D _rigidbody;

        private bool _isMovementEnabled;

        [Inject]
        public void Construct(IPlayerInput inputReader)
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
            ClampPrePhysicsSpeed();
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
                ? -upwardGravityScale
                : downwardGravityScale;
        }

        private void ClampPrePhysicsSpeed()
        {
            // Preserve the tuned movement: Unity adds gravity after this clamp.
            float clampedY = Mathf.Clamp(
                _rigidbody.velocity.y,
                -prePhysicsSpeedLimit,
                prePhysicsSpeedLimit);

            _rigidbody.velocity = new Vector2(
                _rigidbody.velocity.x,
                clampedY);
        }
    }
}
