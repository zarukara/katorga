using System;
using UnityEngine;
using UnityEngine.UI;

namespace ViewSystem
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        private Action closeAction;

        public void SubscribeOnClose(Action action)
        {
            closeAction = action;
            closeButton.onClick.AddListener(OnCloseClicked);
        }

        public void UnsubscribeOnClose(Action action)
        {
            closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            closeAction?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}