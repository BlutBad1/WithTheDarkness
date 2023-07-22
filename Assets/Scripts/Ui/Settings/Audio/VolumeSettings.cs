using SoundNS;
using UIControlling;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SettingsNS.VolumeNS
{
    public class VolumeSettings : SliderInputBox
    {
        public SoundsAudioSettings SoundsAudioSettings;
        public AudioKind AudioKind;
        [Tooltip("If IsMasterVolume is true, audioKind would be ignored.")]
        public bool IsMasterVolume = false;
        public void PlaySound()
        {
            if (SoundsAudioSettings)
                SoundsAudioSettings?.PlayRandomSound(AudioKind);
        }
        public override void ChangeInputFieldBySlider()
        {
            base.ChangeInputFieldBySlider();
            if (IsMasterVolume)
                MasterVolume = Slider.value / 100;
            else
                SetVolume(AudioKind, Slider.value / 100);
        }
    }
}