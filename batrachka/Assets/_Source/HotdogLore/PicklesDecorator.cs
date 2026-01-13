namespace HotdogLore
{
    public class PicklesDecorator : HotdogDecorator
    {
        private const int pickles_cost = 50;

        public PicklesDecorator(Hotdog hotdog) : base(hotdog)
        {
        }

        public override string GetName()
        {
            return _hotdog.GetName() + " с маринованными огурцами";
        }

        public override int GetCost()
        {
            return _hotdog.GetCost() + pickles_cost;
        }
    }
}