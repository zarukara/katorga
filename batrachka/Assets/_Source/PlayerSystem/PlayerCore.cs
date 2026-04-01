using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    public class PlayerCore : MonoBehaviour
    {
        public CharacterController characterController;
        public float speed = 5f;

        private Camera cam;

        private IWeapon currentWeapon;

        [SerializeField] private Renderer weaponRenderer;
        [SerializeField] private Material defaultMaterial;      // Дефолт ган  
        [SerializeField] private Material fireMaterial;         // Огненный ган
        [SerializeField] private Material shockMaterial;        // Шоковый ган 

        private void Start()
        {
            cam = Camera.main;

            currentWeapon = new BaseWeapon(cam);
            weaponRenderer.material = defaultMaterial;
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
                ICommand shootCommand = new ShootCommand(currentWeapon);
                shootCommand.Execute();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeapon = new BaseWeapon(cam);
                weaponRenderer.material = defaultMaterial;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeapon = new FireDecorator(currentWeapon, weaponRenderer, fireMaterial);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentWeapon = new ShockDecorator(currentWeapon, weaponRenderer, shockMaterial);
            }
        }

        public void Move(Vector2 input)
        {
            Vector3 move = new Vector3(input.x, 0, input.y);
            characterController.Move(move * (speed * Time.deltaTime));
        }

        public void RotateTowardsMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0;
                transform.LookAt(transform.position + lookDir);
            }
        }
    }
}