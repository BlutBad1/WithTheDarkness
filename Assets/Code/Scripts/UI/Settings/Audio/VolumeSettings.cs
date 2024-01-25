using SoundNS;
using UIControlling;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace UINS.Settings.Audio
{
    public class VolumeSettings : SliderInputBox
    {
        public SoundsAudioSettings SoundsAudioSettings;
        public AudioKind AudioKind;
        [Tooltip("If IsMasterVolume is true, audioKind would be ignored.")]
        public bool IsMasterVolume = false;
        private new void Start()
        {
            if (IsMasterVolume)
                Slider.value = SettingsNS.AudioSettings.MasterVolume * 100f;
            else
                Slider.value = SettingsNS.AudioSettings.GetVolumeOfType(AudioKind) * 100f;
            base.Start();
        }
        public void PlaySound()
        {
            if (SoundsAudioSettings)
                SoundsAudioSettings?.PlayRandomSound(AudioKind);
        }
        public override void ChangeInputFieldBySlider()
        {
            base.ChangeInputFieldBySlider();
            if (IsMasterVolume)
                MasterVolume = Slider.value / 100f;
            else
                SetVolume(AudioKind, Slider.value / 100f);
        }
    }
}