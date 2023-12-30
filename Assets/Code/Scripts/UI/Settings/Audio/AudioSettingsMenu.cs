using UnityEngine;
using UnityEngine.UI;

namespace UINS.Settings.Audio
{
    public class AudioSettingsMenu : MonoBehaviour
    {
        public Slider MasterVolumeSlider;
        public Slider SoundVolumeSlider;
        public Slider MusicVolumeSlider;
        public Slider SFXVolumeSlider;
        public void InitializeSliders()
        {
            UnityEngine.UI.Slider.SliderEvent sliderBufferEvent = MasterVolumeSlider.onValueChanged;
            MasterVolumeSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            MasterVolumeSlider.value = SettingsNS.AudioSettings.MasterVolume * 100f;
            MasterVolumeSlider.onValueChanged = sliderBufferEvent;
            ////
            sliderBufferEvent = SoundVolumeSlider.onValueChanged;
            SoundVolumeSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            SoundVolumeSlider.value = SettingsNS.AudioSettings.SoundVolume * 100f;
            SoundVolumeSlider.onValueChanged = sliderBufferEvent;
            ////
            sliderBufferEvent = MusicVolumeSlider.onValueChanged;
            MusicVolumeSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            MusicVolumeSlider.value = SettingsNS.AudioSettings.MusicVolume * 100f;
            MusicVolumeSlider.onValueChanged = sliderBufferEvent;
            ////
            sliderBufferEvent = SFXVolumeSlider.onValueChanged;
            SFXVolumeSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            SFXVolumeSlider.value = SettingsNS.AudioSettings.SFXVolume * 100f;
            SFXVolumeSlider.onValueChanged = sliderBufferEvent;
        }
    }
}