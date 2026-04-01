using UnityEngine;

namespace PlayerSystem
{
    public class Health : MonoBehaviour
    {
        public int health = 10;

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("Player HP: " + health);

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
        }
    }
}