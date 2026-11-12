using UnityEngine;

namespace ViewSystem
{
    // Passive display only. Position, rotation and selection belong to ECS.
    public sealed class SoldierView : MonoBehaviour
    {
        [SerializeField] private Renderer body;
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private LineRenderer facingLine;

        public void Configure(Renderer capsule, Material normal, Material selected, LineRenderer direction)
        { body = capsule; normalMaterial = normal; selectedMaterial = selected; facingLine = direction; }

        public void SetSelected(bool selected)
        {
            var material = selected ? selectedMaterial : normalMaterial;
            if (body.sharedMaterial != material) body.sharedMaterial = material;
            // A capsule is symmetric: this short line shows its forward direction.
            facingLine.enabled = selected;
        }

        public bool IsHighlighted => body.sharedMaterial == selectedMaterial;
    }
}
