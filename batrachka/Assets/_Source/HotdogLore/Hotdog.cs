namespace HotdogLore
{
    public abstract class Hotdog
    {
        protected string _name;

        public Hotdog(string name)
        {
            _name = name;
        }

        public virtual string GetName()
        {
            return _name;
        }

        public abstract int GetCost();
    }
}