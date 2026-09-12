using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObstacleSystem
{
    public sealed class Obstacle : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 4f;
        [FormerlySerializedAs("destroyX")]
        [SerializeField] private float despawnX = -12f;

        public event Action<Obstacle> DespawnRequested;

        private bool _despawnRequested;

        public void Activate(Vector3 position)
        {
            transform.position = position;
            _despawnRequested = false;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _despawnRequested = true;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            Move();
            CheckDespawn();
        }

        private void Move()
        {
            // Preserve the existing frame-based trajectory and contact timing.
            transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
        }

        private void CheckDespawn()
        {
            if (_despawnRequested)
                return;

            if (transform.position.x > despawnX)
                return;

            _despawnRequested = true;
            DespawnRequested?.Invoke(this);
        }
    }
}
