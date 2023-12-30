using SettingsNS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UINS.Settings.Video
{
    public class VideoSettingsMenu : MonoBehaviour
    {
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown FullscreenModeDropdown;
        public Toggle VsyncToggle;
        public Toggle HdrToggle;
        public Slider BrightnessSlider;
        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;
        [HideInInspector]
        public float CurrentRefreshRate;
        private int currentResolutionIndex = 0;
        public void InitializeResolutionDropdown()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            ResolutionDropdown.ClearOptions();
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRate == CurrentRefreshRate)
                    filteredResolutions.Add(resolutions[i]);
            }
            List<string> options = new List<string>();
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
                options.Add(resolutionOption);
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                    currentResolutionIndex = i;
            }
            ResolutionDropdown.AddOptions(options);
            ResolutionDropdown.value = currentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();
        }
        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            GraphicSettings.CurrentResolution = resolution;
        }
        private enum FullscreeMode
        {
            Fullscreen, Borderless, Windowed
        }
        public void InitializeFullscreenModeDropdown() =>
             FullscreenModeDropdown.value = GraphicSettings.CurrentFullScreenMode == FullScreenMode.ExclusiveFullScreen ? 0 : (int)GraphicSettings.CurrentFullScreenMode - 1;
        public void SetFullscreenMode(int fullscreenModeIndex)
        {
            switch (fullscreenModeIndex)
            {
                case (int)FullscreeMode.Fullscreen:
                    GraphicSettings.CurrentFullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case (int)FullscreeMode.Borderless:
                    GraphicSettings.CurrentFullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case (int)FullscreeMode.Windowed:
                    GraphicSettings.CurrentFullScreenMode = FullScreenMode.Windowed;
                    break;
                default:
                    break;
            }
        }
        public void InitializeVsyncToggle() =>
         VsyncToggle.isOn = GraphicSettings.VSync == 0 ? false : true;
        public void VSyncToggle(bool vsyncOn)
        {
            if (vsyncOn)
                GraphicSettings.VSync = 1;
            else
                GraphicSettings.VSync = 0;
        }
        public void InitializeHDRToggle() =>
         HdrToggle.isOn = GraphicSettings.HDROn;
        public void HDRToggle(bool vsyncOn) =>
            GraphicSettings.HDROn = vsyncOn;
        public void InitializeBrightnessSlider()
        {
            UnityEngine.UI.Slider.SliderEvent sliderBufferEvent = BrightnessSlider.onValueChanged;
            BrightnessSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            BrightnessSlider.value = (float)GraphicSettings.Brightness * 100f;
            BrightnessSlider.onValueChanged = sliderBufferEvent;
        }
        public void BrightnessChange() =>
            GraphicSettings.Brightness = (float)BrightnessSlider.value / 100f;
    }
}