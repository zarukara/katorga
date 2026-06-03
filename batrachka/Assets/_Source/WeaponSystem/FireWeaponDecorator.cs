using EnemySystem;
using UnityEngine;

namespace WeaponSystem
{
    public class FireWeaponDecorator : AWeaponDecorator
    {
        private readonly int fireDamage;

        public FireWeaponDecorator(
            AWeapon weapon,
            Camera camera,
            int fireDamage = 1) : base(weapon, camera)
        {
            this.fireDamage = fireDamage;
        }

        public override void Attack()
        {
            base.Attack();

            if (!TryGetEnemyUnderCursor(out Enemy enemy))
            {
                return;
            }

            enemy.TakeDamage(fireDamage);

            Debug.Log("FireWeaponDecorator applied fire damage: " + fireDamage);
        }
    }
}