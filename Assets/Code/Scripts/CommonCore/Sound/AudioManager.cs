using System;
using System.Collections;
using UnityEngine;

namespace SoundNS
{
    public class AudioManager : AudioSetup
    {
        public AudioManager(Sound[] sounds) =>
            this.sounds = sounds;
        //NOTE: Don't rename otherwise all sounds and their settings will be gone!!
        public Sound[] sounds;
        new void Awake()
        {
            base.Awake();
            AssignVariables();
        }
        public void AssignVariables()
        {
            if (sounds != null)
            {
                foreach (Sound s in sounds)
                {
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;
                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                    s.source.loop = s.loop;
                    s.source.playOnAwake = false;
                    availableSources.Add(new AudioObject(s.source, s.volume, s.audioKind));
                    s.source.volume = s.volume * SettingsNS.AudioSettings.GetVolumeOfType(s.audioKind);
                }
            }
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
        public AudioSource CreateAndPlay(Sound s)
        {
            if (s.source)
                StartCoroutine(DeleteAudioSourceAfterPlaying(s.source));
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            s.source.Play();
            availableSources.Add(new AudioObject(s.source, s.volume, s.audioKind));
            s.source.volume = s.volume * SettingsNS.AudioSettings.GetVolumeOfType(s.audioKind);
            StartCoroutine(DeleteAudioSourceAfterPlaying(s.source));
            return s.source;
        }
        IEnumerator DeleteAudioSourceAfterPlaying(AudioSource source)
        {
            while (source != null && source.isPlaying)
                yield return null;
            availableSources.Remove(availableSources.Find(x => x.AudioSource == source));
            Destroy(source);
        }
        public void PlayAFewTimes(string[] names, int times) =>
            StartCoroutine(PlayTimes(names, times));
        IEnumerator PlayTimes(string[] names, int times)
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