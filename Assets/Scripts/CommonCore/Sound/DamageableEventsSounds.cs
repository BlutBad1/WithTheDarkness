using UnityEngine;

namespace SoundNS
{
    public class DamageableEventsSounds : MonoBehaviour
    {

        private Damageable damageable;
        public Sound[] OnTakeDamageSounds;
        public Sound[] OnDeathSounds;
        public AudioSource OnTakeDamageAudioSource;
        public AudioSource OnDeathAudioSource;


        void Awake()
        {
            if (!OnTakeDamageAudioSource)
                OnTakeDamageAudioSource = gameObject.AddComponent<AudioSource>();
            if (!OnDeathAudioSource)
                OnDeathAudioSource = gameObject.AddComponent<AudioSource>();
            foreach (Sound s in OnTakeDamageSounds)
                s.source = OnTakeDamageAudioSource;
            foreach (Sound s in OnDeathSounds)
            {
                s.source = OnDeathAudioSource;
            }
        }

        void Start()
        {
            damageable = GetComponent<Damageable>();
            damageable.OnTakeDamage += OnTakeDamage;
            damageable.OnDeath += OnDeath;
        }

        public void PlayRandomSoundFromCollection(Sound[] Collection)
        {

            Sound s = Collection[UnityEngine.Random.Range(0, Collection.Length)];

            s.source.clip = s.clip;
            s.source.volume = s.volume + UnityEngine.Random.Range(-0.2f, 0.2f);
            s.source.pitch = s.pitch + UnityEngine.Random.Range(-0.1f, 0.1f); ;
            s.source.loop = false;
            s.source.playOnAwake = false;

            if (s == null)
            {
                Debug.Log($"Sound is not found!");
                return;
            }


            if (!s.source.isPlaying)
                s.source.Play();

        }
        private void OnTakeDamage(float force, Vector3 hit)
        {
            PlayRandomSoundFromCollection(OnTakeDamageSounds);
        }
        private void OnDeath()
        {
            if (OnDeathSounds.Length!=0)
            PlayRandomSoundFromCollection(OnDeathSounds);
        }
    }
}
