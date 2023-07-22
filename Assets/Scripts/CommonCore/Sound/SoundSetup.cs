using System.Collections.Generic;
using UnityEngine;
namespace SoundNS
{
    public class SoundSetup : MonoBehaviour
    {
        static protected List<Sound> Sounds = new List<Sound>();
        protected void Awake()
        {
            AttachVolumeMethodToEvent();
        }
        public void AttachVolumeMethodToEvent() =>
          SettingsNS.AudioSettings.OnVolumeChangeEvent += VolumeChange;
        public void VolumeChange()
        {
            Sounds.RemoveAll(x => !x.source);
            foreach (var item in Sounds)
                item.source.volume = item.volume * SettingsNS.AudioSettings.GetVolumeOfType(item.audioKind);
        }
    }
}