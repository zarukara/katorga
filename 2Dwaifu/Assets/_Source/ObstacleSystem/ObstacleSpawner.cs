using System.Collections.Generic;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace ObstacleSystem
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private Obstacle obstaclePrefab;

        [Header("Spawn")]
        [SerializeField] private float spawnX = 10f;
        [SerializeField] private float minSpawnInterval = 2.5f;
        [SerializeField] private float maxSpawnInterval = 4.5f;

        [Header("Upper Zone")]
        [SerializeField] private float upperMinY = 2f;
        [SerializeField] private float upperMaxY = 3f;

        [Header("Lower Zone")]
        [SerializeField] private float lowerMinY = -3f;
        [SerializeField] private float lowerMaxY = -2f;

        private readonly List<Obstacle> _spawnedObstacles = new();

        private PlayerMovement _playerMovement;

        private float _spawnTimer;
        private bool _isSpawning;

        [Inject]
        public void Construct(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        private void Update()
        {
            if (!_isSpawning)
                return;

            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0f)
            {
                SpawnObstacle();
                ResetTimer();
            }
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            ResetTimer();
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        public void ClearObstacles()
        {
            foreach (Obstacle obstacle in _spawnedObstacles)
            {
                if (obstacle != null)
                    Destroy(obstacle.gameObject);
            }

            _spawnedObstacles.Clear();
        }

        private void SpawnObstacle()
        {
            float upperCenterY = (upperMinY + upperMaxY) / 2f;
            float lowerCenterY = (lowerMinY + lowerMaxY) / 2f;

            float playerY = _playerMovement.transform.position.y;

            float distanceToUpper = Mathf.Abs(
                playerY - upperCenterY
            );

            float distanceToLower = Mathf.Abs(
                playerY - lowerCenterY
            );

            float spawnY;

            if (distanceToUpper < distanceToLower)
            {
                spawnY = Random.Range(
                    upperMinY,
                    upperMaxY
                );
            }
            else
            {
                spawnY = Random.Range(
                    lowerMinY,
                    lowerMaxY
                );
            }

            Vector3 spawnPosition = new Vector3(
                spawnX,
                spawnY,
                0f
            );

            Obstacle obstacle = Instantiate(
                obstaclePrefab,
                spawnPosition,
                Quaternion.identity
            );

            _spawnedObstacles.Add(obstacle);
        }

        private void ResetTimer()
        {
            _spawnTimer = Random.Range(
                minSpawnInterval,
                maxSpawnInterval
            );
        }
    }
}