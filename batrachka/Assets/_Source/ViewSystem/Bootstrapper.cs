using System;
using UnityEngine;
using ServiceSystem;
using GameAnalyticsSDK;

namespace ViewSystem
{
    public class Bootstrapper : MonoBehaviour
    {
        public static ServiceLocator Services;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;

        [SerializeField] private bool useJsonSaver;

        private void Start()
        {
            GameAnalytics.Initialize();
        }

        private void Awake()
        {
            IFadeService fadeService = new FadeService();

            ISoundPlayer soundPlayer =
                new SoundPlayer(audioSource, openClip, closeClip);

            ISaver saver;

            if (useJsonSaver)
            {
                saver = new JsonSaver();
            }
            else
            {
                saver = new PlayerPrefsSaver();
            }

            Services = new ServiceLocator(
                fadeService,
                soundPlayer,
                saver);
        }
    }
}