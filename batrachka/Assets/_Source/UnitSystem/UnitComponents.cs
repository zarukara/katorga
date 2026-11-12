using UnityEngine;
using ViewSystem;

namespace UnitSystem
{
    public struct UnitPosition { public Vector3 Value; }
    public struct MovementSpeed { public float Value; }
    public struct MoveDestination { public Vector3 Value; public bool Active; }
    public struct UnitRotation { public Quaternion Value; public float DegreesPerSecond; }
    public struct FacingOrder { public Vector3 Direction; public bool Active; }
    public struct SelectionState { public bool Selected; }
    public struct UnitViewReference { public SoldierView Value; }
}
