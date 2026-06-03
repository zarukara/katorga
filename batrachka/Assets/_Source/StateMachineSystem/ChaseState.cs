using UnityEngine;

namespace StateMachineSystem
{
    public class ChaseState : AEnemyState
    {
        public ChaseState(EnemySystem.Enemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enemy entered ChaseState");
        }

        public override void Update()
        {
            if (enemy.DistanceToPlayer() > enemy.ChaseDistance)
            {
                enemy.ChangeState(enemy.IdleState);
                return;
            }

            if (enemy.DistanceToPlayer() <= enemy.AttackDistance)
            {
                enemy.ChangeState(enemy.AttackState);
                return;
            }

            enemy.MoveToPlayer();
        }

        public override void Exit()
        {
            Debug.Log("Enemy exited ChaseState");
        }
    }
}