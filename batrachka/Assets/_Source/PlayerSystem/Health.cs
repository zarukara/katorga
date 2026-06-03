using ObserverSystem;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(HealthSubject))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 10;

        private int currentHealth;
        private bool isDead;

        private HealthSubject healthSubject;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public bool IsDead => isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
            isDead = false;

            healthSubject = GetComponent<HealthSubject>();
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
            {
                return;
            }

            if (damage <= 0)
            {
                return;
            }

            currentHealth -= damage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            healthSubject.NotifyDamage(
                gameObject,
                damage,
                currentHealth,
                maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        private void Die()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;

            Debug.Log(gameObject.name + " died");
        }
    }
}