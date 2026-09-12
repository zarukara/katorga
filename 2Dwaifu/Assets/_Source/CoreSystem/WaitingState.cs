using PlayerSystem;
using UiSystem;

namespace CoreSystem
{
    public sealed class WaitingState : IGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerTrail _playerTrail;
        private readonly StartPromptView _startPromptView;

        public WaitingState(
            PlayerMovement playerMovement,
            PlayerTrail playerTrail,
            StartPromptView startPromptView)
        {
            _playerMovement = playerMovement;
            _playerTrail = playerTrail;
            _startPromptView = startPromptView;
        }

        public void Enter()
        {
            _playerMovement.DisableMovement();
            _playerTrail.DisableTrail();
            _startPromptView.Show();
        }

        public void Exit()
        {
            _startPromptView.Hide();
        }
    }
}
