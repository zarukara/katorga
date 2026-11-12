using Leopotam.EcsProto;
using UnitSystem;

namespace CoreSystem
{
    public sealed class RtsAspect : IProtoAspect
    {
        public ProtoPool<UnitPosition> Positions;
        public ProtoPool<MovementSpeed> Speeds;
        public ProtoPool<MoveDestination> Destinations;
        public ProtoPool<UnitRotation> Rotations;
        public ProtoPool<FacingOrder> Facings;
        public ProtoPool<SelectionState> Selections;
        public ProtoPool<UnitViewReference> Views;
        public readonly ProtoIt Units = new ProtoIt(new[] { typeof(UnitPosition) });
        private ProtoWorld _world;

        public void Init(ProtoWorld world)
        {
            _world = world;
            world.AddAspect(this);
            Positions = new ProtoPool<UnitPosition>(); world.AddPool(Positions);
            Speeds = new ProtoPool<MovementSpeed>(); world.AddPool(Speeds);
            Destinations = new ProtoPool<MoveDestination>(); world.AddPool(Destinations);
            Rotations = new ProtoPool<UnitRotation>(); world.AddPool(Rotations);
            Facings = new ProtoPool<FacingOrder>(); world.AddPool(Facings);
            Selections = new ProtoPool<SelectionState>(); world.AddPool(Selections);
            Views = new ProtoPool<UnitViewReference>(); world.AddPool(Views);
        }

        public void PostInit() => Units.Init(_world);
        public ProtoWorld World() => _world;
    }
}
