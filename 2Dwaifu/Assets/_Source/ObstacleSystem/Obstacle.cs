using System;
using UnityEngine;

namespace ObstacleSystem
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float despawnX = -12f;

        public event Action<Obstacle> DespawnRequested;

        private bool _despawnRequested;

        private void OnEnable()
        {
            _despawnRequested = false;
        }

        private void Update()
        {
            Move();
            CheckDespawn();
        }

        private void Move()
        {
            transform.position +=
                Vector3.left * (moveSpeed * Time.deltaTime);
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