using System;
using UnityEngine;

namespace InputSystem
{
    public class PlayerInputReader : MonoBehaviour
    {
        public event Action JumpPressed;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                JumpPressed?.Invoke();
            }
        }
    }
}