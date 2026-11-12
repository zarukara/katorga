using UnityEngine;

namespace ViewSystem
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraFraming : MonoBehaviour
    {
        [SerializeField] private float verticalSize = 17.5f;
        [SerializeField] private float horizontalSize = 28.5f;
        private Camera _camera;
        private void Awake() => _camera = GetComponent<Camera>();
        private void LateUpdate() => _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize / _camera.aspect);
    }
}
