using TMPro;
using UnityEngine;

namespace UiSystem
{
    public class MantraView : MonoBehaviour
    {
        [SerializeField] private TMP_Text mantraText;

        public void Show()
        {
            mantraText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            mantraText.gameObject.SetActive(false);
        }
    }
}