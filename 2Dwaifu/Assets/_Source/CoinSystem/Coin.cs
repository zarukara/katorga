using System;
using UnityEngine;

namespace CoinSystem
{
    public sealed class Coin : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 4f;
        [SerializeField, Min(0f)] private float magnetRadius = 2.5f;
        [SerializeField, Min(0f)] private float magnetSpeed = 8f;
        [SerializeField] private float despawnX = -12f;

        public event Action<Coin> Collected;
        public event Action<Coin> DespawnRequested;

        private Transform _target;
        private bool _isFinished;

        public void Activate(Vector3 position, Transform target)
        {
            transform.position = position;

            _target = target;
            _isFinished = false;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isFinished = true;
            _target = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isFinished)
                return;

            Move();
            CheckDespawn();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isFinished)
                return;

            if (other.transform != _target)
                return;

            _isFinished = true;
            Collected?.Invoke(this);
        }

        private void Move()
        {
            if (_target != null)
            {
                float distance = Vector2.Distance(transform.position, _target.position);

                if (distance <= magnetRadius)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        _target.position,
                        magnetSpeed * Time.deltaTime);

                    return;
                }
            }

            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        private void CheckDespawn()
        {
            if (transform.position.x > despawnX)
                return;

            _isFinished = true;
            DespawnRequested?.Invoke(this);
        }
    }
}
