using EnemySystem;
using UnityEngine;

namespace WeaponSystem
{
    public class ShockWeaponDecorator : AWeaponDecorator
    {
        private readonly int shockDamage;

        public ShockWeaponDecorator(
            AWeapon weapon,
            Camera camera,
            int shockDamage = 1) : base(weapon, camera)
        {
            this.shockDamage = shockDamage;
        }

        public override void Attack()
        {
            base.Attack();

            if (!TryGetEnemyUnderCursor(out Enemy enemy))
            {
                return;
            }

            enemy.TakeDamage(shockDamage);

            Debug.Log("ShockWeaponDecorator applied shock damage: " + shockDamage);
        }
    }
}