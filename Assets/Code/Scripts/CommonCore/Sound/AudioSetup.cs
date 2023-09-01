using SettingsNS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class AudioObject
    {
        private AudioSource audioSource;
        private float startedVolume;
        private AudioKind audioType;
        public AudioKind AudioType
        {
            get { return audioType; }
            set { audioType = value; ChangeVolumeBySettings(); }
        }
        public AudioSource AudioSource
        {
            get { return audioSource; }
        }
        public void ChangeAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            ChangeVolumeBySettings();
        }
        public void ChangeAudioSourceVolume(float volume)
        {
            startedVolume = volume;
            ChangeVolumeBySettings();
        }
        public void ChangeStartedVolume(float volume) =>
               startedVolume = volume;
        public AudioObject(AudioSource audioSource, float startedVolume, AudioKind audioType)
        {
            this.audioSource = audioSource;
            this.startedVolume = startedVolume;
            this.audioType = audioType;
        }
        public void ChangeVolumeBySettings() =>
            audioSource.volume = startedVolume * SettingsNS.AudioSettings.GetVolumeOfType(audioType);
    }
    public class AudioSetup : MonoBehaviour
    {
        public bool PauseSourcesInGameMenu = true;
        static protected List<AudioObject> availableSources = new List<AudioObject>();
        protected void Awake()
        {
            AttachMethodsToEvents();
        }
        private void OnDestroy()
        {
            DetachMethodsFromEvents();
        }
        public void DetachMethodsFromEvents()
        {
            SettingsNS.AudioSettings.OnVolumeChangeEvent -= VolumeChange;
            InGameMenu.OnGameMenuOpenEvent -= OnGameMenuOpen;
            InGameMenu.OnGameMenuCloseEvent -= OnGameMenuClose;
        }
        public void AttachMethodsToEvents()
        {
            SettingsNS.AudioSettings.OnVolumeChangeEvent += VolumeChange;
            InGameMenu.OnGameMenuOpenEvent += OnGameMenuOpen;
            InGameMenu.OnGameMenuCloseEvent += OnGameMenuClose;
        }
        public virtual void OnGameMenuOpen()
        {
            if (PauseSourcesInGameMenu)
            {
                foreach (var s in availableSources.Where(kv => !kv.AudioSource).ToList())
                    availableSources.Remove(s);
                foreach (var source in availableSources)
                    source.AudioSource?.Pause();
            }
        }
        public virtual void OnGameMenuClose()
        {
            if (PauseSourcesInGameMenu)
            {
                foreach (var source in availableSources)
                    source.AudioSource?.UnPause();
            }
        }
        public void VolumeChange()
        {
            foreach (var s in availableSources.Where(kv => !kv.AudioSource).ToList())
                availableSources.Remove(s);
            foreach (var item in availableSources)
                item.ChangeVolumeBySettings();
        }
    }
}