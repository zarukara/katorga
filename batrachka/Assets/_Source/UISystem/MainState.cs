using ViewSystem;
using GameAnalyticsSDK;

namespace UISystem
{
    public class MainState : IUIState
    {
        private MainView mainView;
        private PanelView panelView;
        private UISwitcher switcher;

        private Score score;

        public MainState(
            MainView mainView,
            PanelView panelView,
            UISwitcher switcher,
            Score score)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;
            this.score = score;
        }

        public void Enter()
        {
            panelView.Hide();

            mainView.SetInteractable(true);

            mainView.SubscribeOnOpen(OnOpenClicked);
        }

        public void Exit()
        {
            mainView.UnsubscribeOnOpen(OnOpenClicked);
        }

        private void OnOpenClicked()
        {
            GameAnalytics.NewDesignEvent("panel_opened");
            
            switcher.SwitchState(
                new PanelState(
                    mainView,
                    panelView,
                    switcher,
                    score));
        }
    }
}