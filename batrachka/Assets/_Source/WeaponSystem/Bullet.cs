using System.Collections;
using AudioSystem;
using PoolSystem;
using UnityEngine;

namespace WeaponSystem
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 2f;

        private BulletPool bulletPool;
        private GameplayAudioService audioService;

        private Vector3 direction;
        private Coroutine lifeRoutine;
        private bool isActive;

        [Zenject.Inject]
        public void Construct(
            BulletPool bulletPool,
            GameplayAudioService audioService)
        {
            this.bulletPool = bulletPool;
            this.audioService = audioService;
        }

        public void Launch(Vector3 shootDirection)
        {
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

            transform.position += direction * (speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                return;
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
            isActive = false;

            if (lifeRoutine != null)
            {
                StopCoroutine(lifeRoutine);
                lifeRoutine = null;
            }

            bulletPool.Release(this);
        }
    }
}