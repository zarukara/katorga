using CoinSystem;
using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using PoolSystem;
using ScoreSystem;
using UiSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerInputReader playerInputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerTrail playerTrail;
        [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;

        [Header("UI")]
        [SerializeField] private MantraView mantraView;

        [Header("Obstacles")]
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private Obstacle obstaclePrefab;
        [SerializeField] private Transform obstaclePoolRoot;
        [SerializeField] private int obstaclePoolInitialSize = 2;

        [Header("Coins")]
        [SerializeField] private CoinSpawner coinSpawner;
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private Transform coinPoolRoot;
        [SerializeField] private int coinPoolInitialSize = 3;

        public override void InstallBindings()
        {
            BindPlayer();
            BindUI();
            BindScore();
            BindObstacles();
            BindCoins();
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
                .Bind<PlayerTrail>()
                .FromInstance(playerTrail)
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

        private void BindUI()
        {
            Container
                .Bind<MantraView>()
                .FromInstance(mantraView)
                .AsSingle();
        }

        private void BindScore()
        {
            Container
                .Bind<ScoreModel>()
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

        private void BindCoins()
        {
            Container
                .Bind<CoinSpawner>()
                .FromInstance(coinSpawner)
                .AsSingle();

            Container
                .Bind<CoinPool>()
                .FromMethod(context =>
                    new CoinPool(
                        context.Container,
                        coinPrefab,
                        coinPoolRoot,
                        coinPoolInitialSize
                    ))
                .AsSingle()
                .NonLazy();
        }
    }
}