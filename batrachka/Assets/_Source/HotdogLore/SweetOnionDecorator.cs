namespace HotdogLore
{
    public class SweetOnionDecorator : HotdogDecorator
    {
        private const int sweet_onion_cost = 30;

        public SweetOnionDecorator(Hotdog hotdog) : base(hotdog)
        {
        }

        public override string GetName()
        {
            return _hotdog.GetName() + " со сладким луком";
        }

        public override int GetCost()
        {
            return _hotdog.GetCost() + sweet_onion_cost;
        }
    }
}