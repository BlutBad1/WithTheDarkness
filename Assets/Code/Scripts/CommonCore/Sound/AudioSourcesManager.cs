using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UtilitiesNS;

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
            set { audioSource = value; AudioObject.AudioSource = AudioSource; }
        }
        [HideInInspector]
        public AudioObject AudioObject;
    }
    public class AudioSourcesManager : AudioSetup
    {
        [SerializeField, FormerlySerializedAs("AudioSourceObjects")]
        public AudioSourceObject[] AudioSourceObjects;
        private Dictionary<AudioSource, Coroutine> currentCoroutines;
        private void Start() =>
            InitializeAudioObjects();
        public void InitializeAudioObjects()
        {
            if (AudioSourceObjects != null && AudioSourceObjects.Length > 0)
            {
                foreach (var audioSourceObject in AudioSourceObjects)
                {
                    audioObjects.Add(audioSourceObject.AudioObject = new AudioObject(audioSourceObject.AudioSource,
                        audioSourceObject.AudioSource.volume));
                }
            }
        }
        public AudioSourceObject GetRandomSound()
        {
            int rIndex = UnityEngine.Random.Range(0, AudioSourceObjects.Length);
            return AudioSourceObjects[rIndex];
        }
        public void PlayRandomSound() =>
        PlayAudioSource(GetRandomSound());
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
            PlayAudioSource(audioSourceObject);
        }
        public void PlayAudioSource(AudioSourceObject audioSourceObject) =>
            audioSourceObject.AudioSource.Play();
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
            PlayAudioSourceOnceAtTime(audioSourceObject);
        }
        public void PlayAudioSourceOnceAtTime(AudioSourceObject audioSourceObject)
        {
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
            StopAudioSource(audioSourceObject);
        }
        public void StopAudioSource(AudioSourceObject audioSourceObject) =>
            audioSourceObject.AudioSource.Stop();
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
            StopAudioSourceSmoothly(audioSourceObject, transitionTime);
        }
        public void StopAudioSourceSmoothly(AudioSourceObject audioSourceObject, float transitionTime)
        {
            if (currentCoroutines.ContainsKey(audioSourceObject.AudioSource))
            {
                StopCoroutine(currentCoroutines[audioSourceObject.AudioSource]);
                currentCoroutines.Remove(audioSourceObject.AudioSource);
            }
            currentCoroutines.Add(audioSourceObject.AudioSource, StartCoroutine(ChangeVolumeSmoothly(audioSourceObject.AudioSource, 0, transitionTime)));
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
            AudioUtilities.CopyAudioSourceSettings(audioSourceObject.AudioSource, audioSource);
            audioObjects.Add(newAudioSourceObject.AudioObject = new AudioObject(audioSource,
                  audioSourceObject.AudioObject.StartedVolume));
            newAudioSourceObject.AudioSource = audioSource;
            newAudioSourceObject.AudioSource.outputAudioMixerGroup = audioSourceObject.AudioSource.outputAudioMixerGroup;
            newAudioSourceObject.AudioSource.Play();
            StartCoroutine(DeleteAudioSourceObjectAfterPlaying(newAudioSourceObject));
        }
        IEnumerator DeleteAudioSourceObjectAfterPlaying(AudioSourceObject audioSourceObject)
        {
            while (audioSourceObject.AudioSource.isPlaying)
                yield return null;
            Destroy(audioSourceObject.AudioSource);
            audioObjects.Remove(audioSourceObject.AudioObject);
        }
        IEnumerator ChangeVolumeSmoothly(AudioSource audioSource, float newVolume, float transitionTime)
        {
            float percentage = 0;
            while (audioSource.volume != newVolume)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, newVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
            if (currentCoroutines.ContainsKey(audioSource))
                currentCoroutines.Remove(audioSource);
        }
    }
}