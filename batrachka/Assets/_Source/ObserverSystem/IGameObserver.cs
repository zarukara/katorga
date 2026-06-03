namespace ObserverSystem
{
    public interface IGameObserver
    {
        void OnNotify(DamageEventData damageEventData);
    }
}