using TMPro;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourceItemView : MonoBehaviour
    {
        public TMP_Text  nameText;
        public TMP_Text  valueText;

        private ResourceType type;
        private ResourceManager resourceManager;

        public void Init(ResourceType type, ResourceManager manager)
        {
            this.type = type;
            this.resourceManager = manager;

            nameText.text = type.ToString();
            UpdateView();
        }

        public void UpdateView()
        {
            valueText.text = resourceManager.GetResource(type).ToString();
        }
        
        private void OnEnable()
        {
            if (resourceManager != null)
                resourceManager.OnResourcesChanged += UpdateView;
        }

        private void OnDisable()
        {
            if (resourceManager != null)
                resourceManager.OnResourcesChanged -= UpdateView;
        }
    }
}