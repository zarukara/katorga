using UnityEngine;

namespace EnemySystem
{
    public class IdleState : IState
    {
        private Enemy enemy;

        public IdleState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            Debug.Log("Enemy Idle");
        }

        public void Update()
        {
            if (enemy.DistanceToPlayer() < 10f)
            {
                enemy.SetState(new ChaseState(enemy));
            }
        }

        public void Exit() { }
    }
}