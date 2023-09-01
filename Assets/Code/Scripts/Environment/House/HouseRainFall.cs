using SoundNS;
using UnityEditor;
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
        public float TransitionOutChangeClipTime = 0.1f;
        public float TransitionInChangeClipTime = 0.1f;
        public float TransitionThunderVolumeTime = 0.1f;
        public bool IsWindowOpened = false;
        public bool EnableOnStart = true;
        [HideInInspector]
        public float TransitionInTimeOnStart = 0.1f;
        [HideInInspector]
        public float TransitionOutOnStart = 0.1f;
        private float timeToThunder = 0f;
        private bool isEnabled = false;
        private void Start()
        {
            if (EnableOnStart)
                ChangeRainAudioType(IsWindowOpened, TransitionInTimeOnStart, TransitionOutOnStart);
        }
        public void OpenWindow()
        {
            if (isEnabled)
                ChangeRainAudioType(true, TransitionInChangeClipTime, TransitionOutChangeClipTime);
            ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionOutChangeClipTime);
            isEnabled = true;
        }
        public void CloseWindow()
        {
            if (isEnabled)
                ChangeRainAudioType(false, TransitionInChangeClipTime, TransitionOutChangeClipTime);
            ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionOutChangeClipTime);
            isEnabled = true;
        }
        public void ChangeRainAudioType(bool WindowStatus, float TransitionInChangeClipTime, float TransitionOutChangeClipTime)
        {
            IsWindowOpened = WindowStatus;
            Sound s = IsWindowOpened ? RainOutsideSounds[Random.Range(0, RainOutsideSounds.Length)] : RainInsideSounds[Random.Range(0, RainInsideSounds.Length)];
            if (IsWindowOpened)
            {
                RainInsideAudioSourceManager.StopAudioSourceSmoothly(TransitionStopTime);
                RainOutsideAudioSourceManager.ChangeSound(s, TransitionOutChangeClipTime, TransitionInChangeClipTime, true);
            }
            else
            {
                RainOutsideAudioSourceManager.StopAudioSourceSmoothly(TransitionStopTime);
                RainInsideAudioSourceManager.ChangeSound(s, TransitionOutChangeClipTime, TransitionInChangeClipTime, true);
            }
        }
        private void Update()
        {
            if (timeToThunder >= ThunderTimer)
            {
                timeToThunder = 0f;
                if (ThunderChance > Random.Range(0, 100))
                {
                    ThunderAudioSourceManager.ChangeSound(ThunderSounds[Random.Range(0, ThunderSounds.Length)], TransitionOutChangeClipTime, TransitionInChangeClipTime, true);
                    ThunderAudioSourceManager.ChangeAudioSourceVolumeSmoothly(IsWindowOpened ? 1f : 0.7f, TransitionThunderVolumeTime);
                }
            }
            timeToThunder += Time.deltaTime;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(HouseRainFall))]
    public class HouseRainFall_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            HouseRainFall script = (HouseRainFall)target;
            // draw checkbox for the bool
            script.EnableOnStart = EditorGUILayout.Toggle("Enable On Start", script.EnableOnStart);
            if (script.EnableOnStart) // if bool is true, show other fields
            {
                script.TransitionInTimeOnStart = EditorGUILayout.FloatField("Transition In Time On Start", script.TransitionInTimeOnStart);
                script.TransitionOutOnStart = EditorGUILayout.FloatField("Transition Out On Start", script.TransitionOutOnStart);
            }
        }
    }
#endif
}
