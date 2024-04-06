using SettingsNS;
using System.Collections.Generic;
using UnityEngine;

namespace SoundNS
{
	public class AudioObject
	{
		private AudioSource audioSource;
		private float startedVolume;
		public AudioSource AudioSource
		{
			get { return audioSource; }
			set { if (audioSource) value.volume = audioSource.volume; audioSource = value; }
		}
		public float StartedVolume { get => startedVolume; set => startedVolume = value; }
		public AudioObject(AudioSource audioSource, float volume)
		{
			this.AudioSource = audioSource;
			this.StartedVolume = volume;
			this.AudioSource.volume = volume;
		}
	}
	public class AudioSetup : MonoBehaviour
	{
		public bool PauseSourcesInGameMenu = true;

		protected List<AudioObject> audioObjects;

		protected void Awake()
		{
			audioObjects = new List<AudioObject>();
			AttachMethodsToEvents();
		}
		private void OnDestroy() =>
			DetachMethodsFromEvents();
		public void AttachMethodsToEvents()
		{
			InGameMenu.OnGameMenuOpenEvent += OnGameMenuOpen;
			InGameMenu.OnGameMenuCloseEvent += OnGameMenuClose;
		}
		public void DetachMethodsFromEvents()
		{
			InGameMenu.OnGameMenuOpenEvent -= OnGameMenuOpen;
			InGameMenu.OnGameMenuCloseEvent -= OnGameMenuClose;
		}
		private void OnGameMenuOpen()
		{
			if (PauseSourcesInGameMenu && audioObjects != null)
			{
				audioObjects.RemoveAll(x => x == null || !x.AudioSource);
				foreach (var source in audioObjects)
					source.AudioSource?.Pause();
			}
		}
		private void OnGameMenuClose()
		{
			if (PauseSourcesInGameMenu && audioObjects != null)
			{
				foreach (var source in audioObjects)
					source.AudioSource?.UnPause();
			}
		}
	}
}