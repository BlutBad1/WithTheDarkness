using UnityEngine.Audio;
namespace SoundNS.Ambient
{
    [System.Serializable]
    public class AmbientSound : Sound
    {
        public AudioMixerGroup audioMixerGroup;
        public AmbientSound(Sound s) : base(s)
        {
        }
    }
}
