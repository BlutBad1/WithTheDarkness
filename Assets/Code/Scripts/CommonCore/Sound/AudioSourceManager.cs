using SerializableTypes;
using System.Collections;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class AudioSourceManager : AudioSetup
    {
        [SerializeField]
        protected AudioSource audioSource;
        Coroutine ChangeClipCoroutine;
        Coroutine VolumeChangeCoroutine;
        [SerializeProperty("AudioType")]
        public AudioKind audioType;
        public virtual AudioKind AudioType
        {
            get { return audioType; }
            set { audioType = value; }
        }
        private AudioObject audioObject;
        private new void Awake()
        {
            base.Awake();
            availableSources.Add(audioObject = new AudioObject(audioSource, audioSource.volume, AudioType));
            VolumeChange();
        }
        public void SetAudioSource(AudioSource audioSource)
        {
            availableSources.Remove(audioObject);
            this.audioSource = audioSource;
            availableSources.Add(audioObject = new AudioObject(audioSource, audioSource.volume, AudioType));
            this.audioSource.volume = audioSource.volume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
        }
        public void ChangeAudioSourceVolumeSmoothly(float newVolume, float transitionTime = 0f)
        {
            newVolume *= SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
            if (audioSource.volume != newVolume)
            {
                if (VolumeChangeCoroutine != null)
                    StopCoroutine(VolumeChangeCoroutine);
                VolumeChangeCoroutine = StartCoroutine(ChangeVolumeSmoothly(newVolume, transitionTime));
            }
        }
        public void ChangeSound(Sound s, float transitionOutTime, float transitionInTime, bool PlayOnChange = false)
        {
            audioObject.ChangeStartedVolume(s.volume);
            AudioType = s.audioKind;
            audioObject.AudioType = AudioType;
            // VolumeChange();
            audioSource.pitch = s.pitch;
            audioSource.loop = s.loop;
            s.source = audioSource;
            ChangeAudioClipSmoothly(s.volume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType), s.clip, transitionOutTime, transitionInTime, PlayOnChange);
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
        public void PlayAudioSourceSmoothly(float transitionTime) =>
            PlayAudioSourceSmoothly(transitionTime, transitionTime);
        public void PlayAudioSourceSmoothly(float transitionOutTime, float transitionInTime)
        {
            if (ChangeClipCoroutine == null)
                ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(audioSource.volume, transitionOutTime, transitionInTime, PlayOnChange: true));
        }
        public void StopAudioSourceSmoothly(float transitionOutTime)
        {
            if (ChangeClipCoroutine != null)
                StopCoroutine(ChangeClipCoroutine);
            ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(audioSource.volume, transitionOutTime, 0, PlayOnChange: false));
        }
        public void ChangeAudioClipSmoothly(float wantedVolume, AudioClip audioClip, float transitionOutTime, float transitionInTime, bool PlayOnChange)
        {
            if (ChangeClipCoroutine != null)
                StopCoroutine(ChangeClipCoroutine);
            ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(wantedVolume, transitionOutTime, transitionInTime, audioClip, PlayOnChange));
        }
        public void DestroyAfterPlaying()
        {
            PlayAudioSource();
            StartCoroutine(DestroyAfterPlayingCoroutine());
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
        IEnumerator ChangeClipSmoothly(float wantedVolume, float transitionOutTime, float transitionInTime, AudioClip audioClip = null, bool PlayOnChange = false)
        {
            float percentage = 0;
            while (audioSource.isPlaying && audioSource.volume > 0)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0, percentage);
                percentage += Time.deltaTime / transitionOutTime;
                if (audioSource.volume <= 0.001f)
                    audioSource.Stop();
                yield return null;
            }
            percentage = 0;
            if (audioClip)
                audioSource.clip = audioClip;
            if (PlayOnChange)
            {
                PlayAudioSource();
                while (Mathf.Abs(audioSource.volume - wantedVolume) > 0.001f)
                {
                    audioSource.volume = Mathf.Lerp(audioSource.volume, wantedVolume, percentage);
                    percentage += Time.deltaTime / transitionInTime;
                    yield return null;
                }
            }
            audioSource.volume = wantedVolume;
            ChangeClipCoroutine = null;
        }
        IEnumerator DestroyAfterPlayingCoroutine()
        {
            while (audioSource.isPlaying)
                yield return null;
            Destroy(gameObject);
        }
    }
}