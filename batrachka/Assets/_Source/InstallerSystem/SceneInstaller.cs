using ServiceSystem;
using UISystem;
using UnityEngine;
using ViewSystem;
using Zenject;

namespace InstallerSystem
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("Views")]
        [SerializeField] private MainView mainView;
        [SerializeField] private PanelView panelView;
        [SerializeField] private UISwitcher uiSwitcher;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;

        [Header("Save")]
        [SerializeField] private bool useJsonSaver;

        public override void InstallBindings()
        {
            BindViews();
            BindServices();
            BindUI();
        }

        private void BindViews()
        {
            Container
                .Bind<MainView>()
                .FromInstance(mainView)
                .AsSingle();

            Container
                .Bind<PanelView>()
                .FromInstance(panelView)
                .AsSingle();

            Container
                .Bind<UISwitcher>()
                .FromInstance(uiSwitcher)
                .AsSingle();

            Container.QueueForInject(uiSwitcher);
        }

        private void BindServices()
        {
            Container
                .Bind<IFadeService>()
                .To<FadeService>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ISoundPlayer>()
                .To<SoundPlayer>()
                .AsSingle()
                .WithArguments(audioSource, openClip, closeClip)
                .NonLazy();

            if (useJsonSaver)
            {
                Container
                    .Bind<ISaver>()
                    .To<JsonSaver>()
                    .AsSingle()
                    .NonLazy();
            }
            else
            {
                Container
                    .Bind<ISaver>()
                    .To<PlayerPrefsSaver>()
                    .AsSingle()
                    .NonLazy();
            }
        }

        private void BindUI()
        {
            Container
                .Bind<Score>()
                .AsSingle()
                .NonLazy();

            Container
                .BindFactory<MainState, MainState.Factory>()
                .AsTransient();

            Container
                .BindFactory<PanelState, PanelState.Factory>()
                .AsTransient();
        }
    }
}