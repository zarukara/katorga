using System;
using CoinSystem;
using UnityEngine;

namespace PoolSystem
{
    public sealed class CoinPool : ICoinCollectionSource
    {
        private readonly Func<Coin> _createCoin;
        private readonly ObjectPool<Coin> _pool;

        public event Action CoinCollected;

        public CoinPool(Func<Coin> createCoin, int initialSize)
        {
            _createCoin = createCoin ?? throw new ArgumentNullException(nameof(createCoin));
            _pool = new ObjectPool<Coin>(CreateCoin, coin => coin.Deactivate(), initialSize);
        }

        public Coin Get(Vector3 position, Transform target)
        {
            Coin coin = _pool.Get();
            coin.Activate(position, target);
            return coin;
        }

        public bool Release(Coin coin) => _pool.Release(coin);

        public void ReleaseAll() => _pool.ReleaseAll();

        private Coin CreateCoin()
        {
            Coin coin = _createCoin();
            if (coin == null)
                throw new InvalidOperationException("The coin factory returned no coin.");

            // Subscriptions belong to the pool and are installed once per instance.
            coin.Collected += OnCoinCollected;
            coin.DespawnRequested += OnDespawnRequested;
            return coin;
        }

        private void OnCoinCollected(Coin coin)
        {
            if (_pool.Release(coin))
                CoinCollected?.Invoke();
        }

        private void OnDespawnRequested(Coin coin) => _pool.Release(coin);
    }
}
