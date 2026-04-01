using UnityEngine;

namespace EnemySystem
{
    public class ChaseState : IState
    {
        private Enemy enemy;

        public ChaseState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            Debug.Log("Enemy Chase");
        }

        public void Update()
        {
            enemy.MoveToPlayer();

            if (enemy.DistanceToPlayer() < 2f)
            {
                enemy.SetState(new AttackState(enemy));
            }
        }

        public void Exit() { }
    }
}