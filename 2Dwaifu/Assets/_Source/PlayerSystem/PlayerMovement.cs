using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private float jumpForce = 5f;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            inputReader.JumpPressed += Jump;
        }

        private void OnDisable()
        {
            inputReader.JumpPressed -= Jump;
        }

        private void Jump()
        {
            _rigidbody.velocity = new Vector2(
                _rigidbody.velocity.x,
                jumpForce
            );
        }
    }
}