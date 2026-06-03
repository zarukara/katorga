using System;
using System.Collections.Generic;
using ResourcesSystem;
using TMPro;
using UnityEngine;

namespace UISystem
{
    public class RemoveMenuUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ResourceManager resourceManager;

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

            FillDropdown();
            ResetFields();
        }

        public void OnRemoveClicked()
        {
            if (resourceManager == null)
            {
                resourceManager = ResourceManager.Instance;
            }

            if (resourceManager == null)
            {
                Debug.LogError("RemoveMenuUI: ResourceManager is missing");
                return;
            }

            if (!TryGetAmount(out int amount))
            {
                return;
            }

            ResourceType type = GetSelectedResourceType();

            resourceManager.Remove(type, amount);

            ResetFields();
        }

        private void FillDropdown()
        {
            if (dropdown == null)
            {
                return;
            }

            dropdown.ClearOptions();

            List<string> options = new List<string>();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                options.Add(type.ToString());
            }

            dropdown.AddOptions(options);
        }

        private bool TryGetAmount(out int amount)
        {
            amount = 0;

            if (inputField == null)
            {
                Debug.LogError("RemoveMenuUI: input field is not assigned");
                return false;
            }

            if (!int.TryParse(inputField.text, out amount))
            {
                Debug.LogWarning("RemoveMenuUI: enter a valid integer number");
                return false;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("RemoveMenuUI: amount must be greater than zero");
                return false;
            }

            return true;
        }

        private ResourceType GetSelectedResourceType()
        {
            return (ResourceType)dropdown.value;
        }

        private void ResetFields()
        {
            if (dropdown != null)
            {
                dropdown.value = 0;
                dropdown.RefreshShownValue();
            }

            if (inputField != null)
            {
                inputField.text = string.Empty;
            }
        }
    }
}