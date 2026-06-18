using ServiceSystem;
using UnityEngine;
using ViewSystem;

namespace UISystem
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private MainView mainView;
        [SerializeField] private PanelView panelView;

        private IUIState currentState;

        private Score score;

        private void Start()
        {
            ISaver saver =
                Bootstrapper.Services.GetService<ISaver>();

            score =
                Bootstrapper.Services.GetService<Score>();

            score.Set(
                saver.LoadScore(
                    Application.persistentDataPath + "/save.json"));

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
