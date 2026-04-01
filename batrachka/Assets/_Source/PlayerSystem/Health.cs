using UnityEngine;

namespace PlayerSystem
{
    public class Health : MonoBehaviour
    {
        public int health = 10;

        public void TakeDamage(int damage)
        {
            health -= damage;

            EventManager.PlayerDamaged(damage);

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("GG");
        }
    }
}