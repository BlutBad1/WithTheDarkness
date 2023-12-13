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
        [SerializeProperty("AudioType")]
        public AudioKind audioType;
        private Coroutine ChangeClipCoroutine;
        private Coroutine VolumeChangeCoroutine;
        private AudioObject audioObject;
        public virtual AudioKind AudioType
        {
            get { return audioType; }
            set { audioType = value; }
        }
        private new void Awake()
        {
            base.Awake();
            InitializeAudioSourceManager();
        }
        public void InitializeAudioSourceManager()
        {
            if (audioSource != null)
            {
                availableSources.Add(audioObject = new AudioObject(audioSource, audioSource.volume, AudioType));
                VolumeChange();
            }
        }
        public bool IsPlaying() => audioSource.isPlaying;
        public void CreateAudioSourceCopy(AudioSource source)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            UtilitiesNS.Utilities.CopyAudioSourceSettings(source, audioSource);
            SetAudioSource(audioSource);
        }
        public void SetAudioSource(AudioSource audioSource)
        {
            if (availableSources.Contains(audioObject))
                availableSources.Remove(audioObject);
            this.audioSource = audioSource;
            availableSources.Add(audioObject = new AudioObject(audioSource, audioSource.volume, AudioType));
            this.audioSource.volume = audioSource.volume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
            VolumeChange();
        }
        public void ChangeAudioSourceVolumeSmoothly(float newVolume, float transitionTime = 0.000001f)
        {
            if (audioSource.volume != newVolume * SettingsNS.AudioSettings.GetVolumeOfType(AudioType))
            {
                if (VolumeChangeCoroutine != null)
                    StopCoroutine(VolumeChangeCoroutine);
                VolumeChangeCoroutine = StartCoroutine(ChangeVolumeSmoothly(newVolume, transitionTime));
            }
        }
        public void ChangeAudioSourceVolume(float newVolume)
        {
            newVolume *= SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
            audioSource.volume = newVolume;
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
            ChangeClipCoroutine = StartCoroutine(ChangeClipSmoothly(0, transitionOutTime, 0, PlayOnChange: false));
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
            newVolume *= SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
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
                PlayAudioSource();
            while (Mathf.Abs(audioSource.volume - wantedVolume * GetVolumeOfType(AudioType)) > 0.001f)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, wantedVolume * GetVolumeOfType(AudioType), percentage);
                percentage += Time.deltaTime / transitionInTime;
                yield return null;
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