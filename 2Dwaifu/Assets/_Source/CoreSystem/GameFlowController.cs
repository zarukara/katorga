using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using UnityEngine;

namespace CoreSystem
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCollisionHandler collisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;
        [SerializeField] private ObstacleSpawner obstacleSpawner;

        private GameStateMachine _stateMachine;

        private WaitingState _waitingState;
        private PlayingState _playingState;
        private GameOverState _gameOverState;

        private void Awake()
        {
            _stateMachine = new GameStateMachine();

            _waitingState = new WaitingState(
                playerMovement
            );

            _playingState = new PlayingState(
                playerMovement,
                obstacleSpawner
            );

            _gameOverState = new GameOverState(
                playerMovement,
                playerRespawn,
                collisionHandler,
                obstacleSpawner
            );
        }

        private void OnEnable()
        {
            inputReader.InputPressed += OnInputPressed;
            collisionHandler.Died += OnPlayerDied;
        }

        private void Start()
        {
            collisionHandler.ResetState();
            _stateMachine.Initialize(_waitingState);
        }

        private void OnDisable()
        {
            inputReader.InputPressed -= OnInputPressed;
            collisionHandler.Died -= OnPlayerDied;
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