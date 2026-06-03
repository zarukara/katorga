using EnemySystem;
using UnityEngine;

namespace WeaponSystem
{
    public class BaseWeapon : AWeapon
    {
        private readonly int damage;

        public BaseWeapon(Camera camera, int damage = 1) : base(camera)
        {
            this.damage = damage;
        }

        public override void Attack()
        {
            if (!TryGetEnemyUnderCursor(out Enemy enemy))
            {
                Debug.Log("BaseWeapon attack missed");
                return;
            }

            enemy.TakeDamage(damage);

            Debug.Log("BaseWeapon attack. Damage: " + damage);
        }
    }
}