namespace PlayerSystem
{
    public class ShootCommand : ICommand
    {
        private IWeapon weapon;

        public ShootCommand(IWeapon weapon)
        {
            this.weapon = weapon;
        }

        public void Execute()
        {
            weapon.Attack();
        }
    }
}