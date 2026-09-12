using System;

namespace CoreSystem
{
    public sealed class GameStateMachine
    {
        private bool _isTransitioning;

        public IGameState CurrentState { get; private set; }

        public void Initialize(IGameState initialState)
        {
            if (CurrentState != null)
                throw new InvalidOperationException("The state machine is already initialized.");

            ChangeState(initialState);
        }

        public void ChangeState(IGameState newState)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));
            if (_isTransitioning)
                throw new InvalidOperationException("A state transition is already in progress.");
            if (ReferenceEquals(newState, CurrentState))
                return;

            _isTransitioning = true;
            try
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }
            finally
            {
                _isTransitioning = false;
            }
        }
    }
}
