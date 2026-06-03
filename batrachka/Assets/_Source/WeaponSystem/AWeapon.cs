using EnemySystem;
using UnityEngine;

namespace WeaponSystem
{
    public abstract class AWeapon
    {
        protected readonly Camera camera;

        protected AWeapon(Camera camera)
        {
            this.camera = camera;
        }

        public abstract void Attack();

        protected bool TryGetEnemyUnderCursor(out Enemy enemy)
        {
            enemy = null;

            if (camera == null)
            {
                Debug.LogError("Weapon camera is null");
                return false;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                return false;
            }

            enemy = hit.collider.GetComponent<Enemy>();

            if (enemy == null)
            {
                enemy = hit.collider.GetComponentInParent<Enemy>();
            }

            return enemy != null;
        }
    }
}