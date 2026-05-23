using UnityEngine;

namespace AudioSystem
{
    public class GameplayAudioService
    {
        private readonly AudioSource audioSource;
        private readonly AudioClip shootClip;
        private readonly AudioClip obstacleHitClip;

        public GameplayAudioService(
            AudioSource audioSource,
            AudioClip shootClip,
            AudioClip obstacleHitClip)
        {
            this.audioSource = audioSource;
            this.shootClip = shootClip;
            this.obstacleHitClip = obstacleHitClip;
        }

        public void PlayShootSound()
        {
            if (shootClip == null)
            {
                return;
            }

            audioSource.PlayOneShot(shootClip);
        }

        public void PlayObstacleHitSound()
        {
            if (obstacleHitClip == null)
            {
                return;
            }

            audioSource.PlayOneShot(obstacleHitClip);
        }
    }
}