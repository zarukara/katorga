using System.Collections.Generic;
using PlayerSystem;
using PoolSystem;
using UnityEngine;
using Zenject;

namespace ObstacleSystem
{
    public class ObstacleSpawner : MonoBehaviour
    {
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

        private readonly HashSet<Obstacle> _spawnedObstacles = new();

        private PlayerMovement _playerMovement;
        private ObstaclePool _obstaclePool;

        private float _spawnTimer;
        private bool _isSpawning;

        [Inject]
        public void Construct(
            PlayerMovement playerMovement,
            ObstaclePool obstaclePool)
        {
            _playerMovement = playerMovement;
            _obstaclePool = obstaclePool;
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
            Obstacle[] obstacles =
                new Obstacle[_spawnedObstacles.Count];

            _spawnedObstacles.CopyTo(obstacles);

            foreach (Obstacle obstacle in obstacles)
            {
                ReleaseObstacle(obstacle);
            }
        }

        private void SpawnObstacle()
        {
            float upperCenterY =
                (upperMinY + upperMaxY) / 2f;

            float lowerCenterY =
                (lowerMinY + lowerMaxY) / 2f;

            float playerY =
                _playerMovement.transform.position.y;

            float distanceToUpper =
                Mathf.Abs(playerY - upperCenterY);

            float distanceToLower =
                Mathf.Abs(playerY - lowerCenterY);

            float spawnY = distanceToUpper < distanceToLower
                ? Random.Range(upperMinY, upperMaxY)
                : Random.Range(lowerMinY, lowerMaxY);

            Vector3 spawnPosition = new Vector3(
                spawnX,
                spawnY,
                0f
            );

            Obstacle obstacle =
                _obstaclePool.Get(spawnPosition);

            obstacle.DespawnRequested += OnDespawnRequested;

            _spawnedObstacles.Add(obstacle);
        }

        private void OnDespawnRequested(Obstacle obstacle)
        {
            ReleaseObstacle(obstacle);
        }

        private void ReleaseObstacle(Obstacle obstacle)
        {
            if (!_spawnedObstacles.Remove(obstacle))
                return;

            obstacle.DespawnRequested -= OnDespawnRequested;

            _obstaclePool.Release(obstacle);
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