using PlayerSystem;
using UnityEngine;
using Zenject;

namespace TargetSystem
{
    public class MovingTarget : MonoBehaviour, ITargetData
    {
        [Header("Follow Settings")]
        [SerializeField] private float xOffset = 4f;

        private PlayerMovement playerMovement;

        private float startY;
        private float startZ;

        public Vector3 Position => transform.position;

        [Inject]
        public void Construct(PlayerMovement playerMovement)
        {
            this.playerMovement = playerMovement;
        }

        private void Awake()
        {
            startY = transform.position.y;
            startZ = transform.position.z;
        }

        private void LateUpdate()
        {
            if (playerMovement == null)
            {
                return;
            }

            transform.position = new Vector3(
                playerMovement.transform.position.x + xOffset,
                startY,
                startZ);
        }
    }
}