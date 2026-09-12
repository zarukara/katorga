using CoinSystem;
using ObstacleSystem;
using PlayerSystem;

namespace CoreSystem
{
    public class PlayingState : AGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly ObstacleSpawner _obstacleSpawner;
        private readonly CoinSpawner _coinSpawner;

        public PlayingState(
            PlayerMovement playerMovement,
            ObstacleSpawner obstacleSpawner,
            CoinSpawner coinSpawner)
        {
            _playerMovement = playerMovement;
            _obstacleSpawner = obstacleSpawner;
            _coinSpawner = coinSpawner;
        }

        public override void Enter()
        {
            _playerMovement.EnableMovement();

            _obstacleSpawner.StartSpawning();
            _coinSpawner.StartSpawning();
        }

        public override void Exit()
        {
            _obstacleSpawner.StopSpawning();
            _coinSpawner.StopSpawning();
        }
    }
}