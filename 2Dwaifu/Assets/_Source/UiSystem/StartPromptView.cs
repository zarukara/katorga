using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UiSystem
{
    public sealed class StartPromptView : MonoBehaviour
    {
        [FormerlySerializedAs("mantraText")]
        [SerializeField] private TMP_Text promptText;

        public void Show() => promptText.gameObject.SetActive(true);

        public void Hide() => promptText.gameObject.SetActive(false);
    }
}
