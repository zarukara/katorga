namespace HotdogLore
{
    public abstract class HotdogDecorator : Hotdog
    {
        protected Hotdog _hotdog;

        public HotdogDecorator(Hotdog hotdog) : base(hotdog.GetName())
        {
            _hotdog = hotdog;
        }

        public override string GetName()
        {
            return _hotdog.GetName();
        }

        public override abstract int GetCost();
    }
}