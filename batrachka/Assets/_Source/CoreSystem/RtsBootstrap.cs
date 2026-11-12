using Leopotam.EcsProto;
using PlayerSystem;
using UnitSystem;
using UnityEngine;
using ViewSystem;

namespace CoreSystem
{
    public sealed class RtsBootstrap : MonoBehaviour
    {
        public const int UnitCount = 60;
        [SerializeField] private SoldierView soldierPrefab;
        [SerializeField] private Transform unitsRoot;
        [SerializeField] private Camera gameCamera;
        [SerializeField] private SelectionFeedback feedback;
        private ProtoWorld _world;
        private IProtoSystems _systems;
        private RtsPointerInput _input;
        private readonly SimulationClock _clock = new SimulationClock();
        public RtsAspect Aspect { get; private set; }
        public PlayerCommands Commands { get; private set; }
        public ArmyStatistics Statistics { get; private set; }

        public void Configure(SoldierView prefab, Transform root, Camera camera, SelectionFeedback selectionFeedback)
        { soldierPrefab = prefab; unitsRoot = root; gameCamera = camera; feedback = selectionFeedback; }

        private void Awake()
        {
            if (soldierPrefab == null || unitsRoot == null || gameCamera == null || feedback == null)
            {
                Debug.LogError("Missing scene references. Use ECS Practice / Rebuild Demo Scene.", this);
                enabled = false;
                return;
            }
            Application.targetFrameRate = 60;
            Aspect = new RtsAspect();
            _world = new ProtoWorld(Aspect);
            Commands = new PlayerCommands();
            Statistics = new ArmyStatistics();
            for (int i = 0; i < UnitCount; i++)
            {
                Vector3 position = new Vector3((i % 10 - 4.5f) * 1.65f, 0, (i / 10 - 2.5f) * 1.65f);
                var view = Instantiate(soldierPrefab, position, Quaternion.identity, unitsRoot);
                view.name = $"Soldier {i + 1:00}";
                Aspect.Positions.NewEntity(out var entity).Value = position;
                Aspect.Speeds.Add(entity).Value = 4.8f;
                Aspect.Destinations.Add(entity);
                Aspect.Rotations.Add(entity) = new UnitRotation { Value = Quaternion.identity, DegreesPerSecond = 360 };
                Aspect.Facings.Add(entity);
                Aspect.Selections.Add(entity);
                Aspect.Views.Add(entity).Value = view;
            }
            _input = new RtsPointerInput(gameCamera, feedback, Commands);
            _systems = new ProtoSystems(_world);
            _systems
                .AddSystem(new SelectionSystem(Aspect, Commands, gameCamera))
                .AddSystem(new MoveOrderSystem(Aspect, Commands))
                .AddSystem(new MovementSystem(Aspect, _clock))
                .AddSystem(new RotationSystem(Aspect, _clock))
                .AddSystem(new UnitViewSystem(Aspect, Statistics))
                .Init();
            Step(0);
        }

        private void Update()
        {
            _input.Poll();
            // Avoid giant jumps when a browser tab regains focus.
            Step(Time.deltaTime);
        }

        public void Step(float deltaTime)
        {
            _clock.DeltaTime = Mathf.Clamp(deltaTime, 0, 0.05f);
            _systems.Run();
            Commands.Clear();
        }

        private void OnApplicationFocus(bool focused) { if (!focused) _input?.Cancel(); }
        private void OnDisable() => _input?.Cancel();
        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
        }
    }
}
