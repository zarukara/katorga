using System;
using CoinSystem;
using InputSystem;
using ObstacleSystem;
using PlayerSystem;
using PoolSystem;
using ScoreSystem;
using UiSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CoreSystem.Installers
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [Header("Game Flow")]
        [SerializeField] private GameFlowController gameFlowController;

        [Header("Player")]
        [SerializeField] private PlayerInputReader playerInputReader;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerTrail playerTrail;
        [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
        [SerializeField] private PlayerRespawn playerRespawn;

        [Header("UI")]
        [SerializeField] private StartPromptView startPromptView;
        [SerializeField] private ScoreView scoreView;

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

        protected override void Configure(IContainerBuilder builder)
        {
            ValidateReferences();

            RegisterPlayer(builder);
            RegisterUI(builder);
            RegisterScore(builder);
            RegisterObstacles(builder);
            RegisterCoins(builder);
            RegisterGameStates(builder);
            RegisterGameFlow(builder);
        }

        private void RegisterPlayer(IContainerBuilder builder)
        {
            builder
                .RegisterComponent(playerInputReader)
                .As<IPlayerInput>();

            builder.RegisterComponent(playerMovement);
            builder.RegisterComponent(playerTrail);
            builder.RegisterComponent(playerCollisionHandler);
            builder.RegisterComponent(playerRespawn);

            builder.RegisterInstance(playerMovement.transform);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterComponent(startPromptView);
            builder.RegisterComponent(scoreView);
        }

        private void RegisterScore(IContainerBuilder builder)
        {
            builder.Register<ScoreModel>(Lifetime.Singleton);

            builder.RegisterEntryPoint<CoinScoreHandler>();
        }

        private void RegisterObstacles(IContainerBuilder builder)
        {
            builder.RegisterComponent(obstacleSpawner);

            var obstaclePool = new ObstaclePool(
                () => UnityEngine.Object.Instantiate(
                    obstaclePrefab,
                    obstaclePoolRoot),
                obstaclePoolInitialSize);

            builder.RegisterInstance(obstaclePool);
        }

        private void RegisterCoins(IContainerBuilder builder)
        {
            builder.RegisterComponent(coinSpawner);

            var coinPool = new CoinPool(
                () => UnityEngine.Object.Instantiate(
                    coinPrefab,
                    coinPoolRoot),
                coinPoolInitialSize);

            builder
                .RegisterInstance(coinPool)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        private void RegisterGameStates(IContainerBuilder builder)
        {
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<WaitingState>(Lifetime.Singleton);
            builder.Register<PlayingState>(Lifetime.Singleton);
            builder.Register<ResettingState>(Lifetime.Singleton);
        }

        private void RegisterGameFlow(IContainerBuilder builder)
        {
            builder.RegisterComponent(gameFlowController);
        }

        private void ValidateReferences()
        {
            RequireReference(gameFlowController, nameof(gameFlowController));

            RequireReference(playerInputReader, nameof(playerInputReader));
            RequireReference(playerMovement, nameof(playerMovement));
            RequireReference(playerTrail, nameof(playerTrail));
            RequireReference(playerCollisionHandler, nameof(playerCollisionHandler));
            RequireReference(playerRespawn, nameof(playerRespawn));

            RequireReference(startPromptView, nameof(startPromptView));
            RequireReference(scoreView, nameof(scoreView));

            RequireReference(obstacleSpawner, nameof(obstacleSpawner));
            RequireReference(obstaclePrefab, nameof(obstaclePrefab));
            RequireReference(obstaclePoolRoot, nameof(obstaclePoolRoot));

            RequireReference(coinSpawner, nameof(coinSpawner));
            RequireReference(coinPrefab, nameof(coinPrefab));
            RequireReference(coinPoolRoot, nameof(coinPoolRoot));
        }

        private static void RequireReference(
            UnityEngine.Object value,
            string fieldName)
        {
            if (value == null)
            {
                throw new InvalidOperationException(
                    $"Assign {fieldName} on {nameof(GameLifetimeScope)}.");
            }
        }
    }
}
