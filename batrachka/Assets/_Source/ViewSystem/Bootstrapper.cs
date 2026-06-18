using UnityEngine;
using ServiceSystem;
using UISystem;

namespace ViewSystem
{
    public class Bootstrapper : MonoBehaviour
    {
        public static ServiceLocator Services;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;

        [SerializeField] private bool useJsonSaver;

        private void Awake()
        {
            Score score = new Score();

            IFadeService fadeService = new FadeService();

            ISoundPlayer soundPlayer =
                new SoundPlayer(audioSource, openClip, closeClip);

            ISaver saver;

            if (useJsonSaver)
            {
                saver = new JsonSaver(score);
            }
            else
            {
                saver = new PlayerPrefsSaver(score);
            }

            Services = new ServiceLocator(
                fadeService,
                soundPlayer,
                saver,
                score);
        }
    }
}
