using AudioConstantsNS;
using MyConstants;
using UnityEngine;
using UnityEngine.Audio;
using UtilitiesNS;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class MixerVolumeChanger : MonoBehaviour
    {
        public static MixerVolumeChanger Instance;
        public AudioMixer AudioMixer;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
        }
        private void Start()
        {
            SettingsNS.AudioSettings.OnVolumeChangeEvent += OnVolumeChange;
            OnVolumeChange();
        }
        private void OnDisable() =>
            SettingsNS.AudioSettings.OnVolumeChangeEvent -= OnVolumeChange;
        public void OnVolumeChange()
        {
            AudioMixer.SetFloat(AudioConstants.MIXER_MASTER_VOLUME, AudioUtilities.FromLinearToLog(SettingsNS.AudioSettings.MasterVolume));
            AudioMixer.SetFloat(AudioConstants.MIXER_SOUND_VOLUME, AudioUtilities.FromLinearToLog(SettingsNS.AudioSettings.SoundVolume));
            AudioMixer.SetFloat(AudioConstants.MIXER_MUSIC_VOLUME, AudioUtilities.FromLinearToLog(SettingsNS.AudioSettings.MusicVolume));
            AudioMixer.SetFloat(AudioConstants.MIXER_SFX_VOLUME, AudioUtilities.FromLinearToLog(SettingsNS.AudioSettings.SFXVolume));
        }
        public AudioMixerGroup GetAudioMixerGroup(AudioKind audioKind)
        {
            switch (audioKind)
            {
                case AudioKind.Sound:
                    return AudioMixer.FindMatchingGroups(AudioConstants.MIXER_SOUND)[0];
                case AudioKind.Music:
                    return AudioMixer.FindMatchingGroups(AudioConstants.MIXER_MUSIC)[0];
                case AudioKind.SFX:
                    return AudioMixer.FindMatchingGroups(AudioConstants.MIXER_SFX)[0];
                default:
                    return null;
            }
        }
    }
}
