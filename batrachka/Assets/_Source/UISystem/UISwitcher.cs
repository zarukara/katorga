using UISystem;
using UnityEngine;

namespace ViewSystem
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private MainView mainView;
        [SerializeField] private PanelView panelView;

        private IUIState currentState;

        private Score score = new Score();

        private void Start()
        {
            SwitchState(
                new MainState(
                    mainView,
                    panelView,
                    this,
                    score));
        }

        public void SwitchState(IUIState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }
    }
}