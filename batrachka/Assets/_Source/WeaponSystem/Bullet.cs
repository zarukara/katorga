using System.Collections;
using ObstacleSystem;
using PoolSystem;
using TargetSystem;
using UnityEngine;
using Zenject;

namespace WeaponSystem
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 2f;

        private BulletPool bulletPool;
        private ITargetData targetData;

        private Vector3 direction;
        private Vector3 targetPoint;
        private Coroutine lifeRoutine;
        private bool isActive;
        private bool useMovingTarget;

        [Inject]
        public void Construct(
            BulletPool bulletPool,
            ITargetData targetData)
        {
            this.bulletPool = bulletPool;
            this.targetData = targetData;
        }

        public void LaunchToPoint(Vector3 point)
        {
            targetPoint = point;
            useMovingTarget = false;

            Vector3 shootDirection = targetPoint - transform.position;
            shootDirection.y = 0f;

            Launch(shootDirection);
        }

        public void LaunchToTarget()
        {
            useMovingTarget = true;

            Vector3 shootDirection = targetData.Position - transform.position;
            shootDirection.y = 0f;

            Launch(shootDirection);
        }

        private void Launch(Vector3 shootDirection)
        {
            if (shootDirection.sqrMagnitude <= 0.01f)
            {
                ReturnToPool();
                return;
            }

            direction = shootDirection.normalized;
            isActive = true;

            gameObject.SetActive(true);

            if (lifeRoutine != null)
            {
                StopCoroutine(lifeRoutine);
            }

            lifeRoutine = StartCoroutine(LifeTimer());
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            if (useMovingTarget)
            {
                Vector3 targetDirection = targetData.Position - transform.position;
                targetDirection.y = 0f;

                if (targetDirection.sqrMagnitude > 0.01f)
                {
                    direction = targetDirection.normalized;
                }
            }

            transform.position += direction * speed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                return;
            }

            DestructibleObstacle obstacle =
                other.GetComponentInParent<DestructibleObstacle>();

            if (obstacle != null)
            {
                obstacle.DestroyObstacle();
            }

            ReturnToPool();
        }

        private IEnumerator LifeTimer()
        {
            yield return new WaitForSeconds(lifeTime);

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (!isActive)
            {
                return;
            }

            isActive = false;
            useMovingTarget = false;

            if (lifeRoutine != null)
            {
                StopCoroutine(lifeRoutine);
                lifeRoutine = null;
            }

            bulletPool.Release(this);
        }

        public class Factory : PlaceholderFactory<Bullet>
        {
        }
    }
}