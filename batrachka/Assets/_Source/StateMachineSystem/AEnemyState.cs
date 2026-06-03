using EnemySystem;

namespace StateMachineSystem
{
    public abstract class AEnemyState
    {
        protected readonly Enemy enemy;

        protected AEnemyState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}