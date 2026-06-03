using UnityEngine;

namespace ObserverSystem
{
    public readonly struct DamageEventData
    {
        public readonly GameObject Target;
        public readonly int Damage;
        public readonly int CurrentHealth;
        public readonly int MaxHealth;

        public DamageEventData(
            GameObject target,
            int damage,
            int currentHealth,
            int maxHealth)
        {
            Target = target;
            Damage = damage;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}