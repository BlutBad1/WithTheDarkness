using SoundNS;
using UnityEngine;

namespace UIControlling
{
    public class PlayUISounds : MonoBehaviour
    {
        AudioManager audioManager;
        private void Start()
        {
            GameObject.Find(MyConstants.UIConstants.UI_SOUNDS).TryGetComponent(out audioManager);
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
          PlaySoundOnceAtTime(MyConstants.UIConstants.UISoundsNameConstants.BUTTON_HOVER_SOUND);
        public void PlayButtonClick() =>
            PlaySoundOnceAtTime(MyConstants.UIConstants.UISoundsNameConstants.BUTTON_CLICK_SOUND);
        public void PlayButtonHoverHigher() =>
        PlaySoundOnceAtTime(MyConstants.UIConstants.UISoundsNameConstants.BUTTON_HOVER_SOUND_HIGHER);
        public void PlayButtonClickHigher() =>
            PlaySoundOnceAtTime(MyConstants.UIConstants.UISoundsNameConstants.BUTTON_CLICK_SOUND_HIGHER);
        public void PlaySliderChange() =>
            PlaySoundOnceAtTime(MyConstants.UIConstants.UISoundsNameConstants.SLIDER_CHANGE);
    }
}