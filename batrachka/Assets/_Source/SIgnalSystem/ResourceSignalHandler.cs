using deVoid.Utils;
using UISystem;
using UnityEngine;

namespace SignalSystem
{
    public class ResourceSignalHandler : MonoBehaviour
    {
        [SerializeField] private AddMenuUI addMenuUI;
        [SerializeField] private RemoveMenuUI removeMenuUI;
        [SerializeField] private MainMenuUI mainMenuUI;

        private void Awake()
        {
            if (addMenuUI == null)
            {
                addMenuUI = FindObjectOfType<AddMenuUI>(true);
            }

            if (removeMenuUI == null)
            {
                removeMenuUI = FindObjectOfType<RemoveMenuUI>(true);
            }

            if (mainMenuUI == null)
            {
                mainMenuUI = FindObjectOfType<MainMenuUI>(true);
            }
        }

        private void OnEnable()
        {
            Signals.Get<AddResourceSignal>().AddListener(OnAddResourceSignal);
            Signals.Get<RemoveResourceSignal>().AddListener(OnRemoveResourceSignal);
            Signals.Get<ResetResourcesSignal>().AddListener(OnResetResourcesSignal);
        }

        private void OnDisable()
        {
            Signals.Get<AddResourceSignal>().RemoveListener(OnAddResourceSignal);
            Signals.Get<RemoveResourceSignal>().RemoveListener(OnRemoveResourceSignal);
            Signals.Get<ResetResourcesSignal>().RemoveListener(OnResetResourcesSignal);
        }

        private void OnAddResourceSignal()
        {
            if (addMenuUI == null)
            {
                Debug.LogError("ResourceSignalHandler: AddMenuUI is missing");
                return;
            }

            Debug.Log("Получен сигнал: AddResourceSignal");

            addMenuUI.OnAddClicked();
        }

        private void OnRemoveResourceSignal()
        {
            if (removeMenuUI == null)
            {
                Debug.LogError("ResourceSignalHandler: RemoveMenuUI is missing");
                return;
            }

            Debug.Log("Получен сигнал: RemoveResourceSignal");

            removeMenuUI.OnRemoveClicked();
        }

        private void OnResetResourcesSignal()
        {
            if (mainMenuUI == null)
            {
                Debug.LogError("ResourceSignalHandler: MainMenuUI is missing");
                return;
            }

            Debug.Log("Получен сигнал: ResetResourcesSignal");

            mainMenuUI.OnResetClicked();
        }
    }
}