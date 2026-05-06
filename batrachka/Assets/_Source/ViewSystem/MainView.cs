using System;
using UnityEngine;
using UnityEngine.UI;

namespace ViewSystem
{
    public class MainView : MonoBehaviour
    {
        [SerializeField] private Button openButton;

        private Action openAction;

        public void SubscribeOnOpen(Action action)
        {
            openAction = action;
            openButton.onClick.AddListener(OnOpenClicked);
        }

        public void UnsubscribeOnOpen(Action action)
        {
            openButton.onClick.RemoveListener(OnOpenClicked);
        }

        private void OnOpenClicked()
        {
            openAction?.Invoke();
        }

        public void SetInteractable(bool value)
        {
            openButton.interactable = value;
        }
    }
}