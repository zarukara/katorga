using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ViewSystem
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        private Action closeAction;
        private Action collectAction;
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button collectButton;

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
        
        public void SubscribeOnCollect(Action action)
        {
            collectAction = action;
            collectButton.onClick.AddListener(OnCollectClicked);
        }

        public void UnsubscribeOnCollect(Action action)
        {
            collectButton.onClick.RemoveListener(OnCollectClicked);
        }

        private void OnCollectClicked()
        {
            collectAction?.Invoke();
        }

        public void UpdateScore(int value)
        {
            scoreText.text = value.ToString();
        }
    }
}