using AudioSystem;
using ObstacleSystem;
using PoolSystem;
using TargetSystem;
using UnityEngine;
using WeaponSystem;
using Zenject;

namespace PlayerSystem
{
    public class PlayerShooter : MonoBehaviour
    {
        [Header("Shoot")]
        [SerializeField] private Transform shootPoint;

        [Header("Auto Aim")]
        [SerializeField] private float obstacleSearchRadius = 5f;
        [SerializeField] private LayerMask obstacleLayerMask;

        private BulletPool bulletPool;
        private GameplayAudioService audioService;
        private ITargetData targetData;

        [Inject]
        public void Construct(
            BulletPool bulletPool,
            GameplayAudioService audioService,
            ITargetData targetData)
        {
            this.bulletPool = bulletPool;
            this.audioService = audioService;
            this.targetData = targetData;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (shootPoint == null)
            {
                Debug.LogError("ShootPoint is not assigned in PlayerShooter");
                return;
            }

            Bullet bullet = bulletPool.Get();

            bullet.transform.position = shootPoint.position;

            if (TryFindNearestObstacle(out DestructibleObstacle obstacle))
            {
                bullet.LaunchToPoint(obstacle.AimPoint);
            }
            else
            {
                bullet.LaunchToTarget();
            }

            audioService.PlayShootSound();
        }

        private bool TryFindNearestObstacle(out DestructibleObstacle nearestObstacle)
        {
            nearestObstacle = null;

            Collider[] colliders = Physics.OverlapSphere(
                shootPoint.position,
                obstacleSearchRadius,
                obstacleLayerMask,
                QueryTriggerInteraction.Collide);

            float nearestDistance = float.MaxValue;

            foreach (Collider collider in colliders)
            {
                DestructibleObstacle obstacle =
                    collider.GetComponentInParent<DestructibleObstacle>();

                if (obstacle == null)
                {
                    continue;
                }

                if (obstacle.IsDestroyed)
                {
                    continue;
                }

                float distance = Vector3.Distance(
                    shootPoint.position,
                    obstacle.AimPoint);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObstacle = obstacle;
                }
            }

            return nearestObstacle != null;
        }

        private void OnDrawGizmosSelected()
        {
            if (shootPoint == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                shootPoint.position,
                obstacleSearchRadius);
        }
    }
}