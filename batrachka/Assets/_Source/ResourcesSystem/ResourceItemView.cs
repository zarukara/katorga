using UnityEngine;
using UnityEngine.UI;

namespace ResourcesSystem
{
    public class ResourceItemView : MonoBehaviour
    {
        public Text nameText;
        public Text valueText;

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
    }
}