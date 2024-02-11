using DamageableNS;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class DamageableEventsSounds : AudioSetup
    {
        public AudioKind AudioType = AudioKind.Sound;
        public Damageable Damageable;
        public Sound[] OnTakeDamageSounds;
        public Sound[] OnDeathSounds;
        public AudioSource OnTakeDamageAudioSource;
        public AudioSource OnDeathAudioSource;
        protected void Start()
        {
            if (!OnTakeDamageAudioSource)
            {
                OnTakeDamageAudioSource = gameObject.AddComponent<AudioSource>();
                OnTakeDamageAudioSource.outputAudioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(AudioType);
            }
            if (!OnDeathAudioSource)
            {
                OnDeathAudioSource = gameObject.AddComponent<AudioSource>();
                OnDeathAudioSource.outputAudioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(AudioType);
            }
            foreach (Sound s in OnTakeDamageSounds)
                s.source = OnTakeDamageAudioSource;
            foreach (Sound s in OnDeathSounds)
                s.source = OnDeathAudioSource;
            audioObjects.Add(new AudioObject(OnTakeDamageAudioSource, OnTakeDamageAudioSource.volume));
            audioObjects.Add(new AudioObject(OnDeathAudioSource, OnDeathAudioSource.volume));
            if (!Damageable)
                Damageable = GetComponent<Damageable>();
            Damageable.OnTakeDamageWithDamageData += OnTakeDamage;
            Damageable.OnDead += OnDeath;
        }
        public void PlayRandomSoundFromCollection(Sound[] Collection)
        {
            Sound s = Collection[UnityEngine.Random.Range(0, Collection.Length)];
            AudioObject audioObject = audioObjects.Find(x => x.AudioSource == s.source);
            audioObject.AudioSource.outputAudioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(s.audioKind);
            s.source.clip = s.clip;
            s.source.volume = s.volume + UnityEngine.Random.Range(-0.2f, 0.2f);
            s.source.pitch = s.pitch + UnityEngine.Random.Range(-0.1f, 0.1f); ;
            s.source.loop = false;
            s.source.playOnAwake = false;
            if (s == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Sound is not found!");
#endif
                return;
            }
            if (!s.source.isPlaying)
                s.source.Play();
        }
        private void OnTakeDamage(TakeDamageData takeDamageData)
        {
            if (OnTakeDamageSounds.Length != 0 && takeDamageData.HitData != null)
                PlayRandomSoundFromCollection(OnTakeDamageSounds);
        }
        private void OnDeath()
        {
            if (OnDeathSounds.Length != 0)
                PlayRandomSoundFromCollection(OnDeathSounds);
        }
    }
}
