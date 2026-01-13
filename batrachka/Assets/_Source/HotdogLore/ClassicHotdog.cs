namespace HotdogLore
{
    public class ClassicHotdog : Hotdog
    {
        public ClassicHotdog() : base("Классический хот-дог")
        {
        }

        public override int GetCost()
        {
            return 210;
        }
    }
}