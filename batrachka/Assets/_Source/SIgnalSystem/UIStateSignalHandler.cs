using CoreSystem;
using deVoid.Utils;
using UnityEngine;

namespace SignalSystem
{
    public class UIStateSignalHandler : MonoBehaviour
    {
        [SerializeField] private UIStateController uiStateController;

        private void Awake()
        {
            if (uiStateController == null)
            {
                uiStateController = FindObjectOfType<UIStateController>();
            }
        }

        private void OnEnable()
        {
            Signals.Get<ShowMainMenuSignal>().AddListener(OnShowMainMenuSignal);
            Signals.Get<ShowAddMenuSignal>().AddListener(OnShowAddMenuSignal);
            Signals.Get<ShowRemoveMenuSignal>().AddListener(OnShowRemoveMenuSignal);
        }

        private void OnDisable()
        {
            Signals.Get<ShowMainMenuSignal>().RemoveListener(OnShowMainMenuSignal);
            Signals.Get<ShowAddMenuSignal>().RemoveListener(OnShowAddMenuSignal);
            Signals.Get<ShowRemoveMenuSignal>().RemoveListener(OnShowRemoveMenuSignal);
        }

        private void OnShowMainMenuSignal()
        {
            if (uiStateController == null)
            {
                Debug.LogError("UIStateSignalHandler: UIStateController is missing");
                return;
            }

            Debug.Log("Получен сигнал: ShowMainMenuSignal");

            uiStateController.ShowMainMenu();
        }

        private void OnShowAddMenuSignal()
        {
            if (uiStateController == null)
            {
                Debug.LogError("UIStateSignalHandler: UIStateController is missing");
                return;
            }

            Debug.Log("Получен сигнал: ShowAddMenuSignal");

            uiStateController.ShowAddMenu();
        }

        private void OnShowRemoveMenuSignal()
        {
            if (uiStateController == null)
            {
                Debug.LogError("UIStateSignalHandler: UIStateController is missing");
                return;
            }

            Debug.Log("Получен сигнал: ShowRemoveMenuSignal");

            uiStateController.ShowRemoveMenu();
        }
    }
}