using TMPro;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourceItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text valueText;

        private ResourceType type;
        private ResourceManager resourceManager;
        private bool isInitialized;

        public void Init(ResourceType resourceType, ResourceManager manager)
        {
            type = resourceType;
            resourceManager = manager;

            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            isInitialized = true;

            if (nameText != null)
            {
                nameText.text = type.ToString();
            }

            Subscribe();
            UpdateView();
        }

        private void OnEnable()
        {
            Subscribe();
            UpdateView();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        public void UpdateView()
        {
            if (!isInitialized)
            {
                return;
            }

            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            if (resourceManager == null)
            {
                return;
            }

            if (valueText == null)
            {
                return;
            }

            int value = resourceManager.GetResource(type);

            valueText.text = value.ToString();

            Debug.Log("ResourceItemView updated: " + type + " = " + value);
        }

        private void Subscribe()
        {
            if (!isInitialized)
            {
                return;
            }

            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            if (resourceManager == null)
            {
                return;
            }

            resourceManager.OnResourcesChanged -= UpdateView;
            resourceManager.OnResourcesChanged += UpdateView;
        }

        private void Unsubscribe()
        {
            if (resourceManager == null)
            {
                return;
            }

            resourceManager.OnResourcesChanged -= UpdateView;
        }
    }
}