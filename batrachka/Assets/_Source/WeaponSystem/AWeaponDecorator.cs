using UnityEngine;

namespace WeaponSystem
{
    public abstract class AWeaponDecorator : AWeapon
    {
        protected readonly AWeapon weapon;

        protected AWeaponDecorator(AWeapon weapon, Camera camera) : base(camera)
        {
            this.weapon = weapon;
        }

        public override void Attack()
        {
            weapon.Attack();
        }
    }
}