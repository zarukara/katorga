namespace PlayerSystem
{
    public class ShootCommand : ICommand
    {
        private PlayerCore pcore;

        public ShootCommand(PlayerCore movement)
        {
            this.pcore = movement;
        }

        public void Execute()
        {
            pcore.Shoot();
        }
    }
}