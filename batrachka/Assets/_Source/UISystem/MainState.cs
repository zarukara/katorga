using ViewSystem;

namespace UISystem
{
    public class MainState : IUIState
    {
        private MainView mainView;
        private PanelView panelView;
        private UISwitcher switcher;

        public MainState(MainView mainView, PanelView panelView, UISwitcher switcher)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;
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
            switcher.SwitchState(new PanelState(mainView, panelView, switcher));
        }
    }
}