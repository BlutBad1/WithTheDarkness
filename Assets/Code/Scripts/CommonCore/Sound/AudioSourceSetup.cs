using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;
namespace SoundNS
{
    public class AudioSourceSetup : MonoBehaviour
    {
        private AudioKind audioType;
        public virtual AudioKind AudioType
        {
            get { return audioType; }
            set { audioType = value; }
        }
        //float - base sound volume
        protected Dictionary<AudioSource, float> availableSources = new Dictionary<AudioSource, float>();
        protected void Awake()
        {
            AttachVolumeMethodToEvent();
        }
        public void AttachVolumeMethodToEvent() =>
            SettingsNS.AudioSettings.OnVolumeChangeEvent += VolumeChange;
        public void VolumeChange()
        {
            foreach (var s in availableSources.Where(kv => !kv.Key).ToList())
                availableSources.Remove(s.Key);
            foreach (var item in availableSources)
                item.Key.volume = item.Value * SettingsNS.AudioSettings.GetVolumeOfType(AudioType);
        }
    }
}