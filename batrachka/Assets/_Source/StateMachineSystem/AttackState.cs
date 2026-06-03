using UnityEngine;

namespace StateMachineSystem
{
    public class AttackState : AEnemyState
    {
        private float nextAttackTime;

        public AttackState(EnemySystem.Enemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enemy entered AttackState");
            nextAttackTime = 0f;
        }

        public override void Update()
        {
            if (enemy.DistanceToPlayer() > enemy.AttackDistance)
            {
                enemy.ChangeState(enemy.ChaseState);
                return;
            }

            if (Time.time < nextAttackTime)
            {
                return;
            }

            enemy.AttackPlayer();

            nextAttackTime = Time.time + enemy.AttackCooldown;
        }

        public override void Exit()
        {
            Debug.Log("Enemy exited AttackState");
        }
    }
}