using SettingsNS;
using UnityEngine;
using UnityEngine.UI;

namespace UINS
{
    public class SettingsMenuInitialize : MonoBehaviour
    {
        public GameObject SettingsMenuGameObject;
        public static SettingsMenuInitialize instance;
        [Header("Volume Settings")]
        public Slider MasterVolumeSlider;
        public Slider SoundVolumeSlider;
        public Slider MusicVolumeSlider;
        [Header("Video Settings")]
        public VideoSettings VideoSettings;
        [Header("Game Settings")]
        public GameSettingsUI GameSettingsUI;
        private void Start()
        {
            if (!instance)
                instance = this;
            else
                Destroy(this);
            if (!SettingsMenuGameObject)
                SettingsMenuGameObject = this.gameObject;
            InitializeSettingsParameters();
            InitializeSettings();
        }
        public void InitializeSettingsParameters()
        {
            if (false)//NOTE:Read settings file and getting settings parametres from there 
                return;
            else//If can't read
            {
                SettingsNS.AudioSettings.MasterVolume = 1f;
                SettingsNS.AudioSettings.SoundVolume = 1f;
                SettingsNS.AudioSettings.MusicVolume = 1f;
                GraphicSettings.CurrentResolution = Screen.currentResolution;
                GraphicSettings.CurrentFullScreenMode = FullScreenMode.ExclusiveFullScreen;
                GraphicSettings.Brightness = 1f;
                GraphicSettings.HDROn = true;
                GraphicSettings.VSync = 1;
                GameSettings.XSensitivity = 17f;
                GameSettings.YSensitivity = 17f;
                GameSettings.ChangeWeaponAfterPickup = true;
                GameSettings.XInverse = false;
                GameSettings.YInverse = false;
            }
        }
        public void InitializeSettings()
        {
            InitializeVolumeSettings();
            InitializeVideoSettings();
            InitializeGameSettings();
        }
        public void InitializeVolumeSettings()
        {
            MasterVolumeSlider.value = SettingsNS.AudioSettings.MasterVolume * 100f;
            SoundVolumeSlider.value = SettingsNS.AudioSettings.SoundVolume * 100f;
            MusicVolumeSlider.value = SettingsNS.AudioSettings.MusicVolume * 100f;
        }
        public void InitializeVideoSettings()
        {
            VideoSettings.CurrentRefreshRate = Screen.currentResolution.refreshRate;
            VideoSettings.InitializeFullscreenModeDropdown();
            VideoSettings.InitializeResolutionDropdown();
            VideoSettings.InitializeVsyncToggle();
            VideoSettings.InitializeHDRToggle();
            VideoSettings.InitializeBrightnessSlider();
        }
        public void InitializeGameSettings()
        {
            GameSettingsUI.InitializeSensitivitySliders();
            GameSettingsUI.InitializeToggles();
        }
    }
}