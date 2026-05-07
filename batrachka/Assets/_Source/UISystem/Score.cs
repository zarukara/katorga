namespace UISystem
{
    public class Score
    {
        private int value;

        public int Value => value;

        public void Add(int amount)
        {
            value += amount;
        }
    }
}