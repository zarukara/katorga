using ObstacleSystem;
using PlayerSystem;

namespace CoreSystem
{
    public class PlayingState : AGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly ObstacleSpawner _obstacleSpawner;

        public PlayingState(
            PlayerMovement playerMovement,
            ObstacleSpawner obstacleSpawner)
        {
            _playerMovement = playerMovement;
            _obstacleSpawner = obstacleSpawner;
        }

        public override void Enter()
        {
            _playerMovement.EnableMovement();
            _obstacleSpawner.StartSpawning();
        }

        public override void Exit()
        {
            _obstacleSpawner.StopSpawning();
        }
    }
}