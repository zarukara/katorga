using CoinSystem;
using ObstacleSystem;
using PlayerSystem;

namespace CoreSystem
{
    public sealed class PlayingState : IGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerTrail _playerTrail;
        private readonly ObstacleSpawner _obstacleSpawner;
        private readonly CoinSpawner _coinSpawner;

        public PlayingState(
            PlayerMovement playerMovement,
            PlayerTrail playerTrail,
            ObstacleSpawner obstacleSpawner,
            CoinSpawner coinSpawner)
        {
            _playerMovement = playerMovement;
            _playerTrail = playerTrail;
            _obstacleSpawner = obstacleSpawner;
            _coinSpawner = coinSpawner;
        }

        public void Enter()
        {
            _playerMovement.EnableMovement();
            _playerTrail.EnableTrail();

            _obstacleSpawner.StartSpawning();
            _coinSpawner.StartSpawning();
        }

        public void Exit()
        {
            _obstacleSpawner.StopSpawning();
            _coinSpawner.StopSpawning();
        }
    }
}
