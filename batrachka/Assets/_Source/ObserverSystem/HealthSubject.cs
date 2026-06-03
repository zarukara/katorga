using System.Collections.Generic;
using UnityEngine;

namespace ObserverSystem
{
    public class HealthSubject : MonoBehaviour, IGameSubject
    {
        private readonly List<IGameObserver> observers = new List<IGameObserver>();

        public void AddObserver(IGameObserver observer)
        {
            if (observer == null)
            {
                return;
            }

            if (observers.Contains(observer))
            {
                return;
            }

            observers.Add(observer);

            Debug.Log("Observer added: " + observer.GetType().Name);
        }

        public void RemoveObserver(IGameObserver observer)
        {
            if (observer == null)
            {
                return;
            }

            if (!observers.Contains(observer))
            {
                return;
            }

            observers.Remove(observer);

            Debug.Log("Observer removed: " + observer.GetType().Name);
        }

        public void NotifyObservers(DamageEventData damageEventData)
        {
            foreach (IGameObserver observer in observers)
            {
                observer.OnNotify(damageEventData);
            }
        }

        public void NotifyDamage(
            GameObject target,
            int damage,
            int currentHealth,
            int maxHealth)
        {
            DamageEventData damageEventData = new DamageEventData(
                target,
                damage,
                currentHealth,
                maxHealth);

            NotifyObservers(damageEventData);
        }
    }
}