using PlayerSystem;
using UnityEngine;

namespace CommandSystem
{
    public class MoveCommand : ICommand
    {
        private readonly PlayerCore playerCore;
        private readonly Vector3 moveOffset;

        private readonly Vector3 previousPosition;

        public MoveCommand(PlayerCore playerCore, Vector3 moveOffset)
        {
            this.playerCore = playerCore;
            this.moveOffset = moveOffset;

            if (playerCore != null)
            {
                previousPosition = playerCore.transform.position;
            }
        }

        public void Execute()
        {
            if (playerCore == null)
            {
                return;
            }

            CharacterController characterController =
                playerCore.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.Move(moveOffset);
            }
            else
            {
                playerCore.transform.position += moveOffset;
            }

            Debug.Log("MoveCommand executed");
        }

        public void Undo()
        {
            if (playerCore == null)
            {
                return;
            }

            CharacterController characterController =
                playerCore.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false;
            }

            playerCore.transform.position = previousPosition;

            if (characterController != null)
            {
                characterController.enabled = true;
            }

            Debug.Log("MoveCommand undone");
        }
    }
}