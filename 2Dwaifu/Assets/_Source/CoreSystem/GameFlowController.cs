using CoinSystem;
using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using ScoreSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public class GameFlowController : MonoBehaviour
    {
        private PlayerInputReader _inputReader;
        private PlayerCollisionHandler _collisionHandler;

        private GameStateMachine _stateMachine;

        private WaitingState _waitingState;
        private PlayingState _playingState;
        private GameOverState _gameOverState;

        [Inject]
        public void Construct(
            PlayerInputReader inputReader,
            PlayerMovement playerMovement,
            PlayerCollisionHandler collisionHandler,
            PlayerRespawn playerRespawn,
            ObstacleSpawner obstacleSpawner,
            CoinSpawner coinSpawner,
            ScoreModel scoreModel)
        {
            _inputReader = inputReader;
            _collisionHandler = collisionHandler;

            _stateMachine = new GameStateMachine();

            _waitingState = new WaitingState(
                playerMovement
            );

            _playingState = new PlayingState(
                playerMovement,
                obstacleSpawner,
                coinSpawner
            );

            _gameOverState = new GameOverState(
                playerMovement,
                playerRespawn,
                collisionHandler,
                obstacleSpawner,
                coinSpawner,
                scoreModel
            );

            _inputReader.InputPressed += OnInputPressed;
            _collisionHandler.Died += OnPlayerDied;
        }

        private void Start()
        {
            _collisionHandler.ResetState();
            _stateMachine.Initialize(_waitingState);
        }

        private void OnDestroy()
        {
            if (_inputReader != null)
                _inputReader.InputPressed -= OnInputPressed;

            if (_collisionHandler != null)
                _collisionHandler.Died -= OnPlayerDied;
        }

        private void OnInputPressed()
        {
            if (_stateMachine.CurrentState == _waitingState)
            {
                _stateMachine.ChangeState(_playingState);
            }
        }

        private void OnPlayerDied()
        {
            if (_stateMachine.CurrentState != _playingState)
                return;

            _stateMachine.ChangeState(_gameOverState);
            _stateMachine.ChangeState(_waitingState);
        }
    }
}