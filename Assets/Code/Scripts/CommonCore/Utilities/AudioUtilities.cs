using UnityEngine;
namespace UtilitiesNS
{
    public class AudioUtilities
    {
        public static void CopyAudioSourceSettings(AudioSource original, AudioSource destination)
        {
            destination.bypassEffects = original.bypassEffects;
            destination.bypassListenerEffects = original.bypassListenerEffects;
            destination.bypassReverbZones = original.bypassReverbZones;
            destination.clip = original.clip;
            destination.spread = original.spread;
            destination.dopplerLevel = original.dopplerLevel;
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
        public static float FromLinearToLog(float linearVolume) =>
            Mathf.Log10(Mathf.Clamp(linearVolume, 0.0001f, 1f)) * 20;
        public static float FromLogToLinear(float logVolume) =>
            Mathf.Pow(10, logVolume / 20f);
    }
}