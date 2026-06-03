using UnityEngine;

namespace StateMachineSystem
{
    public class IdleState : AEnemyState
    {
        public IdleState(EnemySystem.Enemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enemy entered IdleState");
        }

        public override void Update()
        {
            if (enemy.DistanceToPlayer() < enemy.ChaseDistance)
            {
                enemy.ChangeState(enemy.ChaseState);
            }
        }

        public override void Exit()
        {
            Debug.Log("Enemy exited IdleState");
        }
    }
}