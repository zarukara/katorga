using System;
using CoinSystem;
using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using PoolSystem;
using ScoreSystem;
using UiSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CoreSystem.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerInputReader playerInputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerTrail playerTrail;
        [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;

        [Header("UI")]
        [FormerlySerializedAs("mantraView")]
        [SerializeField] private StartPromptView startPromptView;

        [Header("Obstacles")]
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private Obstacle obstaclePrefab;
        [SerializeField] private Transform obstaclePoolRoot;
        [SerializeField, Min(0)] private int obstaclePoolInitialSize = 2;

        [Header("Coins")]
        [SerializeField] private CoinSpawner coinSpawner;
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private Transform coinPoolRoot;
        [SerializeField, Min(0)] private int coinPoolInitialSize = 3;

        public override void InstallBindings()
        {
            ValidateReferences();
            BindPlayer();
            BindUI();
            BindScore();
            BindObstacles();
            BindCoins();
            BindGameStates();
        }

        private void BindPlayer()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().FromInstance(playerInputReader).AsSingle();
            Container.Bind<PlayerMovement>().FromInstance(playerMovement).AsSingle();
            Container.Bind<PlayerTrail>().FromInstance(playerTrail).AsSingle();
            Container.Bind<PlayerCollisionHandler>().FromInstance(playerCollisionHandler).AsSingle();
            Container.Bind<PlayerRespawn>().FromInstance(playerRespawn).AsSingle();

            Container.Bind<Transform>().FromInstance(playerMovement.transform)
                .WhenInjectedInto(typeof(CoinSpawner), typeof(ObstacleSpawner));
        }

        private void BindUI()
        {
            Container.Bind<StartPromptView>().FromInstance(startPromptView).AsSingle();
        }

        private void BindScore()
        {
            Container.Bind<ScoreModel>().AsSingle();
            Container.BindInterfacesTo<CoinScoreHandler>().AsSingle();
        }

        private void BindObstacles()
        {
            Container.Bind<ObstacleSpawner>().FromInstance(obstacleSpawner).AsSingle();
            Container.Bind<ObstaclePool>().FromMethod(_ => new ObstaclePool(
                    () => Container.InstantiatePrefabForComponent<Obstacle>(obstaclePrefab, obstaclePoolRoot),
                    obstaclePoolInitialSize))
                .AsSingle().NonLazy();
        }

        private void BindCoins()
        {
            Container.Bind<CoinSpawner>().FromInstance(coinSpawner).AsSingle();
            Container.Bind<CoinPool>().FromMethod(_ => new CoinPool(
                    () => Container.InstantiatePrefabForComponent<Coin>(coinPrefab, coinPoolRoot),
                    coinPoolInitialSize))
                .AsSingle().NonLazy();
            Container.Bind<ICoinCollectionSource>().To<CoinPool>().FromResolve();
        }

        private void BindGameStates()
        {
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<WaitingState>().AsSingle();
            Container.Bind<PlayingState>().AsSingle();
            Container.Bind<ResettingState>().AsSingle();
        }

        private void ValidateReferences()
        {
            RequireReference(playerInputReader, nameof(playerInputReader));
            RequireReference(playerMovement, nameof(playerMovement));
            RequireReference(playerTrail, nameof(playerTrail));
            RequireReference(playerCollisionHandler, nameof(playerCollisionHandler));
            RequireReference(playerRespawn, nameof(playerRespawn));
            RequireReference(startPromptView, nameof(startPromptView));
            RequireReference(obstacleSpawner, nameof(obstacleSpawner));
            RequireReference(obstaclePrefab, nameof(obstaclePrefab));
            RequireReference(obstaclePoolRoot, nameof(obstaclePoolRoot));
            RequireReference(coinSpawner, nameof(coinSpawner));
            RequireReference(coinPrefab, nameof(coinPrefab));
            RequireReference(coinPoolRoot, nameof(coinPoolRoot));
        }

        private static void RequireReference(UnityEngine.Object value, string field)
        {
            if (value == null)
                throw new InvalidOperationException($"Assign {field} on {nameof(GameInstaller)}.");
        }
    }
}
