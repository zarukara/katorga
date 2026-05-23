using AudioSystem;
using PoolSystem;
using UnityEngine;
using WeaponSystem;
using Zenject;

namespace PlayerSystem
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;

        private BulletPool bulletPool;
        private GameplayAudioService audioService;

        [Inject]
        public void Construct(
            BulletPool bulletPool,
            GameplayAudioService audioService)
        {
            this.bulletPool = bulletPool;
            this.audioService = audioService;
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
            Vector3 shootDirection = transform.forward;

            Bullet bullet = bulletPool.Get();

            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.up);

            bullet.Launch(shootDirection);

            audioService.PlayShootSound();
        }
    }
}