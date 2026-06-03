using UnityEngine;

namespace StateMachineSystem
{
    public class StateMachine
    {
        private AEnemyState currentState;

        public AEnemyState CurrentState => currentState;

        public void Initialize(AEnemyState startState)
        {
            currentState = startState;
            currentState.Enter();
        }

        public void ChangeState(AEnemyState newState)
        {
            if (newState == null)
            {
                return;
            }

            if (currentState == newState)
            {
                return;
            }

            currentState?.Exit();

            currentState = newState;

            Debug.Log("StateMachine changed state to: " + currentState.GetType().Name);

            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}