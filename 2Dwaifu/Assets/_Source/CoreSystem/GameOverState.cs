using ObstacleSystem;
using PlayerSystem;

namespace CoreSystem
{
    public class GameOverState : AGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerRespawn _playerRespawn;
        private readonly PlayerCollisionHandler _collisionHandler;
        private readonly ObstacleSpawner _obstacleSpawner;

        public GameOverState(
            PlayerMovement playerMovement,
            PlayerRespawn playerRespawn,
            PlayerCollisionHandler collisionHandler,
            ObstacleSpawner obstacleSpawner)
        {
            _playerMovement = playerMovement;
            _playerRespawn = playerRespawn;
            _collisionHandler = collisionHandler;
            _obstacleSpawner = obstacleSpawner;
        }

        public override void Enter()
        {
            _playerMovement.DisableMovement();

            _obstacleSpawner.StopSpawning();
            _obstacleSpawner.ClearObstacles();

            _playerRespawn.ResetPosition();
            _collisionHandler.ResetState();
        }

        public override void Exit()
        {
        }
    }
}