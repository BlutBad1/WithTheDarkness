using SettingsNS;
using System.Collections.Generic;
using System.Linq;
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
        static protected List<AudioObject> audioObjects = new List<AudioObject>();
        protected void Awake() =>
            AttachMethodsToEvents();
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
        protected virtual void OnGameMenuOpen()
        {
            if (PauseSourcesInGameMenu)
            {
                foreach (var s in audioObjects.Where(kv => !kv.AudioSource).ToList())
                    audioObjects.Remove(s);
                foreach (var source in audioObjects)
                    source.AudioSource?.Pause();
            }
        }
        protected virtual void OnGameMenuClose()
        {
            if (PauseSourcesInGameMenu)
            {
                foreach (var source in audioObjects)
                    source.AudioSource?.UnPause();
            }
        }
    }
}