using UnityEngine;
using ViewSystem;

namespace PlayerSystem
{
    public enum SelectionKind { None, Single, Rectangle, All, Clear }

    // One-frame mailbox: input submits intent, ECS owns the state.
    public sealed class PlayerCommands
    {
        public SelectionKind Selection;
        public SoldierView ClickedUnit;
        public Rect ScreenRectangle;
        public bool Additive;
        public bool HasMove;
        public Vector3 MovePoint;
        public bool HasFacing;
        public Vector3 FacingDirection;

        public void Clear()
        {
            Selection = SelectionKind.None;
            ClickedUnit = null;
            Additive = false;
            HasMove = HasFacing = false;
            FacingDirection = default;
        }
    }
}
