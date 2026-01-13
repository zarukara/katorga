namespace HotdogLore
{
    public class CaesarHotdog : Hotdog
    {
        public CaesarHotdog() : base("Хот-дог Цезарь")
        {
        }

        public override int GetCost()
        {
            return 290;
        }
    }
}