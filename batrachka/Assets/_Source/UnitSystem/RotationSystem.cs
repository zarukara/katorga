using CoreSystem;
using Leopotam.EcsProto;
using UnityEngine;

namespace UnitSystem
{
    public sealed class RotationSystem : IProtoRunSystem
    {
        private readonly RtsAspect _aspect;
        private readonly SimulationClock _clock;

        public RotationSystem(RtsAspect aspect, SimulationClock clock)
        { _aspect = aspect; _clock = clock; }

        public void Run()
        {
            foreach (var entity in _aspect.Units)
            {
                var destination = _aspect.Destinations.Get(entity);
                ref var facing = ref _aspect.Facings.Get(entity);
                Vector3 direction;
                if (destination.Active)
                    direction = destination.Value - _aspect.Positions.Get(entity).Value;
                else if (facing.Active)
                    direction = facing.Direction;
                else continue;

                direction.y = 0;
                if (direction.sqrMagnitude < 0.000001f) continue;
                ref var rotation = ref _aspect.Rotations.Get(entity);
                var target = Quaternion.LookRotation(direction);
                rotation.Value = Quaternion.RotateTowards(rotation.Value, target, rotation.DegreesPerSecond * _clock.DeltaTime);
                if (!destination.Active && Quaternion.Angle(rotation.Value, target) < 0.01f)
                {
                    rotation.Value = target;
                    facing.Active = false;
                }
            }
        }
    }
}
