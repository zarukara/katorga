using UnityEngine;
using ViewSystem;

namespace AudioSystem
{
    public class SoundPlayer : ISoundPlayer
    {
        private AudioSource audioSource;
        private AudioClip openClip;
        private AudioClip closeClip;

        public SoundPlayer(AudioSource audioSource, AudioClip openClip, AudioClip closeClip)
        {
            this.audioSource = audioSource;
            this.openClip = openClip;
            this.closeClip = closeClip;
        }

        public void PlayOpenSound()
        {
            audioSource.PlayOneShot(openClip);
        }

        public void PlayCloseSound()
        {
            audioSource.PlayOneShot(closeClip);
        }
    }
}