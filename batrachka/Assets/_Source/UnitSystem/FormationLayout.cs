using UnityEngine;

namespace UnitSystem
{
    public static class FormationLayout
    {
        public const float Spacing = 1.65f;
        public const float ArenaHalfWidth = 22f;
        public const float ArenaHalfDepth = 16f;

        public static Vector3 Fill(Vector3 target, int count, Vector3[] slots, Vector3 forward = default)
        {
            if (count == 0) return target;
            forward.y = 0;
            forward = forward.sqrMagnitude > 0.001f ? forward.normalized : Vector3.forward;
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            int columns = Mathf.CeilToInt(Mathf.Sqrt(count));
            int rows = Mathf.CeilToInt((float)count / columns);
            Vector3 min = Vector3.positiveInfinity, max = Vector3.negativeInfinity;
            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int inRow = Mathf.Min(columns, count - row * columns);
                float x = (i % columns - (inRow - 1) * 0.5f) * Spacing;
                float z = (row - (rows - 1) * 0.5f) * Spacing;
                slots[i] = right * x + forward * z;
                min = Vector3.Min(min, slots[i]); max = Vector3.Max(max, slots[i]);
            }
            // Clamp the rotated formation as a whole, preserving spacing at the plane edges.
            target.x = Mathf.Clamp(target.x, -ArenaHalfWidth - min.x + 0.7f, ArenaHalfWidth - max.x - 0.7f);
            target.z = Mathf.Clamp(target.z, -ArenaHalfDepth - min.z + 0.7f, ArenaHalfDepth - max.z - 0.7f);
            target.y = 0;
            for (int i = 0; i < count; i++) slots[i] += target;
            return target;
        }
    }
}
