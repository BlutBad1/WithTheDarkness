using UnityEngine;

namespace SoundNS
{
    public class AudioSourceManager : MonoBehaviour
    {
        public AudioSource AudioSource;
        public void PlayAudioSource()
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();
        }
        public void PlayDelayedAudioSource(float delay)
        {
            AudioSource.PlayDelayed(delay);
        }
        public void PlayScheduledAudioSource(double time)
        {
            AudioSource.PlayScheduled(time);
        }
        public void StopAudioSource()
        {
            AudioSource.Stop();
        }
    }
}