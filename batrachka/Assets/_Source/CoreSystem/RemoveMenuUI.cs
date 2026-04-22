using System;
using ResourcesSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CoreSystem
{
    public class RemoveMenuUI : MonoBehaviour
    {
        public Dropdown dropdown;
        public InputField inputField;
        public ResourceManager resourceManager;

        private void Start()
        {
            dropdown.ClearOptions();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                dropdown.options.Add(new Dropdown.OptionData(type.ToString()));
            }

            dropdown.value = 0;
        }

        public void OnRemoveClicked()
        {
            if (!int.TryParse(inputField.text, out int amount))
            {
                Debug.Log("нормальное число введи, (д)ебень");
                return;
            }

            if (amount <= 0)
            {
                Debug.Log("натуральное число введи, не как подобным тебе");
                return;
            }

            ResourceType type = (ResourceType)dropdown.value;

            int current = resourceManager.GetResource(type);

            if (current < amount)
            {
                Debug.Log("не в ресурсе");
                return;
            }

            resourceManager.Remove(type, amount);

            inputField.text = "";
        }
    }
}