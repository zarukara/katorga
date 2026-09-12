using System;
using CoinSystem;
using Zenject;

namespace ScoreSystem
{
    public sealed class CoinScoreHandler : IInitializable, IDisposable
    {
        private readonly ICoinCollectionSource _coins;
        private readonly ScoreModel _score;
        private bool _isSubscribed;

        public CoinScoreHandler(ICoinCollectionSource coins, ScoreModel score)
        {
            _coins = coins ?? throw new ArgumentNullException(nameof(coins));
            _score = score ?? throw new ArgumentNullException(nameof(score));
        }

        public void Initialize()
        {
            if (_isSubscribed)
                return;

            _coins.CoinCollected += OnCoinCollected;
            _isSubscribed = true;
        }

        public void Dispose()
        {
            if (!_isSubscribed)
                return;

            _coins.CoinCollected -= OnCoinCollected;
            _isSubscribed = false;
        }

        private void OnCoinCollected() => _score.AddPoint();
    }
}
