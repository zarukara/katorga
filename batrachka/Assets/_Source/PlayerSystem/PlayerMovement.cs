using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Rotation")]
        [SerializeField] private Camera mainCamera;

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            Move();
            RotateToMouse();
        }

        private void Move()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }

            Vector3 movement = direction * (moveSpeed * Time.deltaTime);

            characterController.Move(movement);
        }

        private void RotateToMouse()
        {
            if (mainCamera == null)
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float distance) == false)
            {
                return;
            }

            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 lookDirection = hitPoint - transform.position;

            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude <= 0.01f)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(
                lookDirection.normalized,
                Vector3.up);
        }
    }
}