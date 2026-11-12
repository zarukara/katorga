using UnityEngine;
using ViewSystem;

namespace PlayerSystem
{
    public struct PointerFrame
    {
        public Vector2 Position;
        public bool LeftDown, LeftHeld, LeftUp, RightDown, RightHeld, RightUp;
        public bool Additive, SelectAll, Clear;
    }

    public sealed class RtsPointerInput
    {
        public const int GroundLayer = 8;
        public const int SoldierLayer = 9;
        private readonly Camera _camera;
        private readonly SelectionFeedback _feedback;
        private readonly PlayerCommands _commands;
        private readonly Plane _ground = new Plane(Vector3.up, Vector3.zero);
        private Vector2 _leftStart, _rightStart;
        private Vector3 _orderPoint;
        private bool _leftPressed, _leftDragging, _rightPressed;

        public RtsPointerInput(Camera camera, SelectionFeedback feedback, PlayerCommands commands)
        { _camera = camera; _feedback = feedback; _commands = commands; }

        public void Poll()
        {
            Process(new PointerFrame
            {
                Position = Input.mousePosition,
                LeftDown = Input.GetMouseButtonDown(0),
                LeftHeld = Input.GetMouseButton(0),
                LeftUp = Input.GetMouseButtonUp(0),
                RightDown = Input.GetMouseButtonDown(1),
                RightHeld = Input.GetMouseButton(1),
                RightUp = Input.GetMouseButtonUp(1),
                Additive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift),
                SelectAll = Input.GetKeyDown(KeyCode.A),
                Clear = Input.GetKeyDown(KeyCode.Escape)
            });
        }

        public void Process(PointerFrame frame)
        {
            Vector2 pointer = frame.Position;
            bool inside = pointer.x >= 0 && pointer.y >= 0 && pointer.x < Screen.width && pointer.y < Screen.height;
            float threshold = Mathf.Max(5, 7 * Screen.height / 900f);
            if (frame.Clear)
            {
                Cancel();
                _commands.Selection = SelectionKind.Clear;
                return;
            }
            if (frame.SelectAll) _commands.Selection = SelectionKind.All;

            if (frame.LeftDown && inside && !_rightPressed)
            { _leftStart = pointer; _leftPressed = true; _leftDragging = false; }

            if (_leftPressed)
            {
                var end = new Vector2(Mathf.Clamp(pointer.x, 0, Screen.width), Mathf.Clamp(pointer.y, 0, Screen.height));
                _leftDragging |= (end - _leftStart).sqrMagnitude >= threshold * threshold;
                var rect = Rect.MinMaxRect(Mathf.Min(_leftStart.x, end.x), Mathf.Min(_leftStart.y, end.y), Mathf.Max(_leftStart.x, end.x), Mathf.Max(_leftStart.y, end.y));
                _feedback.ShowRectangle(_camera, rect, _leftDragging);
                if (frame.LeftUp)
                {
                    _commands.Additive = frame.Additive;
                    _commands.Selection = _leftDragging ? SelectionKind.Rectangle : SelectionKind.Single;
                    _commands.ScreenRectangle = rect;
                    _commands.ClickedUnit = null;
                    if (!_leftDragging && inside && Physics.Raycast(_camera.ScreenPointToRay(pointer), out var hit, 200f, 1 << SoldierLayer, QueryTriggerInteraction.Ignore))
                        _commands.ClickedUnit = hit.collider.GetComponentInParent<SoldierView>();
                    Cancel();
                }
                else if (!frame.LeftHeld) Cancel();
            }

            if (!_leftPressed && frame.RightDown && inside
                && Physics.Raycast(_camera.ScreenPointToRay(pointer), out var groundHit, 200f, 1 << GroundLayer, QueryTriggerInteraction.Ignore))
            {
                _rightPressed = true;
                _rightStart = pointer;
                _orderPoint = groundHit.point;
                _orderPoint.y = 0;
            }
            if (!_rightPressed) return;

            // The press point is the destination; the drag sets the final forward direction.
            Vector3 direction = Vector3.zero;
            var ray = _camera.ScreenPointToRay(pointer);
            if (_ground.Raycast(ray, out float distance)) direction = ray.GetPoint(distance) - _orderPoint;
            direction.y = 0;
            bool hasFacing = (pointer - _rightStart).sqrMagnitude >= threshold * threshold && direction.sqrMagnitude >= 0.04f;
            _feedback.ShowDirection(_orderPoint, direction, inside && hasFacing);
            if (frame.RightUp)
            {
                if (inside)
                {
                    _commands.HasMove = true;
                    _commands.MovePoint = _orderPoint;
                    _commands.HasFacing = hasFacing;
                    _commands.FacingDirection = hasFacing ? direction.normalized : Vector3.zero;
                }
                Cancel();
            }
            else if (!frame.RightHeld) Cancel();
        }

        public void Cancel()
        {
            _leftPressed = _leftDragging = _rightPressed = false;
            _feedback.Hide();
        }
    }
}
