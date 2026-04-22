namespace CoreSystem
{
    public class UIStateMachine
    {
        private IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }
    }
}