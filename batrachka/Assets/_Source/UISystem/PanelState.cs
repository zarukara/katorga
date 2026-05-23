using ServiceSystem;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using Zenject;

namespace UISystem
{
    public class PanelState : IUIState
    {
        private readonly MainView mainView;
        private readonly PanelView panelView;
        private readonly UISwitcher switcher;
        private readonly MainState.Factory mainStateFactory;

        private readonly IFadeService fadeService;
        private readonly ISoundPlayer soundPlayer;
        private readonly ISaver saver;
        private readonly Score score;

        private readonly Image panelImage;

        public PanelState(
            MainView mainView,
            PanelView panelView,
            UISwitcher switcher,
            MainState.Factory mainStateFactory,
            IFadeService fadeService,
            ISoundPlayer soundPlayer,
            ISaver saver,
            Score score)
        {
            this.mainView = mainView;
            this.panelView = panelView;
            this.switcher = switcher;
            this.mainStateFactory = mainStateFactory;

            this.fadeService = fadeService;
            this.soundPlayer = soundPlayer;
            this.saver = saver;
            this.score = score;

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
            switcher.SwitchState(mainStateFactory.Create());
        }

        private void OnCollectClicked()
        {
            score.Add(1);

            panelView.UpdateScore(score.Value);
        }

        public class Factory : PlaceholderFactory<PanelState>
        {
        }
    }
}