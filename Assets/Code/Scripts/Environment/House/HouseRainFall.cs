using SoundNS;
using UnityEngine;

namespace EnvironmentNS
{
    public class HouseRainFall : MonoBehaviour
    {
        public Sound[] RainInsideSounds;
        public Sound[] RainOutsideSounds;
        public Sound[] ThunderSounds;
        public AudioSourceManager RainInsideAudioSourceManager;
        public AudioSourceManager RainOutsideAudioSourceManager;
        public AudioSourceManager ThunderAudioSourceManager;
        public float ThunderTimer = 10f;
        [Range(0, 100)]
        public int ThunderChance = 10;
        public float TransitionStopTime = 0.1f;
        public float TransitionChangeClipTime = 0.1f;
        public bool IsWindowOpened = false;
        private float timeToThunder = 0f;
        private void Start() =>
            ChangeRainAudioType(IsWindowOpened);
        public void OpenWindow()
        {
            ChangeRainAudioType(IsWindowOpened = true);
            ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionChangeClipTime);
        }
        public void CloseWindow()
        {
            ChangeRainAudioType(IsWindowOpened = false);
            ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionChangeClipTime);
        }
        public void ChangeRainAudioType(bool WindowStatus)
        {
            IsWindowOpened = WindowStatus;
            Sound s = IsWindowOpened ? RainOutsideSounds[Random.Range(0, RainOutsideSounds.Length)] : RainInsideSounds[Random.Range(0, RainInsideSounds.Length)];
            if (IsWindowOpened)
            {
                RainInsideAudioSourceManager.StopAudioSourceSmoothly(TransitionStopTime);
                RainOutsideAudioSourceManager.ChangeSound(s, TransitionChangeClipTime, true);
            }
            else
            {
                RainOutsideAudioSourceManager.StopAudioSourceSmoothly(TransitionStopTime);
                RainInsideAudioSourceManager.ChangeSound(s, TransitionChangeClipTime, true);
            }
        }
        private void Update()
        {
            if (timeToThunder >= ThunderTimer)
            {
                timeToThunder = 0f;
                if (ThunderChance > Random.Range(0, 100))
                {
                    ThunderAudioSourceManager.ChangeSound(ThunderSounds[Random.Range(0, ThunderSounds.Length)], TransitionChangeClipTime, true);
                    ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionChangeClipTime);
                }
            }
            timeToThunder += Time.deltaTime;
        }
    }
}
