using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputReader playerInputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;
        [SerializeField] private ObstacleSpawner obstacleSpawner;

        public override void InstallBindings()
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

            Container
                .Bind<ObstacleSpawner>()
                .FromInstance(obstacleSpawner)
                .AsSingle();
        }
    }
}