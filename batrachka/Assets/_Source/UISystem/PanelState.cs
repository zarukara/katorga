using ServiceSystem;
using UnityEngine;
using UnityEngine.UI;
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
        private ISaver saver;

        private Image panelImage;

        private Score score;

        public PanelState(
            MainView mainView,
            PanelView panelView,
            UISwitcher switcher,
            Score score)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;
            this.score = score;

            fadeService =
                Bootstrapper.Services.GetService<IFadeService>();

            soundPlayer =
                Bootstrapper.Services.GetService<ISoundPlayer>();

            saver =
                Bootstrapper.Services.GetService<ISaver>();

            panelImage = panelView.GetComponent<Image>();
        }

        public void Enter()
        {
            panelView.Show();

            mainView.SetInteractable(false);

            fadeService.FadeIn(panelImage, 0.3f);

            soundPlayer.PlayOpenSound();

            panelView.SubscribeOnClose(OnCloseClicked);

            panelView.SubscribeOnCollect(OnCollectClicked);

            panelView.UpdateScore(score.Value);
        }

        public void Exit()
        {
            panelView.UnsubscribeOnClose(OnCloseClicked);

            panelView.UnsubscribeOnCollect(OnCollectClicked);

            fadeService.FadeOut(panelImage, 0.3f);

            soundPlayer.PlayCloseSound();

            saver.SaveScore(
                score.Value,
                Application.persistentDataPath + "/save.json");
        }

        private void OnCloseClicked()
        {
            switcher.SwitchState(
                new MainState(
                    mainView,
                    panelView,
                    switcher,
                    score));
        }

        private void OnCollectClicked()
        {
            score.Add(1);

            panelView.UpdateScore(score.Value);
        }
    }
}