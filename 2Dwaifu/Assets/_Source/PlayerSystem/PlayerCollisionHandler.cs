using System;
using UnityEngine;

namespace PlayerSystem
{
    public sealed class PlayerCollisionHandler : MonoBehaviour
    {
        public event Action Died;

        private bool _isDead;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isDead)
                return;

            _isDead = true;
            Died?.Invoke();
        }

        public void ResetDeathState()
        {
            _isDead = false;
        }
    }
}
