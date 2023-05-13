using System;
using System.Collections;
using UnityEngine;

namespace SoundNS
{
    public class AudioManager : MonoBehaviour
    {
        //NOTE: Don't rename otherwise all sounds and their settings will be gone!!
        public Sound[] sounds;

        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = false;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log($"Sound \"{name}\" is not found!");
                return;
            }
            s.source.Play();
        }
        //It creates a new AudioSource component and destroys the component after the sound is played.
        public void CreateAndPlay(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log($"Sound \"{name}\" is not found!");
                return;
            }
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            s.source.Play();
            StartCoroutine(DeleteAudioSourceAfterPlaying(s.source));
        }

        IEnumerator DeleteAudioSourceAfterPlaying(AudioSource source)
        {
            while (source.isPlaying)
                yield return null;
            Destroy(source);
        }
        public void PlayAFewTimes(string[] names, int times)
        {
            StartCoroutine(PlayTimes(names, times));
        }

        IEnumerator PlayTimes(string[] names, int times)
        {
            yield return null;
            Sound[] currentSounds = new Sound[name.Length];
            for (int i = 0; i < names.Length; i++)
            {
                currentSounds[i] = Array.Find(sounds, sound => sound.name == names[i]);
                if (currentSounds[i] == null)
                    Debug.Log($"Sound \"{names[i]}\" is not found!");
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