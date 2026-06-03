using System;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        [Header("SO Events")]
        [SerializeField] private GameEventSO addResourceEvent;
        [SerializeField] private GameEventSO removeResourceEvent;
        [SerializeField] private GameEventSO resetResourcesEvent;

        private readonly Dictionary<ResourceType, int> resources =
            new Dictionary<ResourceType, int>();

        public event Action OnResourcesChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("There are multiple ResourceManager instances in scene");
                return;
            }

            Instance = this;

            InitializeResources();
        }

        public int GetResource(ResourceType type)
        {
            if (!resources.ContainsKey(type))
            {
                resources[type] = 0;
            }

            return resources[type];
        }

        public void Add(ResourceType type, int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            resources[type] = GetResource(type) + amount;

            Debug.Log("Resource added: " + type + " +" + amount + ". New value: " + resources[type]);

            NotifyResourcesChanged();

            if (addResourceEvent != null)
            {
                addResourceEvent.Raise();
            }
        }

        public void Remove(ResourceType type, int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            int currentValue = GetResource(type);
            int newValue = currentValue - amount;

            if (newValue < 0)
            {
                newValue = 0;
            }

            resources[type] = newValue;

            Debug.Log("Resource removed: " + type + " -" + amount + ". New value: " + resources[type]);

            NotifyResourcesChanged();

            if (removeResourceEvent != null)
            {
                removeResourceEvent.Raise();
            }
        }

        public void ResetResources()
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }

            Debug.Log("All resources reset");

            NotifyResourcesChanged();

            if (resetResourcesEvent != null)
            {
                resetResourcesEvent.Raise();
            }
        }

        private void InitializeResources()
        {
            resources.Clear();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }

        private void NotifyResourcesChanged()
        {
            OnResourcesChanged?.Invoke();
        }
    }
}