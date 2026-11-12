using System.Collections.Generic;
using CoreSystem;
using Leopotam.EcsProto;
using PlayerSystem;
using UnityEngine;

namespace UnitSystem
{
    public sealed class MoveOrderSystem : IProtoRunSystem
    {
        private readonly RtsAspect _aspect;
        private readonly PlayerCommands _commands;
        private readonly List<ProtoEntity> _selected = new List<ProtoEntity>(60);
        private readonly Vector3[] _slots = new Vector3[60];
        private readonly bool[] _used = new bool[60];

        public MoveOrderSystem(RtsAspect aspect, PlayerCommands commands)
        { _aspect = aspect; _commands = commands; }

        public void Run()
        {
            if (!_commands.HasMove) return;
            _selected.Clear();
            foreach (var entity in _aspect.Units)
                if (_aspect.Selections.Get(entity).Selected) _selected.Add(entity);
            if (_selected.Count == 0) return;

            Vector3 facing = _commands.FacingDirection;
            facing.y = 0;
            bool hasFacing = _commands.HasFacing && facing.sqrMagnitude > 0.001f;
            facing = hasFacing ? facing.normalized : Vector3.forward;
            FormationLayout.Fill(_commands.MovePoint, _selected.Count, _slots, facing);
            System.Array.Clear(_used, 0, _used.Length);
            for (int slot = 0; slot < _selected.Count; slot++)
            {
                int best = -1;
                float distance = float.PositiveInfinity;
                for (int i = 0; i < _selected.Count; i++)
                {
                    if (_used[i]) continue;
                    float candidate = (_aspect.Positions.Get(_selected[i]).Value - _slots[slot]).sqrMagnitude;
                    if (candidate < distance) { distance = candidate; best = i; }
                }
                _used[best] = true;
                ref var destination = ref _aspect.Destinations.Get(_selected[best]);
                destination.Value = _slots[slot];
                destination.Active = true;
                ref var order = ref _aspect.Facings.Get(_selected[best]);
                order.Direction = facing;
                order.Active = hasFacing; // A plain click also clears any previous facing order.
            }
            _selected.Clear();
        }
    }
}
