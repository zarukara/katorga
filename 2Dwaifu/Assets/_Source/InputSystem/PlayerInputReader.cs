using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private InputActionReference jumpAction;

        public event Action InputPressed;

        public bool IsJumpPressed => jumpAction.action.IsPressed();

        private void OnEnable()
        {
            jumpAction.action.started += OnInputStarted;
            jumpAction.action.Enable();
        }

        private void OnDisable()
        {
            jumpAction.action.started -= OnInputStarted;
            jumpAction.action.Disable();
        }

        private void OnInputStarted(InputAction.CallbackContext context)
        {
            InputPressed?.Invoke();
        }
    }
}