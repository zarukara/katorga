using PlayerSystem;
using UiSystem;

namespace CoreSystem
{
    public class WaitingState : AGameState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerTrail _playerTrail;
        private readonly MantraView _mantraView;

        public WaitingState(
            PlayerMovement playerMovement,
            PlayerTrail playerTrail,
            MantraView mantraView)
        {
            _playerMovement = playerMovement;
            _playerTrail = playerTrail;
            _mantraView = mantraView;
        }

        public override void Enter()
        {
            _playerMovement.DisableMovement();
            _playerTrail.DisableTrail();
            _mantraView.Show();
        }

        public override void Exit()
        {
            _mantraView.Hide();
        }
    }
}