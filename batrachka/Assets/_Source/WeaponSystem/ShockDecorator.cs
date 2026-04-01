using UnityEngine;

namespace WeaponSystem
{
    public class ShockDecorator : AWeaponDecorator
    {
        private Renderer renderer;
        private Material material;

        public ShockDecorator(IWeapon weapon, Renderer renderer, Material material)
            : base(weapon)
        {
            this.renderer = renderer;
            this.material = material;

            renderer.material = material;
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log("ШОКККККК");
        }
    }
}