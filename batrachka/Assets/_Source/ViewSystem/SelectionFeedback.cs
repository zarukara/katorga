using UnityEngine;

namespace ViewSystem
{
    // Functional ground lines only. No Canvas, labels or HUD.
    public sealed class SelectionFeedback : MonoBehaviour
    {
        [SerializeField] private LineRenderer rectangle;
        [SerializeField] private LineRenderer direction;
        private readonly Plane _ground = new Plane(Vector3.up, Vector3.zero);

        public void Configure(LineRenderer selectionRectangle, LineRenderer facingPreview)
        { rectangle = selectionRectangle; direction = facingPreview; }

        public void ShowRectangle(Camera camera, Rect rect, bool visible)
        {
            rectangle.enabled = visible;
            if (!visible) return;
            for (int i = 0; i < 4; i++)
            {
                var point = new Vector2(i == 0 || i == 3 ? rect.xMin : rect.xMax, i < 2 ? rect.yMin : rect.yMax);
                var ray = camera.ScreenPointToRay(point);
                if (!_ground.Raycast(ray, out float distance)) { rectangle.enabled = false; return; }
                rectangle.SetPosition(i, ray.GetPoint(distance) + Vector3.up * 0.03f);
            }
        }

        public void ShowDirection(Vector3 origin, Vector3 forward, bool visible)
        {
            direction.enabled = visible;
            if (!visible) return;
            forward.Normalize();
            Vector3 start = origin + Vector3.up * 0.04f;
            Vector3 end = start + forward * 3;
            Vector3 right = Vector3.Cross(Vector3.up, forward) * 0.45f;
            direction.SetPosition(0, start);
            direction.SetPosition(1, end);
            direction.SetPosition(2, end - forward * 0.7f + right);
            direction.SetPosition(3, end);
            direction.SetPosition(4, end - forward * 0.7f - right);
        }

        public void Hide() { rectangle.enabled = false; direction.enabled = false; }
    }
}
