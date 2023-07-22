using System.Collections;
using UnityEngine;

namespace SoundNS
{
    [RequireComponent(typeof(Damageable))]
    public class DamageableEventsSounds : SoundSetup
    {
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
        }
        void Start()
        {
            Damageable damageable = GetComponent<Damageable>();
            damageable.OnTakeDamage += OnTakeDamage;
            damageable.OnDeath += OnDeath;
        }
        public void PlayRandomSoundFromCollection(Sound[] Collection)
        {
            Sound s = Collection[UnityEngine.Random.Range(0, Collection.Length)];
            s.source.clip = s.clip;
            Sounds.Add(s);
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
            {
                s.source.Play();
                StartCoroutine(DeleteSoundAfterPlaying(s));
            }
        }
        IEnumerator DeleteSoundAfterPlaying(Sound s)
        {
            while (s.source.isPlaying)
                yield return null;
            Sounds.Remove(s);
        }
        private void OnDisable()
        {
            if (OnTakeDamageAudioSource.clip)
                Sounds.Remove(Sounds.Find(x => x.clip == OnTakeDamageAudioSource.clip));
            if (OnDeathAudioSource.clip)
                Sounds.Remove(Sounds.Find(x => x.clip == OnDeathAudioSource.clip));
        }
        private void OnTakeDamage(float damage, float force, Vector3 hit)
        {
            if (OnTakeDamageSounds.Length != 0)
                PlayRandomSoundFromCollection(OnTakeDamageSounds);
        }

        private void OnDeath()
        {
            if (OnDeathSounds.Length != 0)
                PlayRandomSoundFromCollection(OnDeathSounds);
        }
    }
}
