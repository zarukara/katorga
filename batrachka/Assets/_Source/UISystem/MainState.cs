using ViewSystem;
using Zenject;

namespace UISystem
{
    public class MainState : IUIState
    {
        private readonly MainView mainView;
        private readonly PanelView panelView;
        private readonly UISwitcher switcher;
        private readonly PanelState.Factory panelStateFactory;

        public MainState(
            MainView mainView,
            PanelView panelView,
            UISwitcher switcher,
            PanelState.Factory panelStateFactory)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;
            this.panelStateFactory = panelStateFactory;
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
            switcher.SwitchState(panelStateFactory.Create());
        }

        public class Factory : PlaceholderFactory<MainState>
        {
        }
    }
}