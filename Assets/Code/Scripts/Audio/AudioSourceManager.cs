using System.Collections;
using UnityEngine;

namespace SoundNS
{
    public class AudioSourceManager : AudioSetup
    {
        [SerializeField]
        protected AudioSource audioSource;

        private Coroutine ChangeClipCoroutine;
        private Coroutine VolumeChangeCoroutine;
        private AudioObject audioObject;

        public AudioObject AudioObject
        {
            get { return audioObject; }
            set
            {
                if (audioObjects.Contains(AudioObject))
                    audioObjects.Remove(AudioObject);
                audioObject = value;
                audioObjects.Add(audioObject);
            }
        }

        private void Start() =>
            InitializeAudioObject();
        public void InitializeAudioObject()
        {
            if (audioSource)
                AudioObject = new AudioObject(audioSource, audioSource.volume);
        }
        public void CreateAudioSourceCopy(AudioSource source) // don't know why i need this 
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            UtilitiesNS.AudioUtilities.CopyAudioSourceSettings(source, newAudioSource);
            SetAudioSource(newAudioSource);
        }
        public void SetAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            AudioObject = new AudioObject(audioSource, audioSource.volume);
        }
        public void ChangeAudioSourceVolumeSmoothly(float newVolume, float transitionTime = 0.000001f)
        {
            if (audioSource.volume != newVolume)
            {
                if (VolumeChangeCoroutine != null)
                    StopCoroutine(VolumeChangeCoroutine);
                VolumeChangeCoroutine = StartCoroutine(ChangeVolumeSmoothly(newVolume, transitionTime));
            }
        }
        public void ChangeAudioSourceVolume(float newVolume)
        {
            audioSource.volume = newVolume;
            AudioObject.StartedVolume = newVolume;
        }
        public void ChangeSoundSmoothly(Sound s, float transitionOutTime, float transitionInTime, bool PlayOnChange = false)
        {
            audioObject.AudioSource.volume = s.volume;
            audioObject.StartedVolume = s.volume;
            audioSource.pitch = s.pitch;
            audioSource.loop = s.loop;
            s.source = audioSource;
            ChangeAudioClipSmoothly(s.volume, s.clip, transitionOutTime, transitionInTime, PlayOnChange);
        }
        public bool IsAudioSourcePlaying() => audioSource.isPlaying;
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
        private bool ChangeVolumeTo(float wantedVolume, float transitionTime, ref float percentage)
        {
            if (Mathf.Abs(audioSource.volume - wantedVolume) > 0.001f)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, wantedVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                return false;
            }
            return true;
        }
        private IEnumerator ChangeVolumeSmoothly(float newVolume, float transitionTime)
        {
            float percentage = 0;
            while (!ChangeVolumeTo(newVolume, transitionTime, ref percentage))
                yield return null;
            audioSource.volume = newVolume;
            VolumeChangeCoroutine = null;
        }
        private IEnumerator ChangeClipSmoothly(float wantedVolume, float transitionOutTime, float transitionInTime, AudioClip audioClip = null, bool PlayOnChange = false)
        {
            float percentage = 0;
            while (audioSource.isPlaying && !ChangeVolumeTo(0, transitionOutTime, ref percentage))
                yield return null;
            audioSource.Stop();
            percentage = 0;
            audioSource.volume = 0;
            if (audioClip)
                audioSource.clip = audioClip;
            if (PlayOnChange)
                PlayAudioSource();
            while (!ChangeVolumeTo(wantedVolume, transitionInTime, ref percentage))
                yield return null;
            audioSource.volume = wantedVolume;
            ChangeClipCoroutine = null;
        }
        private IEnumerator DestroyAfterPlayingCoroutine()
        {
            while (audioSource.isPlaying)
                yield return null;
            Destroy(gameObject);
        }
    }
}