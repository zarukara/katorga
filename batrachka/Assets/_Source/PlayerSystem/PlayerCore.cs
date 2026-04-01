using UnityEngine;
using EnemySystem;

namespace PlayerSystem
{
    public class PlayerCore : MonoBehaviour
    {
        public CharacterController characterController;
        public float speed = 5f;
        
        private Camera cam;
        public Vector3 lookPos;

        private void Start()
        {
            cam = Camera.main;
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            ICommand moveCommand = new MoveCommand(this, input);
            moveCommand.Execute();

            RotateTowardsMouse();

            if (Input.GetMouseButtonDown(0))
            {
                ICommand shootCommand = new ShootCommand(this);
                shootCommand.Execute();
            }
        }

        public void Move(Vector2 input)
        {
            Vector3 move = new Vector3(input.x, 0, input.y);
            characterController.Move(move * (speed * Time.deltaTime));
        }
        
        public void Shoot()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log("Hit: " + hit.collider.name);
                var enemy = hit.collider.GetComponent<EnemySystem.Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }

        private void RotateTowardsMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                lookPos = hit.point;
            }

            Vector3 lookDir = lookPos - transform.position;
            lookDir.y = 0;
            transform.LookAt(transform.position + lookDir, Vector3.up);
        }
    }
}
