using UnityEngine;

namespace PlayerSystem
{
    public class MoveCommand : ICommand
    {
        private PlayerCore movement;
        private Vector2 input;

        public MoveCommand(PlayerCore movement, Vector2 input)
        {
            this.movement = movement;
            this.input = input;
        }

        public void Execute()
        {
            movement.Move(input);
        }
    }
}