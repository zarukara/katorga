using System.Collections.Generic;
using CoinSystem;
using UnityEngine;
using Zenject;

namespace PoolSystem
{
    public class CoinPool
    {
        private readonly DiContainer _container;
        private readonly Coin _prefab;
        private readonly Transform _parent;

        private readonly Queue<Coin> _availableCoins = new();
        private readonly HashSet<Coin> _activeCoins = new();

        public CoinPool(
            DiContainer container,
            Coin prefab,
            Transform parent,
            int initialSize)
        {
            _container = container;
            _prefab = prefab;
            _parent = parent;

            CreateInitialPool(initialSize);
        }

        public Coin Get(Vector3 position, Transform target)
        {
            Coin coin = _availableCoins.Count > 0
                ? _availableCoins.Dequeue()
                : CreateCoin();

            coin.Activate(position, target);

            _activeCoins.Add(coin);

            return coin;
        }

        public void Release(Coin coin)
        {
            if (coin == null)
                return;

            if (!_activeCoins.Remove(coin))
                return;

            coin.gameObject.SetActive(false);
            _availableCoins.Enqueue(coin);
        }

        public void ReleaseAll()
        {
            Coin[] coins = new Coin[_activeCoins.Count];

            _activeCoins.CopyTo(coins);

            foreach (Coin coin in coins)
            {
                Release(coin);
            }
        }

        private void CreateInitialPool(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                Coin coin = CreateCoin();

                coin.gameObject.SetActive(false);
                _availableCoins.Enqueue(coin);
            }
        }

        private Coin CreateCoin()
        {
            return _container.InstantiatePrefabForComponent<Coin>(
                _prefab,
                _parent
            );
        }
    }
}