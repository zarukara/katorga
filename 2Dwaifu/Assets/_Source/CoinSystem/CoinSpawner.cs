using System.Collections.Generic;
using PlayerSystem;
using PoolSystem;
using ScoreSystem;
using UnityEngine;
using Zenject;

namespace CoinSystem
{
    public class CoinSpawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private float spawnX = 10f;
        [SerializeField] private float minSpawnY = -0.75f;
        [SerializeField] private float maxSpawnY = 0.75f;
        [SerializeField] private float minSpawnInterval = 1.5f;
        [SerializeField] private float maxSpawnInterval = 3f;

        private readonly HashSet<Coin> _spawnedCoins = new();

        private PlayerMovement _playerMovement;
        private CoinPool _coinPool;
        private ScoreModel _scoreModel;

        private float _spawnTimer;
        private bool _isSpawning;

        [Inject]
        public void Construct(
            PlayerMovement playerMovement,
            CoinPool coinPool,
            ScoreModel scoreModel)
        {
            _playerMovement = playerMovement;
            _coinPool = coinPool;
            _scoreModel = scoreModel;
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

        public void StartSpawning()
        {
            _isSpawning = true;
            ResetTimer();
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        public void ClearCoins()
        {
            Coin[] coins = new Coin[_spawnedCoins.Count];

            _spawnedCoins.CopyTo(coins);

            foreach (Coin coin in coins)
            {
                ReleaseCoin(coin);
            }
        }

        private void SpawnCoin()
        {
            float spawnY = Random.Range(
                minSpawnY,
                maxSpawnY
            );

            Vector3 spawnPosition = new Vector3(
                spawnX,
                spawnY,
                0f
            );

            Coin coin = _coinPool.Get(
                spawnPosition,
                _playerMovement.transform
            );

            coin.Collected += OnCoinCollected;
            coin.DespawnRequested += OnDespawnRequested;

            _spawnedCoins.Add(coin);
        }

        private void OnCoinCollected(Coin coin)
        {
            _scoreModel.AddPoint();
            ReleaseCoin(coin);
        }

        private void OnDespawnRequested(Coin coin)
        {
            ReleaseCoin(coin);
        }

        private void ReleaseCoin(Coin coin)
        {
            if (!_spawnedCoins.Remove(coin))
                return;

            coin.Collected -= OnCoinCollected;
            coin.DespawnRequested -= OnDespawnRequested;

            _coinPool.Release(coin);
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