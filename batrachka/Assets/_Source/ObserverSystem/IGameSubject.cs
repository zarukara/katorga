namespace ObserverSystem
{
    public interface IGameSubject
    {
        void AddObserver(IGameObserver observer);
        void RemoveObserver(IGameObserver observer);
        void NotifyObservers(DamageEventData damageEventData);
    }
}