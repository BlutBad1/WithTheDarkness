using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundNS.Ambient
{
    public enum TriggerMode
    {
        None, Enter, Exit, Both
    }
    public class AmbientSoundMuter : MonoBehaviour
    {
        public TriggerMode TriggerMode;
        [HideInInspector]
        public LayerMask TriggerableLayerMask;
        [HideInInspector]
        public float MuteTime = 3f;
        [HideInInspector]
        public float UnmuteTime = 3f;
        [HideInInspector]
        public AudioMixerGroup[] AudioMixerGroups;
        private void OnTriggerEnter(Collider other)
        {
            if ((TriggerMode == TriggerMode.Enter || TriggerMode == TriggerMode.Both) && TriggerableLayerMask == (TriggerableLayerMask | (1 << other.gameObject.layer)))
                MuteGroups();
        }
        private void OnTriggerExit(Collider other)
        {
            if ((TriggerMode == TriggerMode.Exit || TriggerMode == TriggerMode.Both) && TriggerableLayerMask == (TriggerableLayerMask | (1 << other.gameObject.layer)))
                UnmuteGroups();
        }
        public void EnableAutoPlay() =>
             AmbientSoundController.Instance.AutoPlayOn = true;
        public void DisableAutoPlay() =>
             AmbientSoundController.Instance.AutoPlayOn = false;
        public void StopCurrentAmbientSound(float tranTime) =>
            AmbientSoundController.Instance.StopCurrentAmbient(tranTime);
        public void StopCurrentAmbientSound() =>
             StopCurrentAmbientSound(MuteTime);
        public void MuteGroups()
        {
            foreach (AudioMixerGroup group in AudioMixerGroups)
                AmbientSoundController.Instance.MuteAmbientGroup(group, MuteTime);
        }
        public void UnmuteGroups()
        {
            foreach (AudioMixerGroup group in AudioMixerGroups)
                AmbientSoundController.Instance.UnmuteAmbientGroup(group, UnmuteTime);
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(AmbientSoundMuter))]
    public class AmbientSoundMuter_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AmbientSoundMuter script = (AmbientSoundMuter)target;
            SerializedProperty property;
            if (script.TriggerMode != TriggerMode.None)
            {
                property = serializedObject.FindProperty("TriggerableLayerMask");
                EditorGUILayout.PropertyField(property, new GUIContent("TriggerableLayerMask"), true);
            }
            property = serializedObject.FindProperty("MuteTime");
            EditorGUILayout.PropertyField(property, new GUIContent("MuteTime"), true);
            property = serializedObject.FindProperty("UnmuteTime");
            EditorGUILayout.PropertyField(property, new GUIContent("UnmuteTime"), true);
            property = serializedObject.FindProperty("AudioMixerGroups");
            EditorGUILayout.PropertyField(property, new GUIContent("AudioMixerGroups"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}