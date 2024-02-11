using SoundNS;
using UIConstantsNS;
using UnityEngine;

namespace UIControlling
{
    public class PlayUISounds : MonoBehaviour
    {
        private AudioSourcesManager audioSourcesManager;
        private void Start()
        {
            GameObject.Find(UISoundsNameConstants.UI_SOUNDS).TryGetComponent(out audioSourcesManager);
        }
        public void PlaySound(string soundName)
        {
            if (gameObject.activeInHierarchy)
                audioSourcesManager?.PlayAudioSource(soundName);
        }
        public void PlaySoundOnceAtTime(string soundName)
        {
            if (gameObject.activeInHierarchy)
                audioSourcesManager?.PlayAudioSourceOnceAtTime(soundName);
        }
        public void CreateAndPlaySound(string soundName) =>
        audioSourcesManager.CreateNewAudioSourceAndPlay(soundName);
        public void PlayButtonHover() =>
          PlaySoundOnceAtTime(UISoundsNameConstants.BUTTON_HOVER_SOUND);
        public void PlayButtonClick() =>
            PlaySoundOnceAtTime(UISoundsNameConstants.BUTTON_CLICK_SOUND);
        public void PlayButtonHoverHigher() =>
        PlaySoundOnceAtTime(UISoundsNameConstants.BUTTON_HOVER_SOUND_HIGHER);
        public void PlayButtonClickHigher() =>
            PlaySoundOnceAtTime(UISoundsNameConstants.BUTTON_CLICK_SOUND_HIGHER);
        public void PlaySliderChange() =>
            PlaySoundOnceAtTime(UISoundsNameConstants.SLIDER_CHANGE);
    }
}