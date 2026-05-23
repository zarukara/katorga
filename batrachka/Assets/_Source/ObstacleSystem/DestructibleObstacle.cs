using AudioSystem;
using UnityEngine;
using Zenject;

namespace ObstacleSystem
{
    public class DestructibleObstacle : MonoBehaviour
    {
        [Header("Destruction")]
        [SerializeField] private GameObject fracturedPrefab;
        [SerializeField] private float destroyFragmentsDelay = 4f;

        private GameplayAudioService audioService;
        private bool isDestroyed;

        public bool IsDestroyed => isDestroyed;

        public Vector3 AimPoint
        {
            get
            {
                Collider obstacleCollider = GetComponent<Collider>();

                if (obstacleCollider != null)
                {
                    return obstacleCollider.bounds.center;
                }

                return transform.position;
            }
        }

        [Inject]
        public void Construct(GameplayAudioService audioService)
        {
            this.audioService = audioService;
        }

        public void DestroyObstacle()
        {
            if (isDestroyed)
            {
                return;
            }

            isDestroyed = true;

            if (audioService != null)
            {
                audioService.PlayObstacleHitSound();
            }

            if (fracturedPrefab != null)
            {
                GameObject fragments = Instantiate(
                    fracturedPrefab,
                    transform.position,
                    transform.rotation);

                DisableFragmentsCollisionWithPlayer(fragments);

                Destroy(fragments, destroyFragmentsDelay);
            }

            Collider obstacleCollider = GetComponent<Collider>();

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }

            Renderer obstacleRenderer = GetComponent<Renderer>();

            if (obstacleRenderer != null)
            {
                obstacleRenderer.enabled = false;
            }

            Destroy(gameObject);
        }

        private void DisableFragmentsCollisionWithPlayer(GameObject fragments)
        {
            Collider[] colliders = fragments.GetComponentsInChildren<Collider>();

            foreach (Collider fragmentCollider in colliders)
            {
                fragmentCollider.excludeLayers = LayerMask.GetMask("Player");
            }
        }
    }
}