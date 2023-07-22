using System.Collections;
using UnityEngine;

namespace SoundNS
{
    public class AudioSourceManager : AudioSourceSetup
    {
        [SerializeField]
        protected AudioSource audioSource;
        private new void Awake()
        {
            base.Awake();
            availableSources.Add(audioSource, audioSource.volume);
        }
        public void SetAudioSource(AudioSource audioSource)
        {
            availableSources.Remove(this.audioSource);
            this.audioSource = audioSource;
            availableSources.Add(this.audioSource, this.audioSource.volume);
            this.audioSource.volume = audioSource.volume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
        }
        public void ChandeAudioSourceVolume(float newVolume)
        {
            availableSources[audioSource] = newVolume;
            audioSource.volume = newVolume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
        }
        public void PlayAudioSource()
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        public void PlayDelayedAudioSource(float delay) =>
            audioSource.PlayDelayed(delay);

        public void PlayScheduledAudioSource(double time) =>
            audioSource.PlayScheduled(time);

        public void StopAudioSource() =>
            audioSource.Stop();

        public void DestroyAfterPlaying()
        {
            PlayAudioSource();
            StartCoroutine(DestroyAfterPlayingCoroutine());
        }
        IEnumerator DestroyAfterPlayingCoroutine()
        {
            while (audioSource.isPlaying)
                yield return null;
            Destroy(gameObject);
        }
    }
}