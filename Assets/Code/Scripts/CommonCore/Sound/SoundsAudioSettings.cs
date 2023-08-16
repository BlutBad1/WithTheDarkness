using System;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class SoundsAudioSettings : SoundSetup
    {
        public Sound[] SoundsEffects;
        AudioSource[] audioSources = new AudioSource[Enum.GetNames(typeof(AudioKind)).Length];
        AudioManager audioManager;
        new void Awake()
        {
            base.Awake();
            audioManager = gameObject.AddComponent<AudioManager>();
            for (int i = 0; i < Enum.GetNames(typeof(AudioKind)).Length; i++)
                audioSources[i] = gameObject.AddComponent<AudioSource>();
            if (SoundsEffects != null)
            {
                foreach (Sound s in SoundsEffects)
                {
                    s.source = audioSources[(int)s.audioKind % Enum.GetNames(typeof(AudioKind)).Length];
                    s.source.clip = s.clip;
                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                    s.source.loop = s.loop;
                    s.source.playOnAwake = false;
                    Sounds.Add(s);
                    s.source.volume = s.volume * SettingsNS.AudioSettings.GetVolumeOfType(s.audioKind);
                }
            }
            audioManager.sounds = SoundsEffects;
        }
        public void PlayRandomSound(AudioKind audioKind)
        {
            VolumeChange();
            Sound[] s = Array.FindAll(SoundsEffects, x => x.audioKind == audioKind);
            audioManager?.PlayOnceAtTime(s[UnityEngine.Random.Range(0, s.Length)]);
        }
    }
}