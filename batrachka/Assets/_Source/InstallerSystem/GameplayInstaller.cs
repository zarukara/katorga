using AudioSystem;
using ObstacleSystem;
using PlayerSystem;
using PoolSystem;
using UnityEngine;
using WeaponSystem;
using Zenject;

namespace InstallerSystem
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShooter playerShooter;

        [Header("Bullet Pool")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletPoolParent;

        [Header("Audio")]
        [SerializeField] private AudioSource gameplayAudioSource;
        [SerializeField] private AudioClip shootClip;
        [SerializeField] private AudioClip obstacleHitClip;

        public override void InstallBindings()
        {
            BindPlayer();
            BindAudio();
            BindBulletPool();
            QueueSceneObjectsForInject();
        }

        private void BindPlayer()
        {
            Container
                .Bind<PlayerMovement>()
                .FromInstance(playerMovement)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerShooter>()
                .FromInstance(playerShooter)
                .AsSingle()
                .NonLazy();
        }

        private void BindAudio()
        {
            Container
                .Bind<GameplayAudioService>()
                .AsSingle()
                .WithArguments(
                    gameplayAudioSource,
                    shootClip,
                    obstacleHitClip)
                .NonLazy();
        }

        private void BindBulletPool()
        {
            Container
                .Bind<BulletPool>()
                .AsSingle()
                .WithArguments(
                    bulletPrefab,
                    bulletPoolParent)
                .NonLazy();
        }

        private void QueueSceneObjectsForInject()
        {
            Container.QueueForInject(playerShooter);

            DestructibleObstacle[] obstacles =
                FindObjectsOfType<DestructibleObstacle>();

            foreach (DestructibleObstacle obstacle in obstacles)
            {
                Container.QueueForInject(obstacle);
            }
        }
    }
}