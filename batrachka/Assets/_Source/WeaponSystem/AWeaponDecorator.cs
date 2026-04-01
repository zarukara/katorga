namespace WeaponSystem
{
    public abstract class AWeaponDecorator : IWeapon
    {
        protected IWeapon weapon;

        public AWeaponDecorator(IWeapon weapon)
        {
            this.weapon = weapon;
        }

        public virtual void Attack()
        {
            weapon.Attack();
        }
    }
}