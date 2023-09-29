using MyConstants.UIConstants;
using SoundNS;
using UnityEngine;

namespace UIControlling
{
    public class PlayUISounds : MonoBehaviour
    {
        AudioManager audioManager;
        private void Start()
        {
            GameObject.Find(MainUIConstants.UI_SOUNDS).TryGetComponent(out audioManager);
        }
        public void PlaySound(string soundName)
        {
            if (gameObject.activeInHierarchy)
                audioManager?.Play(soundName);
        }
        public void PlaySoundOnceAtTime(string soundName)
        {
            if (gameObject.activeInHierarchy)
                audioManager?.PlayOnceAtTime(soundName);
        }
        public void CreateAndPlaySound(string soundName) =>
        audioManager.CreateAndPlay(soundName);
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