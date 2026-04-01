using UnityEngine;

namespace WeaponSystem
{
    public class FireDecorator : AWeaponDecorator
    {
        private Renderer renderer;
        private Material material;

        public FireDecorator(IWeapon weapon, Renderer renderer, Material material)
            : base(weapon)
        {
            this.renderer = renderer;
            this.material = material;

            renderer.material = material;
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log("Предурь горит х)");
        }
    }
}