using ResourcesSystem;
using UnityEngine;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        public ResourceManager resourceManager;
        public GameObject resourceItemPrefab;
        public Transform container;

        private void Start()
        {
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                GameObject obj = Instantiate(resourceItemPrefab, container);
                var view = obj.GetComponent<ResourceItemView>();
                view.Init(type, resourceManager);
            }
        }
    
        public void OnResetClicked()
        {
            resourceManager.ResetResources();

            foreach (Transform child in container)
            {
                child.GetComponent<ResourceItemView>().UpdateView();
            }
        }
    }
}