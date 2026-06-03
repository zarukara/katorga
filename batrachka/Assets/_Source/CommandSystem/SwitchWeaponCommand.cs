using PlayerSystem;
using UnityEngine;
using WeaponSystem;

namespace CommandSystem
{
    public class SwitchWeaponCommand : ICommand
    {
        private readonly PlayerCore playerCore;
        private readonly WeaponType newWeaponType;

        private WeaponType previousWeaponType;

        public SwitchWeaponCommand(
            PlayerCore playerCore,
            WeaponType newWeaponType)
        {
            this.playerCore = playerCore;
            this.newWeaponType = newWeaponType;
        }

        public void Execute()
        {
            if (playerCore == null)
            {
                return;
            }

            previousWeaponType = playerCore.CurrentWeaponType;

            playerCore.SelectWeapon(newWeaponType);

            Debug.Log(
                "SwitchWeaponCommand executed. " +
                "Previous: " + previousWeaponType +
                ", New: " + newWeaponType);
        }

        public void Undo()
        {
            if (playerCore == null)
            {
                return;
            }

            playerCore.SelectWeapon(previousWeaponType);

            Debug.Log(
                "SwitchWeaponCommand undone. " +
                "Returned to: " + previousWeaponType);
        }
    }
}