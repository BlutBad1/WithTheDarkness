using System.Collections;
using UnityEngine;

namespace SoundNS
{
    public class AudioSourceManager : AudioSourceSetup
    {
        [SerializeField]
        protected AudioSource audioSource;
        Coroutine ChangeClipCoroutine;
        Coroutine VolumeChangeCoroutine;
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
        public void ChangeAudioSourceVolume(float newVolume)
        {
            availableSources[audioSource] = newVolume;
            audioSource.volume = newVolume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
        }
        public void ChangeAudioSourceVolumeSmoothly(float newVolume, float transitionTime)
        {
            if (audioSource.volume != newVolume)
            {
                if (VolumeChangeCoroutine != null)
                    StopCoroutine(VolumeChangeCoroutine);
                VolumeChangeCoroutine = StartCoroutine(ChangeVolumeSmoothly(newVolume, transitionTime));
            }
        }
        IEnumerator ChangeVolumeSmoothly(float newVolume, float transitionTime)
        {
            float percentage = 0;
            while (audioSource.volume != newVolume)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, newVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
            VolumeChangeCoroutine = null;
        }
        public void ChangeSound(Sound s, float transitionTime, bool PlayOnChange = false)
        {
            availableSources[audioSource] = s.volume;
            AudioType = s.audioKind;
            VolumeChange();
            audioSource.pitch = s.pitch;
            audioSource.loop = s.loop;
            s.source = audioSource;
            ChangeAudioClipSmoothly(s.clip, transitionTime, PlayOnChange);
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
        public void PlayAudioSourceSmoothly(float transitionTime)
        {
            if (ChangeClipCoroutine == null)
                ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(transitionTime, PlayOnChange: true));
        }
        public void StopAudioSourceSmoothly(float transitionTime)
        {
            if (ChangeClipCoroutine != null)
                StopCoroutine(ChangeClipCoroutine);
            ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(transitionTime, PlayOnChange: false));
        }
        public void ChangeAudioClipSmoothly(AudioClip audioClip, float transitionTime, bool PlayOnChange)
        {
            if (ChangeClipCoroutine != null)
                StopCoroutine(ChangeClipCoroutine);
            ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(transitionTime, audioClip, PlayOnChange));
        }
        IEnumerator ChangeClipSmoothly(float transitionTime, AudioClip audioClip = null, bool PlayOnChange = false)
        {
            float currentVolume = audioSource.volume, percentage = 0;
            while (audioSource.isPlaying && audioSource.volume > 0)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0, percentage);
                percentage += Time.deltaTime / transitionTime;
                if (audioSource.volume <= 0.001f)
                    audioSource.Stop();
                yield return null;
            }
            audioSource.volume = currentVolume;
            if (audioClip)
                audioSource.clip = audioClip;
            if (PlayOnChange)
                PlayAudioSource();
            ChangeClipCoroutine = null;
        }
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