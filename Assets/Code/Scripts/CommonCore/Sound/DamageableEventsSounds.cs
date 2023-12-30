using DamageableNS;
using UnityEngine;

namespace SoundNS
{
    public class DamageableEventsSounds : AudioSetup
    {
        public Damageable Damageable;
        public Sound[] OnTakeDamageSounds;
        public Sound[] OnDeathSounds;
        public AudioSource OnTakeDamageAudioSource;
        public AudioSource OnDeathAudioSource;
        new void Awake()
        {
            base.Awake();
            if (!OnTakeDamageAudioSource)
                OnTakeDamageAudioSource = gameObject.AddComponent<AudioSource>();
            if (!OnDeathAudioSource)
                OnDeathAudioSource = gameObject.AddComponent<AudioSource>();
            foreach (Sound s in OnTakeDamageSounds)
                s.source = OnTakeDamageAudioSource;
            foreach (Sound s in OnDeathSounds)
                s.source = OnDeathAudioSource;
            availableSources.Add(new AudioObject(OnTakeDamageAudioSource, OnTakeDamageAudioSource.volume, SettingsNS.AudioSettings.AudioKind.Sound));
            availableSources.Add(new AudioObject(OnDeathAudioSource, OnDeathAudioSource.volume, SettingsNS.AudioSettings.AudioKind.Sound));
            VolumeChange();
        }
        void Start()
        {
            if (!Damageable)
                Damageable = GetComponent<Damageable>();
            Damageable.OnTakeDamageWithDamageData += OnTakeDamage;
            Damageable.OnDead += OnDeath;
        }
        public void PlayRandomSoundFromCollection(Sound[] Collection)
        {
            Sound s = Collection[UnityEngine.Random.Range(0, Collection.Length)];
            s.source.clip = s.clip;
            AudioObject audioObject = availableSources.Find(x => x.AudioSource == s.source);
            audioObject.ChangeStartedVolume(s.volume);
            audioObject.AudioType = s.audioKind;
            s.source.volume = s.volume * SettingsNS.AudioSettings.GetVolumeOfType(s.audioKind);
            s.source.volume = s.source.volume + UnityEngine.Random.Range(-0.2f, 0.2f);
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
            // StartCoroutine(DeleteSoundAfterPlaying(s));
        }
        //IEnumerator DeleteSoundAfterPlaying(Sound s)
        //{
        //    while (s.source.isPlaying)
        //        yield return null;
        //    Sounds.Remove(s);
        //}
        //private void OnDisable()
        //{
        //    if (OnTakeDamageAudioSource.clip)
        //        Sounds.Remove(Sounds.Find(x => x.clip == OnTakeDamageAudioSource.clip));
        //    if (OnDeathAudioSource.clip)
        //        Sounds.Remove(Sounds.Find(x => x.clip == OnDeathAudioSource.clip));
        //}
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
