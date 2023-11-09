using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilitiesNS;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    [System.Serializable]
    public class AudioSourceObject
    {
        public string Name;
        [SerializeField]
        private AudioSource audioSource;
        public AudioSource AudioSource
        {
            get { return audioSource; }
            set { audioSource = value; AudioObject?.ChangeAudioSource(AudioSource); }
        }
        public AudioKind AudioKind;
        [HideInInspector]
        public AudioObject AudioObject;
    }
    public class AudioSourcesManager : AudioSetup
    {
        public AudioSourceObject[] AudioSourceObjects;
        private Dictionary<AudioSource, Coroutine> currentCoroutines;
        private new void Awake()
        {
            base.Awake();
            InitializeAudioSourceManager();
        }
        public void InitializeAudioSourceManager()
        {
            if (AudioSourceObjects != null && AudioSourceObjects.Length > 0)
            {
                foreach (var audioSourceObject in AudioSourceObjects)
                {
                    availableSources.Add(audioSourceObject.AudioObject = new AudioObject(audioSourceObject.AudioSource,
                        audioSourceObject.AudioSource.volume, audioSourceObject.AudioKind));
                }
                VolumeChange();
            }
        }
        public void PlayAudioSource(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            audioSourceObject.AudioSource.Play();
        }
        public void PlayAudioSourceOnceAtTime(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            if (!audioSourceObject.AudioSource.isPlaying)
                audioSourceObject.AudioSource.Play();
        }
        public void StopAudioSource(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            audioSourceObject.AudioSource.Stop();
        }
        public void StopAudioSourceSmoothly(string AudioSourceObjectName, float transitionTime)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            if (currentCoroutines.ContainsKey(audioSourceObject.AudioSource))
            {
                StopCoroutine(currentCoroutines[audioSourceObject.AudioSource]);
                currentCoroutines.Remove(audioSourceObject.AudioSource);
            }
            currentCoroutines.Add(audioSourceObject.AudioSource, StartCoroutine(ChangeVolumeSmoothly(audioSourceObject.AudioSource, 0, transitionTime,
                audioSourceObject.AudioKind)));
        }
        public void CreateNewAudioSourceAndPlay(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            CreateNewAudioSourceAndPlay(audioSourceObject);
        }
        public void CreateNewAudioSourceAndPlay(AudioSourceObject audioSourceObject)
        {
            AudioSourceObject newAudioSourceObject = new AudioSourceObject();
            AudioSource audioSource = audioSourceObject.AudioSource.gameObject.AddComponent<AudioSource>();
            Utilities.CopyAudioSourceSettings(audioSourceObject.AudioSource, audioSource);
            availableSources.Add(newAudioSourceObject.AudioObject = new AudioObject(audioSource,
                  audioSourceObject.AudioObject.GetStartedVolume(), audioSourceObject.AudioKind));
            newAudioSourceObject.AudioKind = audioSourceObject.AudioKind;
            newAudioSourceObject.AudioSource = audioSource;
            newAudioSourceObject.AudioSource.Play();
            StartCoroutine(DeleteAudioSourceObjectAfterPlaying(newAudioSourceObject));
        }
        IEnumerator DeleteAudioSourceAfterPlaying(AudioSource source)
        {
            while (source.isPlaying)
                yield return null;
            Destroy(source);
        }
        IEnumerator DeleteAudioSourceObjectAfterPlaying(AudioSourceObject audioSourceObject)
        {
            while (audioSourceObject.AudioSource.isPlaying)
                yield return null;
            Destroy(audioSourceObject.AudioSource);
            availableSources.Remove(audioSourceObject.AudioObject);
        }
        IEnumerator ChangeVolumeSmoothly(AudioSource audioSource, float newVolume, float transitionTime, AudioKind audioKind)
        {
            float percentage = 0;
            while (audioSource.volume != newVolume)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, newVolume * SettingsNS.AudioSettings.GetVolumeOfType(audioKind), percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
            if (currentCoroutines.ContainsKey(audioSource))
                currentCoroutines.Remove(audioSource);
        }
    }
}