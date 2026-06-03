using CommandSystem;
using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCore : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float speed = 5f;

        [Header("Weapon View")]
        [SerializeField] private Renderer weaponRenderer;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material fireMaterial;
        [SerializeField] private Material shockMaterial;
        [SerializeField] private Material fireShockMaterial;

        [Header("Command Settings")]
        [SerializeField] private int commandHistoryLimit = 30;
        [SerializeField] private bool showCommandDebugLogs = false;

        private Camera mainCamera;

        private AWeapon baseWeapon;
        private AWeapon fireWeapon;
        private AWeapon shockWeapon;
        private AWeapon fireShockWeapon;

        private AWeapon currentWeapon;
        private WeaponType currentWeaponType;

        private CommandInvoker commandInvoker;

        public WeaponType CurrentWeaponType => currentWeaponType;

        private void Awake()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            commandInvoker = new CommandInvoker(
                commandHistoryLimit,
                showCommandDebugLogs);
        }

        private void Start()
        {
            mainCamera = Camera.main;

            CreateWeapons();

            SelectWeapon(WeaponType.Base);
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleShootInput();
            HandleWeaponInput();
            HandleUndoInput();

            commandInvoker.ProcessCommands();
        }

        private void CreateWeapons()
        {
            baseWeapon = new BaseWeapon(mainCamera);

            fireWeapon = new FireWeaponDecorator(
                new BaseWeapon(mainCamera),
                mainCamera);

            shockWeapon = new ShockWeaponDecorator(
                new BaseWeapon(mainCamera),
                mainCamera);

            fireShockWeapon = new ShockWeaponDecorator(
                new FireWeaponDecorator(
                    new BaseWeapon(mainCamera),
                    mainCamera),
                mainCamera);
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }

            Vector3 movement = direction * speed * Time.deltaTime;

            characterController.Move(movement);
        }

        private void HandleRotation()
        {
            RotateTowardsMouse();
        }

        private void HandleShootInput()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            ShootCommand shootCommand = new ShootCommand(currentWeapon);

            commandInvoker.AddCommand(shootCommand);
        }

        private void HandleWeaponInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddSwitchWeaponCommand(WeaponType.Base);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddSwitchWeaponCommand(WeaponType.Fire);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddSwitchWeaponCommand(WeaponType.Shock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddSwitchWeaponCommand(WeaponType.FireShock);
            }
        }

        private void AddSwitchWeaponCommand(WeaponType weaponType)
        {
            SwitchWeaponCommand switchWeaponCommand =
                new SwitchWeaponCommand(this, weaponType);

            commandInvoker.AddCommand(switchWeaponCommand);
        }

        private void HandleUndoInput()
        {
            if (!Input.GetKeyDown(KeyCode.Z))
            {
                return;
            }

            commandInvoker.UndoLastCommand();
        }

        public void SelectWeapon(WeaponType weaponType)
        {
            currentWeaponType = weaponType;

            switch (weaponType)
            {
                case WeaponType.Base:
                    currentWeapon = baseWeapon;
                    ApplyWeaponMaterial(defaultMaterial);
                    Debug.Log("Weapon selected: BaseWeapon");
                    break;

                case WeaponType.Fire:
                    currentWeapon = fireWeapon;
                    ApplyWeaponMaterial(fireMaterial);
                    Debug.Log("Weapon selected: FireWeaponDecorator");
                    break;

                case WeaponType.Shock:
                    currentWeapon = shockWeapon;
                    ApplyWeaponMaterial(shockMaterial);
                    Debug.Log("Weapon selected: ShockWeaponDecorator");
                    break;

                case WeaponType.FireShock:
                    currentWeapon = fireShockWeapon;

                    if (fireShockMaterial != null)
                    {
                        ApplyWeaponMaterial(fireShockMaterial);
                    }
                    else
                    {
                        ApplyWeaponMaterial(shockMaterial);
                    }

                    Debug.Log("Weapon selected: FireWeaponDecorator + ShockWeaponDecorator");
                    break;
            }
        }

        private void ApplyWeaponMaterial(Material material)
        {
            if (weaponRenderer == null)
            {
                return;
            }

            if (material == null)
            {
                return;
            }

            weaponRenderer.material = material;
        }

        public void RotateTowardsMouse()
        {
            if (mainCamera == null)
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0f;

                if (lookDirection.sqrMagnitude <= 0.01f)
                {
                    return;
                }

                transform.LookAt(transform.position + lookDirection);
            }
        }
    }
}