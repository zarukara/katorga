namespace CoreSystem
{
    public class GameStateMachine
    {
        private AGameState _currentState;

        public AGameState CurrentState => _currentState;

        public void Initialize(AGameState initialState)
        {
            _currentState = initialState;
            _currentState.Enter();
        }

        public void ChangeState(AGameState newState)
        {
            if (newState == null || newState == _currentState)
                return;

            _currentState?.Exit();

            _currentState = newState;
            _currentState.Enter();
        }
    }
}