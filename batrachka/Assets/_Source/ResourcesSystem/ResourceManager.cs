using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourceManager : MonoBehaviour
    {
        private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
        public event Action OnResourcesChanged;

        private void Awake()
        {
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }

        public int GetResource(ResourceType type)
        {
            return resources[type];
        }

        public void Add(ResourceType type, int amount)
        {
            resources[type] += amount;
            OnResourcesChanged?.Invoke();
        }

        public void Remove(ResourceType type, int amount)
        {
            resources[type] -= amount;
            OnResourcesChanged?.Invoke();
        }

        public void ResetResources()
        {
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }

            OnResourcesChanged?.Invoke();
        }
    }
}