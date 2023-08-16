using System;
using UnityEngine;

namespace SettingsNS
{
    public class Settings { }
    public class AudioSettings
    {
        public enum AudioKind
        {
            Sound, Music, SFX
        }
        public delegate void VolumeChangeEvent();
        static public VolumeChangeEvent OnVolumeChangeEvent;
        public static float GetVolumeOfType(AudioKind audioType, bool masterScale = true)
        {
            switch (audioType)
            {
                case AudioKind.Sound:
                    return masterScale ? SoundVolume * MasterVolume : SoundVolume;
                case AudioKind.Music:
                    return masterScale ? MusicVolume * MasterVolume : MusicVolume;
                case AudioKind.SFX:
                    return masterScale ? MusicVolume * MasterVolume : MusicVolume;
                default:
                    return 1f;
            }
        }
        public static void SetVolume(AudioKind audioType, float volume)
        {
            switch (audioType)
            {
                case AudioKind.Sound:
                    SoundVolume = volume;
                    break;
                case AudioKind.Music:
                    MusicVolume = volume;
                    break;
                case AudioKind.SFX:
                    MusicVolume = volume;
                    break;
                default:
                    break;
            }
        }
        [Min(0)]
        private static float masterVolume = 1f;
        [Min(0)]
        private static float soundVolume = 1f;
        [Min(0)]
        private static float musicVolume = 1f;
        public static float MasterVolume
        {
            get { return masterVolume; }
            set { masterVolume = value; OnVolumeChangeEvent?.Invoke(); }
        }
        public static float SoundVolume
        {
            get { return soundVolume; }
            set { soundVolume = value; OnVolumeChangeEvent?.Invoke(); }
        }
        public static float MusicVolume
        {
            get { return musicVolume; }
            set { musicVolume = value; OnVolumeChangeEvent?.Invoke(); }
        }
    }
    public class GraphicSettings
    {
        private static Resolution currentResolution;
        private static FullScreenMode fullscreenMode = FullScreenMode.ExclusiveFullScreen;
        [Range(0f, 4f)]
        private static int vSync;
        [Range(0f, 2f)]
        private static float brightness;
        private static bool hdrOn;
        public delegate void HDRChangeEvent();
        static public HDRChangeEvent OnHDRChange;
        public delegate void BrightnessChangeEvent();
        static public BrightnessChangeEvent OnBrightnessChange;
        public static Resolution CurrentResolution
        {
            get { return currentResolution; }
            set { currentResolution = value; Screen.SetResolution(currentResolution.width, currentResolution.height, CurrentFullScreenMode); }
        }
        public static FullScreenMode CurrentFullScreenMode
        {
            get { return fullscreenMode; }
            set { fullscreenMode = value; Screen.fullScreenMode = fullscreenMode; }
        }
        public static int VSync
        {
            get { return vSync; }
            set { vSync = value; QualitySettings.vSyncCount = vSync; }
        }
        public static bool HDROn
        {
            get { return hdrOn; }
            set { hdrOn = value; OnHDRChange?.Invoke(); }
        }
        public static float Brightness
        {
            get { return brightness; }
            set { brightness = value; OnBrightnessChange?.Invoke(); }
        }
    }
    public class GameSettings
    {
        public static PlayerInput PlayerInput = new PlayerInput();
        public delegate void InteracteRebindEvent();
        static public InteracteRebindEvent OnInteracteRebind;
        public delegate void SwitchWeaponOnPickUpChangeEvent();
        static public SwitchWeaponOnPickUpChangeEvent OnSwitchWeaponOnPickUpChange;
        public delegate void KeyRebingDisplayEvent();
        static public KeyRebingDisplayEvent OnKeyRebind;
        private static bool changeWeaponAfterPickup = true;
        private static float xSensitivity = 17f;
        private static float ySensitivity = 17f;
        private static bool xInverse = false;
        private static bool yInverse = false;
        public static bool XInverse
        {
            get { return xInverse; }
            set { xInverse = value; }
        }
        public static bool YInverse
        {
            get { return yInverse; }
            set { yInverse = value; }
        }
        public static bool ChangeWeaponAfterPickup
        {
            get { return changeWeaponAfterPickup; }
            set { changeWeaponAfterPickup = value; OnSwitchWeaponOnPickUpChange?.Invoke(); }
        }
        public static float XSensitivity
        {
            get { return xSensitivity * (XInverse ? -1 : 1); }
            set { xSensitivity = value; }
        }
        public static float YSensitivity
        {
            get { return ySensitivity * (YInverse ? -1 : 1); }
            set { ySensitivity = value; }
        }
    }
}