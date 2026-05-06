using ViewSystem;

namespace UISystem
{
    public class PanelState : IUIState
    {
        private MainView mainView;
        private PanelView panelView;
        private UISwitcher switcher;
        
        private IFadeService fadeService;
        private ISoundPlayer soundPlayer;
        private UnityEngine.UI.Image panelImage;

        public PanelState(MainView mainView, PanelView panelView, UISwitcher switcher)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;

            fadeService = Bootstrapper.Services.GetService<IFadeService>();
            panelImage = panelView.GetComponent<UnityEngine.UI.Image>();
            soundPlayer = Bootstrapper.Services.GetService<ISoundPlayer>();
        }

        public void Enter()
        {
            panelView.Show();
            mainView.SetInteractable(false);
            soundPlayer.PlayOpenSound();
            fadeService.FadeIn(panelImage, 0.3f);

            panelView.SubscribeOnClose(OnCloseClicked);
        }

        public void Exit()
        {
            panelView.UnsubscribeOnClose(OnCloseClicked);
            soundPlayer.PlayCloseSound();
            fadeService.FadeOut(panelImage, 0.3f);
        }

        private void OnCloseClicked()
        {
            switcher.SwitchState(new MainState(mainView, panelView, switcher));
        }
    }
}