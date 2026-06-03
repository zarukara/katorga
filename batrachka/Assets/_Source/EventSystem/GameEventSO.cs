using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    [CreateAssetMenu(
        fileName = "GameEvent",
        menuName = "Events/Game Event")]
    public class GameEventSO : ScriptableObject
    {
        private readonly List<IGameEventListener> listeners =
            new List<IGameEventListener>();

        public void RegisterListener(IGameEventListener listener)
        {
            if (listener == null)
            {
                return;
            }

            if (listeners.Contains(listener))
            {
                return;
            }

            listeners.Add(listener);

            //Debug.Log("SO-Observer: listener registered -> " + listener.GetType().Name);
        }

        public void RemoveListener(IGameEventListener listener)
        {
            if (listener == null)
            {
                return;
            }

            if (!listeners.Contains(listener))
            {
                return;
            }

            listeners.Remove(listener);

            //Debug.Log("SO-Observer: listener removed -> " + listener.GetType().Name);
        }

        public void Raise()
        {
            //Debug.Log("SO-Observer: event raised -> " + name);

            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }
    }
}