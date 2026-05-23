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

        [Inject]
        public void Construct(GameplayAudioService audioService)
        {
            this.audioService = audioService;
        }

        public void DestroyObstacle()
        {
            audioService.PlayObstacleHitSound();

            if (fracturedPrefab != null)
            {
                GameObject fragments = Instantiate(
                    fracturedPrefab,
                    transform.position,
                    transform.rotation);

                DisableFragmentsCollisionWithPlayer(fragments);

                Destroy(fragments, destroyFragmentsDelay);
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