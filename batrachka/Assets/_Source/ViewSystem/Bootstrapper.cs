using ServiceSystem;
using UnityEngine;

namespace ViewSystem
{
    public class Bootstrapper : MonoBehaviour
    {
        public static ServiceLocator Services;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;

        private void Awake()
        {
            IFadeService fadeService = new FadeService();
            ISoundPlayer soundPlayer = new SoundPlayer(audioSource, openClip, closeClip);

            Services = new ServiceLocator(fadeService, soundPlayer);
        }
    }
}