using System.Collections.Generic;
using CoreSystem;
using PlayerSystem;
using UnitSystem;
using ViewSystem;
using Leopotam.EcsProto;
using NUnit.Framework;
using UnityEngine;

namespace EditModeTestsSystem
{
    public sealed class ArmyTests
    {
        private ProtoWorld _world;
        private RtsAspect _aspect;
        private PlayerCommands _commands;
        private SimulationClock _clock;
        private IProtoSystems _systems;
        private Camera _camera;

        [SetUp]
        public void Setup()
        {
            _aspect = new RtsAspect(); _world = new ProtoWorld(_aspect);
            _commands = new PlayerCommands(); _clock = new SimulationClock { DeltaTime = 1f / 60 };
            _camera = new GameObject("Test camera").AddComponent<Camera>();
            _camera.transform.position = new Vector3(0, 30, -20); _camera.transform.LookAt(Vector3.zero);
            _systems = new ProtoSystems(_world);
            _systems.AddSystem(new SelectionSystem(_aspect, _commands, _camera))
                .AddSystem(new MoveOrderSystem(_aspect, _commands))
                .AddSystem(new MovementSystem(_aspect, _clock))
                .AddSystem(new RotationSystem(_aspect, _clock)).Init();
            for (int i = 0; i < 60; i++)
            {
                _aspect.Positions.NewEntity(out var entity).Value = new Vector3(i % 10, 0, i / 10);
                _aspect.Speeds.Add(entity).Value = 4.8f;
                _aspect.Selections.Add(entity);
                _aspect.Destinations.Add(entity);
                _aspect.Rotations.Add(entity) = new UnitRotation { Value = Quaternion.identity, DegreesPerSecond = 360 };
                _aspect.Facings.Add(entity);
                _aspect.Views.Add(entity);
            }
        }

        [TearDown]
        public void Teardown()
        { _systems.Destroy(); _world.Destroy(); Object.DestroyImmediate(_camera.gameObject); }

        private void Tick() { _systems.Run(); _commands.Clear(); }
        private void Order(Vector3 point) { _commands.HasMove = true; _commands.MovePoint = point; Tick(); }

        [Test]
        public void UnselectedArmyIgnoresMove()
        {
            Order(new Vector3(10, 0, 10));
            foreach (var e in _aspect.Units) Assert.IsFalse(_aspect.Destinations.Get(e).Active);
        }

        [TestCase(22, 16)] [TestCase(-22, -16)] [TestCase(0, 0)]
        public void SixtySoldiersReceiveDistinctInBoundsSlots(float x, float z)
        {
            _commands.Selection = SelectionKind.All; Tick();
            Order(new Vector3(x, 0, z));
            var targets = new List<Vector3>();
            foreach (var e in _aspect.Units)
            {
                var target = _aspect.Destinations.Get(e).Value;
                Assert.LessOrEqual(Mathf.Abs(target.x), FormationLayout.ArenaHalfWidth - 0.69f);
                Assert.LessOrEqual(Mathf.Abs(target.z), FormationLayout.ArenaHalfDepth - 0.69f);
                foreach (var other in targets) Assert.GreaterOrEqual(Vector3.Distance(target, other), 1.64f);
                targets.Add(target);
            }
            Assert.AreEqual(60, targets.Count);
        }

        [Test]
        public void ReplacementOrderIsSmoothAndArrivesWithoutOvershoot()
        {
            _commands.Selection = SelectionKind.All; Tick();
            Order(new Vector3(12, 0, 10));
            var e = _aspect.Units.FirstSlow().Entity;
            var firstTarget = _aspect.Destinations.Get(e).Value;
            var before = _aspect.Positions.Get(e).Value;
            Order(new Vector3(-12, 0, -10));
            Assert.AreNotEqual(firstTarget, _aspect.Destinations.Get(e).Value);
            Assert.LessOrEqual(Vector3.Distance(before, _aspect.Positions.Get(e).Value), 4.8f / 60 + 0.00001f);
            for (int i = 0; i < 900; i++) Tick();
            foreach (var unit in _aspect.Units)
            {
                Assert.IsFalse(_aspect.Destinations.Get(unit).Active);
                Assert.Less(Vector3.Distance(_aspect.Positions.Get(unit).Value, _aspect.Destinations.Get(unit).Value), 0.001f);
            }
        }

        [Test]
        public void NewOrderOnlyChangesCurrentlySelectedSoldiers()
        {
            _commands.Selection = SelectionKind.All; Tick();
            Order(new Vector3(12, 0, 10));
            var chosen = _aspect.Units.FirstSlow().Entity;
            var previous = new Dictionary<ProtoEntity, Vector3>();
            foreach (var e in _aspect.Units)
            {
                previous.Add(e, _aspect.Destinations.Get(e).Value);
                _aspect.Selections.Get(e).Selected = e == chosen;
            }
            Order(new Vector3(-12, 0, -10));
            foreach (var e in _aspect.Units)
                if (e == chosen) Assert.AreNotEqual(previous[e], _aspect.Destinations.Get(e).Value);
                else Assert.AreEqual(previous[e], _aspect.Destinations.Get(e).Value);
        }

        [Test]
        public void SingleRectangleAdditiveAndClearSelection()
        {
            var entity = _aspect.Units.FirstSlow().Entity;
            var view = new GameObject("Selected unit").AddComponent<SoldierView>();
            _aspect.Views.Get(entity).Value = view;
            try
            {
                _commands.Selection = SelectionKind.All; Tick();
                _commands.Selection = SelectionKind.Single; _commands.ClickedUnit = view; Tick();
                int count = 0;
                foreach (var unit in _aspect.Units) if (_aspect.Selections.Get(unit).Selected) count++;
                Assert.AreEqual(1, count);
                var screen = _camera.WorldToScreenPoint(_aspect.Positions.Get(entity).Value + Vector3.up);
                _commands.Selection = SelectionKind.Rectangle;
                _commands.ScreenRectangle = new Rect(screen.x - 1, screen.y - 1, 2, 2); Tick();
                Assert.IsTrue(_aspect.Selections.Get(entity).Selected);
                _commands.Selection = SelectionKind.Single; _commands.Additive = true; Tick();
                Assert.IsTrue(_aspect.Selections.Get(entity).Selected);
                _commands.Selection = SelectionKind.Clear; Tick();
                foreach (var unit in _aspect.Units) Assert.IsFalse(_aspect.Selections.Get(unit).Selected);
            }
            finally { Object.DestroyImmediate(view.gameObject); }
        }

        [TestCase(1)] [TestCase(7)] [TestCase(60)]
        public void FormationSupportsPartialRows(int count)
        {
            var slots = new Vector3[60];
            FormationLayout.Fill(new Vector3(0, 0, 0), count, slots);
            for (int i = 0; i < count; i++)
                for (int j = i + 1; j < count; j++) Assert.Greater(Vector3.Distance(slots[i], slots[j]), 1.64f);
        }

        [TestCase(0)] [TestCase(45)] [TestCase(90)] [TestCase(225)]
        public void RotatedFormationPreservesSpacingAtPlaneEdge(float angle)
        {
            var slots = new Vector3[60];
            FormationLayout.Fill(new Vector3(22, 0, -16), 60, slots, Quaternion.Euler(0, angle, 0) * Vector3.forward);
            for (int i = 0; i < slots.Length; i++)
            {
                Assert.LessOrEqual(Mathf.Abs(slots[i].x), 21.301f);
                Assert.LessOrEqual(Mathf.Abs(slots[i].z), 15.301f);
                for (int j = i + 1; j < slots.Length; j++) Assert.Greater(Vector3.Distance(slots[i], slots[j]), 1.64f);
            }
        }

        [Test]
        public void SoldiersFaceTheRequestedDirectionAfterArrival()
        {
            _commands.Selection = SelectionKind.All; Tick();
            _commands.HasFacing = true; _commands.FacingDirection = Vector3.left;
            Order(new Vector3(-10, 0, 3));
            for (int i = 0; i < 900; i++) Tick();
            foreach (var e in _aspect.Units)
            {
                Assert.IsFalse(_aspect.Destinations.Get(e).Active);
                Assert.IsFalse(_aspect.Facings.Get(e).Active);
                Assert.Less(Vector3.Angle(_aspect.Rotations.Get(e).Value * Vector3.forward, Vector3.left), 0.01f);
            }
        }

        [Test]
        public void StationaryTurnIsSmoothAndPlainOrderClearsOldFacing()
        {
            var e = _aspect.Units.FirstSlow().Entity;
            _aspect.Facings.Get(e) = new FacingOrder { Active = true, Direction = Vector3.back };
            var position = _aspect.Positions.Get(e).Value;
            Tick();
            float turned = Quaternion.Angle(Quaternion.identity, _aspect.Rotations.Get(e).Value);
            Assert.Greater(turned, 0);
            Assert.LessOrEqual(turned, 6.001f);
            Assert.AreEqual(position, _aspect.Positions.Get(e).Value);
            _aspect.Selections.Get(e).Selected = true;
            Order(new Vector3(3, 0, -5));
            Assert.IsFalse(_aspect.Facings.Get(e).Active);
        }
    }
}
