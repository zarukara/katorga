using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public sealed class PlayerInputReader : MonoBehaviour, IPlayerInput
    {
        [SerializeField] private InputActionReference jumpAction;

        private InputAction _jumpAction;

        public event Action JumpPressed;

        public bool IsJumpPressed => _jumpAction != null && _jumpAction.IsPressed();

        private void OnEnable()
        {
            _jumpAction = jumpAction != null ? jumpAction.action : null;
            if (_jumpAction == null)
                throw new InvalidOperationException("Assign a valid jump action to PlayerInputReader.");

            _jumpAction.started += OnJumpStarted;
            _jumpAction.Enable();
        }

        private void OnDisable()
        {
            if (_jumpAction == null)
                return;

            _jumpAction.started -= OnJumpStarted;
            _jumpAction.Disable();
        }

        private void OnJumpStarted(InputAction.CallbackContext context) => JumpPressed?.Invoke();
    }
}
