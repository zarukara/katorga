using PoolSystem;
using UnityEngine;
using Zenject;

namespace ObstacleSystem
{
    public sealed class ObstacleSpawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private float spawnX = 10f;
        [SerializeField, Min(0.01f)] private float minSpawnInterval = 2.5f;
        [SerializeField, Min(0.01f)] private float maxSpawnInterval = 4.5f;

        [Header("Upper Zone")]
        [SerializeField] private float upperMinY = 2f;
        [SerializeField] private float upperMaxY = 3f;

        [Header("Lower Zone")]
        [SerializeField] private float lowerMinY = -3f;
        [SerializeField] private float lowerMaxY = -2f;

        private Transform _playerTransform;
        private ObstaclePool _obstaclePool;
        private float _spawnTimer;
        private bool _isSpawning;

        [Inject]
        public void Construct(Transform playerTransform, ObstaclePool obstaclePool)
        {
            _playerTransform = playerTransform;
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

        private void OnValidate()
        {
            minSpawnInterval = Mathf.Max(0.01f, minSpawnInterval);
            maxSpawnInterval = Mathf.Max(minSpawnInterval, maxSpawnInterval);
            upperMaxY = Mathf.Max(upperMinY, upperMaxY);
            lowerMaxY = Mathf.Max(lowerMinY, lowerMaxY);
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            ResetTimer();
        }

        public void StopSpawning() => _isSpawning = false;

        public void ClearObstacles() => _obstaclePool.ReleaseAll();

        private void SpawnObstacle()
        {
            float upperCenterY = (upperMinY + upperMaxY) / 2f;
            float lowerCenterY = (lowerMinY + lowerMaxY) / 2f;
            float playerY = _playerTransform.position.y;
            float distanceToUpper = Mathf.Abs(playerY - upperCenterY);
            float distanceToLower = Mathf.Abs(playerY - lowerCenterY);

            float spawnY = distanceToUpper < distanceToLower
                ? Random.Range(upperMinY, upperMaxY)
                : Random.Range(lowerMinY, lowerMaxY);

            _obstaclePool.Get(new Vector3(spawnX, spawnY, 0f));
        }

        private void ResetTimer()
        {
            _spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
