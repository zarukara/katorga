using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using PoolSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerInputReader playerInputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;

        [Header("Obstacles")]
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private Obstacle obstaclePrefab;
        [SerializeField] private Transform obstaclePoolRoot;
        [SerializeField] private int obstaclePoolInitialSize = 5;

        public override void InstallBindings()
        {
            BindPlayer();
            BindObstacles();
        }

        private void BindPlayer()
        {
            Container
                .Bind<PlayerInputReader>()
                .FromInstance(playerInputReader)
                .AsSingle();

            Container
                .Bind<PlayerMovement>()
                .FromInstance(playerMovement)
                .AsSingle();

            Container
                .Bind<PlayerCollisionHandler>()
                .FromInstance(playerCollisionHandler)
                .AsSingle();

            Container
                .Bind<PlayerRespawn>()
                .FromInstance(playerRespawn)
                .AsSingle();
        }

        private void BindObstacles()
        {
            Container
                .Bind<ObstacleSpawner>()
                .FromInstance(obstacleSpawner)
                .AsSingle();

            Container
                .Bind<ObstaclePool>()
                .FromMethod(context =>
                    new ObstaclePool(
                        context.Container,
                        obstaclePrefab,
                        obstaclePoolRoot,
                        obstaclePoolInitialSize
                    ))
                .AsSingle()
                .NonLazy();
        }
    }
}