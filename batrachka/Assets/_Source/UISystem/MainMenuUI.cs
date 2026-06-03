using System;
using ResourcesSystem;
using UnityEngine;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private GameObject resourceItemPrefab;
        [SerializeField] private Transform container;

        private void Awake()
        {
            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }
        }

        private void Start()
        {
            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            CreateResourceItems();
        }

        public void OnResetClicked()
        {
            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            if (resourceManager == null)
            {
                Debug.LogError("MainMenuUI: ResourceManager is missing");
                return;
            }

            resourceManager.ResetResources();
        }

        private void CreateResourceItems()
        {
            if (resourceManager == null)
            {
                Debug.LogError("MainMenuUI: ResourceManager is missing");
                return;
            }

            if (resourceItemPrefab == null)
            {
                Debug.LogError("MainMenuUI: Resource item prefab is missing");
                return;
            }

            if (container == null)
            {
                Debug.LogError("MainMenuUI: Container is missing");
                return;
            }

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                GameObject resourceItemObject = Instantiate(
                    resourceItemPrefab,
                    container);

                ResourceItemView view =
                    resourceItemObject.GetComponent<ResourceItemView>();

                if (view == null)
                {
                    Debug.LogError("Resource item prefab does not have ResourceItemView");
                    continue;
                }

                view.Init(type, resourceManager);
            }
        }
    }
}