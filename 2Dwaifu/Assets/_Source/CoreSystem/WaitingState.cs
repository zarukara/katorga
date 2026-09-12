using PlayerSystem;

namespace CoreSystem
{
    public class WaitingState : AGameState
    {
        private readonly PlayerMovement _playerMovement;

        public WaitingState(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        public override void Enter()
        {
            _playerMovement.DisableMovement();
        }

        public override void Exit()
        {
        }
    }
}