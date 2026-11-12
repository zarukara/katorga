using CoreSystem;
using Leopotam.EcsProto;
using UnityEngine;

namespace PlayerSystem
{
    public sealed class SelectionSystem : IProtoRunSystem
    {
        private readonly RtsAspect _aspect;
        private readonly PlayerCommands _commands;
        private readonly Camera _camera;

        public SelectionSystem(RtsAspect aspect, PlayerCommands commands, Camera camera)
        { _aspect = aspect; _commands = commands; _camera = camera; }

        public void Run()
        {
            var kind = _commands.Selection;
            if (kind == SelectionKind.None) return;
            foreach (var entity in _aspect.Units)
            {
                bool hit = kind == SelectionKind.All;
                if (kind == SelectionKind.Single)
                    hit = _commands.ClickedUnit != null && _aspect.Views.Get(entity).Value == _commands.ClickedUnit;
                else if (kind == SelectionKind.Rectangle)
                {
                    var screen = _camera.WorldToScreenPoint(_aspect.Positions.Get(entity).Value + Vector3.up);
                    hit = screen.z > 0 && _commands.ScreenRectangle.Contains(new Vector2(screen.x, screen.y));
                }
                ref var state = ref _aspect.Selections.Get(entity);
                state.Selected = kind != SelectionKind.Clear && (hit || (_commands.Additive && state.Selected));
            }
        }
    }
}
