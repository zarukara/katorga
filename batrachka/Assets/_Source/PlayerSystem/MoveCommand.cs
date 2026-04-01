using UnityEngine;

namespace PlayerSystem
{
    public class MoveCommand : ICommand
    {
        private PlayerCore movement;
        private Vector2 input;

        public MoveCommand(PlayerCore core, Vector2 input)
        {
            movement = core;
            this.input = input;
        }

        public void Execute()
        {
            movement.Move(input);
        }
    }
}