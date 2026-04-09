using System.Collections.Generic;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourceManager : MonoBehaviour
    {
        private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

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
        }

        public void Remove(ResourceType type, int amount)
        {
            resources[type] -= amount;

            if (resources[type] < 0)
                resources[type] = 0;
        }

        public void ResetResources()
        {
            foreach (ResourceType type in resources.Keys)
            {
                resources[type] = 0;
            }
        }
    }
}