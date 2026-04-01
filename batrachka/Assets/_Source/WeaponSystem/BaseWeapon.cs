using UnityEngine;

namespace WeaponSystem
{
    public class BaseWeapon : IWeapon
    {
        private Camera cam;

        public BaseWeapon(Camera cam)
        {
            this.cam = cam;
        }

        public virtual void Attack()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                var enemy = hit.collider.GetComponent<EnemySystem.Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }
}