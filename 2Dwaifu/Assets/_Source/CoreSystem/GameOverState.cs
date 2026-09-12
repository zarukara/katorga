using CoinSystem;
using ObstacleSystem;
using PlayerSystem;
using ScoreSystem;

namespace CoreSystem
{
    public class GameOverState : AGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerTrail _playerTrail;
        private readonly PlayerRespawn _playerRespawn;
        private readonly PlayerCollisionHandler _collisionHandler;
        private readonly ObstacleSpawner _obstacleSpawner;
        private readonly CoinSpawner _coinSpawner;
        private readonly ScoreModel _scoreModel;

        public GameOverState(
            PlayerMovement playerMovement,
            PlayerTrail playerTrail,
            PlayerRespawn playerRespawn,
            PlayerCollisionHandler collisionHandler,
            ObstacleSpawner obstacleSpawner,
            CoinSpawner coinSpawner,
            ScoreModel scoreModel)
        {
            _playerMovement = playerMovement;
            _playerTrail = playerTrail;
            _playerRespawn = playerRespawn;
            _collisionHandler = collisionHandler;
            _obstacleSpawner = obstacleSpawner;
            _coinSpawner = coinSpawner;
            _scoreModel = scoreModel;
        }

        public override void Enter()
        {
            _playerMovement.DisableMovement();
            _playerTrail.DisableTrail();

            _obstacleSpawner.StopSpawning();
            _obstacleSpawner.ClearObstacles();

            _coinSpawner.StopSpawning();
            _coinSpawner.ClearCoins();

            _scoreModel.Reset();

            _playerRespawn.ResetPosition();
            _collisionHandler.ResetState();
        }

        public override void Exit()
        {
        }
    }
}