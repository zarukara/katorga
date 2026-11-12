using CoreSystem;
using Leopotam.EcsProto;
using UnityEngine;

namespace UnitSystem
{
    public sealed class MovementSystem : IProtoRunSystem
    {
        private readonly RtsAspect _aspect;
        private readonly SimulationClock _clock;
        public MovementSystem(RtsAspect aspect, SimulationClock clock) { _aspect = aspect; _clock = clock; }

        public void Run()
        {
            foreach (var entity in _aspect.Units)
            {
                ref var target = ref _aspect.Destinations.Get(entity);
                if (!target.Active) continue;
                ref var position = ref _aspect.Positions.Get(entity);
                position.Value = Vector3.MoveTowards(position.Value, target.Value, _aspect.Speeds.Get(entity).Value * _clock.DeltaTime);
                if ((position.Value - target.Value).sqrMagnitude < 0.000001f) target.Active = false;
            }
        }
    }
}
