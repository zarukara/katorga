using PoolSystem;
using UnityEngine;
using Zenject;

namespace CoinSystem
{
    public sealed class CoinSpawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private float spawnX = 10f;
        [SerializeField] private float minSpawnY = -0.75f;
        [SerializeField] private float maxSpawnY = 0.75f;
        [SerializeField, Min(0.01f)] private float minSpawnInterval = 1.5f;
        [SerializeField, Min(0.01f)] private float maxSpawnInterval = 3f;

        private Transform _playerTransform;
        private CoinPool _coinPool;
        private float _spawnTimer;
        private bool _isSpawning;

        [Inject]
        public void Construct(Transform playerTransform, CoinPool coinPool)
        {
            _playerTransform = playerTransform;
            _coinPool = coinPool;
        }

        private void Update()
        {
            if (!_isSpawning)
                return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnCoin();
                ResetTimer();
            }
        }

        private void OnValidate()
        {
            minSpawnInterval = Mathf.Max(0.01f, minSpawnInterval);
            maxSpawnInterval = Mathf.Max(minSpawnInterval, maxSpawnInterval);
            maxSpawnY = Mathf.Max(minSpawnY, maxSpawnY);
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            ResetTimer();
        }

        public void StopSpawning() => _isSpawning = false;

        public void ClearCoins() => _coinPool.ReleaseAll();

        private void SpawnCoin()
        {
            float spawnY = Random.Range(minSpawnY, maxSpawnY);
            _coinPool.Get(new Vector3(spawnX, spawnY, 0f), _playerTransform);
        }

        private void ResetTimer()
        {
            _spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
