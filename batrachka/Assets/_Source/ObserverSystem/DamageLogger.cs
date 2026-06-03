using UnityEngine;

namespace ObserverSystem
{
    public class DamageLogger : MonoBehaviour, IGameObserver
    {
        [SerializeField] private HealthSubject healthSubject;

        private void Awake()
        {
            if (healthSubject == null)
            {
                healthSubject = FindObjectOfType<HealthSubject>();
            }
        }

        private void OnEnable()
        {
            if (healthSubject == null)
            {
                return;
            }

            healthSubject.AddObserver(this);
        }

        private void OnDisable()
        {
            if (healthSubject == null)
            {
                return;
            }

            healthSubject.RemoveObserver(this);
        }

        public void OnNotify(DamageEventData damageEventData)
        {
            Debug.Log(
                "Observer received damage event. " +
                "Target: " + damageEventData.Target.name +
                ", Damage: " + damageEventData.Damage +
                ", Health: " + damageEventData.CurrentHealth +
                "/" + damageEventData.MaxHealth);
        }
    }
}