using UnityEngine;
using ViewSystem;

namespace UISystem
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private MainView mainView;
        [SerializeField] private PanelView panelView;

        private IUIState currentState;

        private void Start()
        {
            SwitchState(new MainState(mainView, panelView, this));
        }

        public void SwitchState(IUIState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }
    }
}