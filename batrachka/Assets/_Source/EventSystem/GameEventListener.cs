using UnityEngine;
using UnityEngine.Events;

namespace EventSystem
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
        [SerializeField] private GameEventSO gameEvent;
        [SerializeField] private UnityEvent response;

        private void OnEnable()
        {
            if (gameEvent == null)
            {
                Debug.LogError("GameEventListener: GameEventSO is not assigned");
                return;
            }

            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null)
            {
                return;
            }

            gameEvent.RemoveListener(this);
        }

        public void OnEventRaised()
        {
            Debug.Log("SO-Observer: listener received event -> " + gameObject.name);

            response?.Invoke();
        }
    }
}