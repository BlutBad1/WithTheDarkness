using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 0.5f;
        [Range(0.1f, 3)]
        public float pitch = 1f;
        public bool loop;
        public AudioKind audioKind;
        [HideInInspector]
        public AudioSource source;
    }
}