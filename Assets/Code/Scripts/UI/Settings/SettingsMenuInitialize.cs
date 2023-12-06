using DataSaving;
using DifferentUnityMethods;
using MyConstants;
using SerializableTypes;
using SettingsNS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UINS
{
    [System.Serializable]
    public class SettingsData : ISaveData
    {
        ///AUDIO
        public float MasterVolume;
        public float SoundVolume;
        public float MusicVolume;
        ///VIDEO
        public int ResolutionWidth;
        public int ResolutionHeight;
        public FullScreenMode FullScreenMode;
        public int VSync;
        public float Brightness;
        public bool HDROn;
        ///Game
        public float XSensitivity;
        public float YSensitivity;
        public bool XInverse;
        public bool YInverse;
        public bool ChangeWeaponAfterPickup;
        public SerializableDictionary<string, string> Bindings = new SerializableDictionary<string, string>();
        public SettingsData(float masterVolume, float soundVolume, float musicVolume, int resolutionWidth, int resolutionHeight, FullScreenMode fullScreenMode, int vSync, float brightness, bool hdrOn, float xSensitivity, float ySensitivity, bool xInverse, bool yInverse, bool changeWeaponAfterPickup, SerializableDictionary<string, string> bindings)
        {
            MasterVolume = masterVolume;
            SoundVolume = soundVolume;
            MusicVolume = musicVolume;
            ResolutionWidth = resolutionWidth;
            ResolutionHeight = resolutionHeight;
            FullScreenMode = fullScreenMode;
            VSync = vSync;
            Brightness = brightness;
            HDROn = hdrOn;
            XSensitivity = xSensitivity;
            YSensitivity = ySensitivity;
            XInverse = xInverse;
            YInverse = yInverse;
            ChangeWeaponAfterPickup = changeWeaponAfterPickup;
            Bindings = bindings;
        }
    }
    public class SettingsMenuInitialize : MethodsBeforeQuit, IDataPersistence
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
        private SettingsData currentSavedSettings;
        public SettingsData CurrentSavedSettings
        {
            get { return currentSavedSettings; }
            protected set { currentSavedSettings = value; }
        }
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                if (!SettingsMenuGameObject)
                    SettingsMenuGameObject = this.gameObject;
                LoadData();
                InitializeSettings();
            }
            else
                Destroy(this);
        }
        public void LoadData()
        {
            FileDataHandler fileDataHandler = new FileDataHandler(Application.persistentDataPath, DataConstants.SETTINGS_DATA_PATH, false);
            CurrentSavedSettings = fileDataHandler.Load<SettingsData>();
            if (CurrentSavedSettings != null)
            {
                SettingsNS.AudioSettings.MasterVolume = CurrentSavedSettings.MasterVolume;
                SettingsNS.AudioSettings.SoundVolume = CurrentSavedSettings.SoundVolume;
                SettingsNS.AudioSettings.MusicVolume = CurrentSavedSettings.MusicVolume;
                Resolution resolution = new Resolution();
                resolution.width = CurrentSavedSettings.ResolutionWidth;
                resolution.height = CurrentSavedSettings.ResolutionHeight;
                GraphicSettings.CurrentResolution = resolution;
                GraphicSettings.CurrentFullScreenMode = CurrentSavedSettings.FullScreenMode;
                GraphicSettings.Brightness = CurrentSavedSettings.Brightness;
                GraphicSettings.HDROn = CurrentSavedSettings.HDROn;
                GraphicSettings.VSync = CurrentSavedSettings.VSync;
                GameSettings.XSensitivity = CurrentSavedSettings.XSensitivity;
                GameSettings.YSensitivity = CurrentSavedSettings.YSensitivity;
                GameSettings.ChangeWeaponAfterPickup = CurrentSavedSettings.ChangeWeaponAfterPickup;
                GameSettings.XInverse = CurrentSavedSettings.XInverse;
                GameSettings.YInverse = CurrentSavedSettings.YInverse;
                var overrides = CurrentSavedSettings.Bindings;
                foreach (var map in GameSettings.PlayerInput.asset.actionMaps)
                {
                    var bindings = map.bindings;
                    for (var i = 0; i < bindings.Count; ++i)
                    {
                        if (overrides.TryGetValue(bindings[i].id.ToString(), out var overridePath))
                            map.ApplyBindingOverride(i, new InputBinding { overridePath = overridePath });
                    }
                }
            }
            else
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
        public void SaveData()
        {
            var bindings = new SerializableDictionary<string, string>();
            foreach (var map in GameSettings.PlayerInput.asset.actionMaps)
                foreach (var binding in map.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.overridePath))
                        bindings[binding.id.ToString()] = binding.overridePath;
                }
            CurrentSavedSettings = new SettingsData(SettingsNS.AudioSettings.MasterVolume, SettingsNS.AudioSettings.SoundVolume,
            SettingsNS.AudioSettings.MusicVolume, GraphicSettings.CurrentResolution.width, GraphicSettings.CurrentResolution.height, GraphicSettings.CurrentFullScreenMode, GraphicSettings.VSync,
            GraphicSettings.Brightness, GraphicSettings.HDROn, Mathf.Abs(GameSettings.XSensitivity), Mathf.Abs(GameSettings.YSensitivity), GameSettings.XInverse,
            GameSettings.YInverse, GameSettings.ChangeWeaponAfterPickup, bindings);
            FileDataHandler fileDataHandler = new FileDataHandler(Application.persistentDataPath, DataConstants.SETTINGS_DATA_PATH, false);
            fileDataHandler.Save(CurrentSavedSettings);
        }
        public override void OnDisableBeforeQuit()
        {
            SaveData();
        }
        public new void OnDestroy()
        {
            base.OnDestroy();
            SaveData();
        }
        public void InitializeSettings()
        {
            InitializeVolumeSettings();
            InitializeVideoSettings();
            InitializeGameSettings();
        }
        public void InitializeVolumeSettings()
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