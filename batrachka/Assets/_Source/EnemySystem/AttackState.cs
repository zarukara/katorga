using UnityEngine;

namespace EnemySystem
{
    public class AttackState : IState
    {
        private Enemy enemy;
        private float lastAttackTime;
        private float attackCooldown = 1f;

        public AttackState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            Debug.Log("Enemy Attack");
        }

        public void Update()
        {
            if (enemy.DistanceToPlayer() > 2f)
            {
                enemy.SetState(new ChaseState(enemy));
                return;
            }

            if (Time.time >= lastAttackTime)
            {
                enemy.player.GetComponent<PlayerSystem.Health>()?.TakeDamage(1);
                lastAttackTime = Time.time + attackCooldown;
            }
        }

        public void Exit() { }
    }
}