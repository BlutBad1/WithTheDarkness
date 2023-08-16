using System;
using System.Collections;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    [System.Serializable]
    public class AudioSourceObject 
    {
        public string Name;
        public AudioSource AudioSource;
        public AudioKind AudioKind;
        //public override AudioKind AudioType { get => AudioKind; set => AudioKind = value; }
        //private new void Awake()
        //{
        //    base.Awake();
        //    availableSources.Add(AudioSource, AudioSource.volume);
        //}
        //public void ReplaceKey(AudioSource audioSource)
        //{
        //    availableSources.Remove(AudioSource);
        //    AudioSource = audioSource;
        //    availableSources.Add(AudioSource, AudioSource.volume);
        //}
    }
    public class AudioSourcesManager : MonoBehaviour
    {
        public AudioSourceObject[] AudioSourceObjects;
        public void PlayAudioSource(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            audioSourceObject.AudioSource.Play();
        }
        public void PlayAudioSourceOnceAtTime(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            if (!audioSourceObject.AudioSource.isPlaying)
                audioSourceObject.AudioSource.Play();
        }
        public void StopAudioSource(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            audioSourceObject.AudioSource.Stop();
        }
        public void CreateNewAudioSourceAndPlay(string AudioSourceObjectName)
        {
            AudioSourceObject audioSourceObject = Array.Find(AudioSourceObjects, clip => clip.Name == AudioSourceObjectName);
            if (audioSourceObject == null)
            {
#if UNITY_EDITOR
                Debug.Log($"AudioSourceObject \"{audioSourceObject.Name}\" is not found!");
#endif
                return;
            }
            CreateNewAudioSourceAndPlay(audioSourceObject);
        }
        public void CreateNewAudioSourceAndPlay(AudioSourceObject audioSourceObject)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            CopyAudioSourceSettings(audioSourceObject.AudioSource, newAudioSource);
            if (audioSourceObject.AudioSource)
                StartCoroutine(DeleteAudioSourceAfterPlaying(audioSourceObject.AudioSource));
       //     audioSourceObject.ReplaceKey(newAudioSource);
            audioSourceObject.AudioSource.Play();
        }
        public void CopyAudioSourceSettings(AudioSource original, AudioSource destination)
        {
            destination.bypassEffects = original.bypassEffects;
            destination.bypassListenerEffects = original.bypassListenerEffects;
            destination.bypassReverbZones = original.bypassReverbZones;
            destination.clip = original.clip;
            destination.spread = original.spread;
            destination.dopplerLevel = original.dopplerLevel;
            destination.gamepadSpeakerOutputType = original.gamepadSpeakerOutputType;
            destination.ignoreListenerPause = original.ignoreListenerPause;
            destination.ignoreListenerVolume = original.ignoreListenerVolume;
            destination.loop = original.loop;
            destination.maxDistance = original.maxDistance;
            destination.minDistance = original.minDistance;
            destination.pitch = original.pitch;
            destination.mute = original.mute;
            destination.outputAudioMixerGroup = original.outputAudioMixerGroup;
            destination.panStereo = original.panStereo;
            destination.playOnAwake = original.playOnAwake;
            destination.priority = original.priority;
            destination.reverbZoneMix = original.reverbZoneMix;
            destination.rolloffMode = original.rolloffMode;
            destination.spatialBlend = original.spatialBlend;
            destination.spatialize = original.spatialize;
            destination.spatializePostEffects = original.spatializePostEffects;
            destination.spread = original.spread;
            destination.volume = original.volume;
            destination.SetCustomCurve(AudioSourceCurveType.Spread, original.GetCustomCurve(AudioSourceCurveType.Spread));
            destination.SetCustomCurve(AudioSourceCurveType.CustomRolloff, original.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            destination.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, original.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            destination.SetCustomCurve(AudioSourceCurveType.SpatialBlend, original.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
        }
        IEnumerator DeleteAudioSourceAfterPlaying(AudioSource source)
        {
            while (source.isPlaying)
                yield return null;
            Destroy(source);
        }
    }
}