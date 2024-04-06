using InputNS;
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

		[Min(0)]
		private static float masterVolume = 1f;
		[Min(0)]
		private static float soundVolume = 1f;
		[Min(0)]
		private static float musicVolume = 1f;
		[Min(0)]
		private static float sfxVolume = 1f;

		public delegate void VolumeChangeEvent();

		static public VolumeChangeEvent OnVolumeChangeEvent;

		public static float MasterVolume
		{
			get { return masterVolume; }
			set { masterVolume = Mathf.Clamp01(value); OnVolumeChangeEvent?.Invoke(); }
		}
		public static float SoundVolume
		{
			get { return soundVolume; }
			set { soundVolume = Mathf.Clamp01(value); OnVolumeChangeEvent?.Invoke(); }
		}
		public static float MusicVolume
		{
			get { return musicVolume; }
			set { musicVolume = Mathf.Clamp01(value); OnVolumeChangeEvent?.Invoke(); }
		}
		public static float SFXVolume
		{
			get { return sfxVolume; }
			set { sfxVolume = Mathf.Clamp01(value); OnVolumeChangeEvent?.Invoke(); }
		}

		public static float GetVolumeOfType(AudioKind audioType)
		{
			switch (audioType)
			{
				case AudioKind.Sound:
					return SoundVolume;
				case AudioKind.Music:
					return MusicVolume;
				case AudioKind.SFX:
					return SFXVolume;
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
					SFXVolume = volume;
					break;
				default:
					break;
			}
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
		public delegate void BrightnessChangeEvent();

		public static HDRChangeEvent OnHDRChange;
		public static BrightnessChangeEvent OnBrightnessChange;

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
		private static PlayerInput playerInput = GetPlayerInput.GetInput();
		private static bool changeWeaponAfterPickup = true;
		private static bool xInverse = false;
		private static bool yInverse = false;

		public delegate void InteracteRebindEvent();
		public delegate void SwitchWeaponOnPickUpChangeEvent();
		public delegate void KeyRebingDisplayEvent();

		public static InteracteRebindEvent OnInteracteRebind;
		public static SwitchWeaponOnPickUpChangeEvent OnSwitchWeaponOnPickUpChange;
		public static KeyRebingDisplayEvent OnKeyRebind;

		public static bool XInverse
		{
			get { return xInverse; }
			set
			{
				xInverse = value;
				XSensitivity = xInverse ? -Math.Abs(XSensitivity) : Math.Abs(XSensitivity);
			}
		}
		public static bool YInverse
		{
			get { return yInverse; }
			set
			{
				yInverse = value;
				YSensitivity = yInverse ? -Math.Abs(YSensitivity) : Math.Abs(YSensitivity);
			}
		}
		public static bool ChangeWeaponAfterPickup
		{
			get { return changeWeaponAfterPickup; }
			set { changeWeaponAfterPickup = value; OnSwitchWeaponOnPickUpChange?.Invoke(); }
		}
		public static float XSensitivity
		{
			get { return MouseSensivity.XSensitivity; }
			set { MouseSensivity.XSensitivity = (int)value; }
		}
		public static float YSensitivity
		{
			get { return MouseSensivity.YSensitivity; }
			set { MouseSensivity.YSensitivity = (int)value; }
		}
		public static PlayerInput PlayerInput { get => playerInput; }
	}
	public class InGameMenu
	{
		private static bool isInGameMenuOpened = false;

		public static Action OnGameMenuOpenEvent;
		public static Action OnGameMenuCloseEvent;

		public static bool IsInGameMenuOpened
		{
			get { return isInGameMenuOpened; }
			set { isInGameMenuOpened = value; }
		}
	}
}