using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundNS
{
    public class AudioManager : AudioSetup
    {
        public AudioManager(Sound[] sounds) =>
            this.sounds = sounds;
        //NOTE: Do not rename, otherwise all sounds and their settings will be lost!!
        public Sound[] sounds;
        private void Start() =>
            AssignVariables();
        public void CreateNewAudioSourceNCopySettignsFromSound(Sound s)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            audioObjects.Add(new AudioObject(s.source, s.volume));
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(s.audioKind);
        }
        public void AssignVariables()
        {
            if (sounds != null)
            {
                foreach (Sound s in sounds)
                    CreateNewAudioSourceNCopySettignsFromSound(s);
            }
        }
        public AudioSource CreateAndPlay(Sound s)
        {
            AudioMixerGroup audioMixerGroup = null;
            if (s.source)
            {
                StartCoroutine(DeleteAudioSourceAfterPlaying(s.source));
                audioMixerGroup = s.source.outputAudioMixerGroup;
            }
            else
                audioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(s.audioKind);
            CreateNewAudioSourceNCopySettignsFromSound(s);
            s.source.outputAudioMixerGroup = audioMixerGroup;
            s.source.Play();
            StartCoroutine(DeleteAudioSourceAfterPlaying(s.source));
            return s.source;
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Sound \"{name}\" is not found!");
#endif
                return;
            }
            s.source.Play();
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Sound \"{name}\" is not found!");
#endif
                return;
            }
            if (s.source)
                s.source.Stop();
        }
        public void PlayOnceAtTime(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Sound \"{name}\" is not found!");
#endif
                return;
            }
            if (!s.source.isPlaying)
                s.source.Play();
        }
        public void PlayOnceAtTime(Sound sound) =>
            PlayOnceAtTime(sound.name);
        //It creates a new AudioSource component and destroys the component after the sound is played.
        public AudioSource CreateAndPlay(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Sound \"{name}\" is not found!");
#endif
                return null;
            }
            return CreateAndPlay(s);
        }
        public void PlayAFewTimes(string[] names, int times) =>
            StartCoroutine(PlayTimes(names, times));
        private IEnumerator DeleteAudioSourceAfterPlaying(AudioSource source)
        {
            while (source != null && source.isPlaying)
                yield return null;
            audioObjects.Remove(audioObjects.Find(x => x.AudioSource == source));
            Destroy(source);
        }
        private IEnumerator PlayTimes(string[] names, int times)
        {
            yield return null;
            Sound[] currentSounds = new Sound[name.Length];
            for (int i = 0; i < names.Length; i++)
            {
                currentSounds[i] = Array.Find(sounds, sound => sound.name == names[i]);
                if (currentSounds[i] == null)
                {
#if UNITY_EDITOR
                    Debug.Log($"Sound \"{names[i]}\" is not found!");
#endif
                }
            }
            for (int i = 0; i < times; i++)
            {
                for (int j = 0; j < names.Length; j++)
                    currentSounds[j].source.Play();
                for (int j = 0; j < names.Length; j++)
                {
                    while (currentSounds[j].source.isPlaying)
                        yield return null;
                }
            }
        }
    }
}