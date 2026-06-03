using UnityEngine;
using WeaponSystem;

namespace CommandSystem
{
    public class ShootCommand : ICommand
    {
        private readonly AWeapon weapon;

        public ShootCommand(AWeapon weapon)
        {
            this.weapon = weapon;
        }

        public void Execute()
        {
            if (weapon == null)
            {
                return;
            }

            weapon.Attack();

            Debug.Log("ShootCommand executed");
        }

        public void Undo()
        {
            Debug.Log("ShootCommand undo requested. Shot cannot be physically reverted in this prototype.");
        }
    }
}