using ServiceSystem;
using UnityEngine;
using ViewSystem;
using GameAnalyticsSDK;

namespace UISystem
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private MainView mainView;
        [SerializeField] private PanelView panelView;

        private IUIState currentState;

        private Score score = new Score();

        private void Start()
        {
            GameAnalytics.NewDesignEvent("game_started");
            
            ISaver saver =
                Bootstrapper.Services.GetService<ISaver>();

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