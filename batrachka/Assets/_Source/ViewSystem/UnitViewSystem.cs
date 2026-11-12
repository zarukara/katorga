using CoreSystem;
using Leopotam.EcsProto;

namespace ViewSystem
{
    public sealed class UnitViewSystem : IProtoRunSystem
    {
        private readonly RtsAspect _aspect;
        private readonly ArmyStatistics _stats;

        public UnitViewSystem(RtsAspect aspect, ArmyStatistics stats)
        { _aspect = aspect; _stats = stats; }

        public void Run()
        {
            _stats.Total = _stats.Selected = _stats.Moving = 0;
            foreach (var entity in _aspect.Units)
            {
                var view = _aspect.Views.Get(entity).Value;
                bool selected = _aspect.Selections.Get(entity).Selected;
                _stats.Total++;
                if (selected) _stats.Selected++;
                if (_aspect.Destinations.Get(entity).Active) _stats.Moving++;
                if (view == null) continue;
                view.transform.SetPositionAndRotation(_aspect.Positions.Get(entity).Value, _aspect.Rotations.Get(entity).Value);
                view.SetSelected(selected);
            }
        }
    }
}
