using System;

namespace InputSystem
{
    public interface IPlayerInput
    {
        event Action JumpPressed;
        bool IsJumpPressed { get; }
    }
}
