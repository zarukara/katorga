using PlayerSystem;
using StateMachineSystem;
using UnityEngine;

namespace EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        [Header("Target Search")]
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private float targetSearchRadius = 20f;

        [Header("Movement")]
        [SerializeField] private float speed = 3f;
        [SerializeField] private float chaseDistance = 10f;
        [SerializeField] private float attackDistance = 2f;

        [Header("Combat")]
        [SerializeField] private int health = 3;
        [SerializeField] private int damage = 1;
        [SerializeField] private float attackCooldown = 1f;

        private Transform player;
        private StateMachine stateMachine;

        private IdleState idleState;
        private ChaseState chaseState;
        private AttackState attackState;

        public Transform Player => player;
        public float ChaseDistance => chaseDistance;
        public float AttackDistance => attackDistance;
        public float AttackCooldown => attackCooldown;

        public IdleState IdleState => idleState;
        public ChaseState ChaseState => chaseState;
        public AttackState AttackState => attackState;

        private void Awake()
        {
            stateMachine = new StateMachine();

            idleState = new IdleState(this);
            chaseState = new ChaseState(this);
            attackState = new AttackState(this);
        }

        private void Start()
        {
            FindPlayerByLayer();

            stateMachine.Initialize(idleState);
        }

        private void Update()
        {
            if (player == null)
            {
                FindPlayerByLayer();
            }

            stateMachine.Update();
        }

        public void ChangeState(AEnemyState newState)
        {
            stateMachine.ChangeState(newState);
        }

        public void MoveToPlayer()
        {
            if (player == null)
            {
                return;
            }

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.01f)
            {
                return;
            }

            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public float DistanceToPlayer()
        {
            if (player == null)
            {
                return float.MaxValue;
            }

            return Vector3.Distance(transform.position, player.position);
        }

        public void AttackPlayer()
        {
            if (player == null)
            {
                return;
            }

            Health playerHealth = player.GetComponent<Health>();

            if (playerHealth == null)
            {
                playerHealth = player.GetComponentInParent<Health>();
            }

            if (playerHealth == null)
            {
                return;
            }

            playerHealth.TakeDamage(damage);

            Debug.Log("Enemy attacked player");
        }

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;

            Debug.Log("Enemy took damage: " + damageAmount);

            if (health <= 0)
            {
                Die();
            }
        }

        private void FindPlayerByLayer()
        {
            Collider[] colliders = Physics.OverlapSphere(
                transform.position,
                targetSearchRadius,
                playerLayerMask,
                QueryTriggerInteraction.Ignore);

            float nearestDistance = float.MaxValue;
            Transform nearestPlayer = null;

            foreach (Collider collider in colliders)
            {
                Health health = collider.GetComponent<Health>();

                if (health == null)
                {
                    health = collider.GetComponentInParent<Health>();
                }

                if (health == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(
                    transform.position,
                    collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlayer = health.transform;
                }
            }

            player = nearestPlayer;

            if (player != null)
            {
                Debug.Log("Enemy found player by layer: " + player.name);
            }
        }

        private void Die()
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetSearchRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}