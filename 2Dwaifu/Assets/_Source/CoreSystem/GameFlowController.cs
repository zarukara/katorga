using InputSystem;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public sealed class GameFlowController : MonoBehaviour
    {
        private IPlayerInput _input;
        private PlayerCollisionHandler _collisions;
        private GameStateMachine _stateMachine;
        private WaitingState _waitingState;
        private PlayingState _playingState;
        private ResettingState _resettingState;

        [Inject]
        public void Construct(
            IPlayerInput input,
            PlayerCollisionHandler collisions,
            GameStateMachine stateMachine,
            WaitingState waitingState,
            PlayingState playingState,
            ResettingState resettingState)
        {
            _input = input;
            _collisions = collisions;
            _stateMachine = stateMachine;
            _waitingState = waitingState;
            _playingState = playingState;
            _resettingState = resettingState;

            // The controller and its event sources share the scene lifetime.
            _input.JumpPressed += OnJumpPressed;
            _collisions.Died += OnPlayerDied;
        }

        private void Start()
        {
            _collisions.ResetDeathState();
            _stateMachine.Initialize(_waitingState);
        }

        private void OnDestroy()
        {
            if (_input != null)
                _input.JumpPressed -= OnJumpPressed;
            if (_collisions != null)
                _collisions.Died -= OnPlayerDied;
        }

        private void OnJumpPressed()
        {
            if (_stateMachine.CurrentState == _waitingState)
                _stateMachine.ChangeState(_playingState);
        }

        private void OnPlayerDied()
        {
            if (_stateMachine.CurrentState != _playingState)
                return;

            _stateMachine.ChangeState(_resettingState);
            _stateMachine.ChangeState(_waitingState);
        }
    }
}
