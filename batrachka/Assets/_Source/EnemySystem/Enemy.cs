using UnityEngine;

namespace EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        public Transform player;
        public float speed = 3f;
        public int health = 3;

        public IState currentState;

        private void Start()
        {
            SetState(new IdleState(this));
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void SetState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void MoveToPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * (speed * Time.deltaTime);
        }

        public float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, player.position);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            EventManager.EnemyKilled();
            Destroy(gameObject);
        }
    }
}