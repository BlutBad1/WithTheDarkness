using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class SoundSuppressor : MuteSound
    {
        [Tooltip("0 - Sound is full supressed, 1 - Sound is NOT supressed"), Range(0f, 1f)]
        public float CoefficientOfSupressing = 1f;
        public float SupressTime = 1f;
        public float UnSupressTime = 1f;
        public void SupressSound() =>
         SupressSound(CoefficientOfSupressing, SupressTime);
        public void SupressSound(float coefficientOfSupressing) =>
            SupressSound(coefficientOfSupressing, SupressTime);
        public void SupressSound(float coefficientOfSupressing, float supressTime)
        {
            Dictionary<AudioKind, float> newVolume = new Dictionary<AudioKind, float>();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                newVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false) * coefficientOfSupressing);
            ChangeVolumeTo(newVolume, supressTime);
        }
        public void SupressSound(float coefficientOfSupressing, AudioKind audioKind) =>
            SupressSound(coefficientOfSupressing, SupressTime, audioKind);
        public void SupressSound(AudioKind audioKind) =>
         SupressSound(CoefficientOfSupressing, SupressTime, audioKind);
        public void SupressSound(float coefficientOfSupressing, float supressTime, AudioKind audioKind)
        {
            Dictionary<AudioKind, float> newVolume = new Dictionary<AudioKind, float>();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                newVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false) * coefficientOfSupressing);
            if (currentCoroutines.ContainsKey(audioKind))
                StopCoroutine(currentCoroutines[audioKind]);
            currentCoroutines.Add(audioKind, StartCoroutine(ChangeVolumeSmoothly(newVolume, supressTime, audioKind)));
        }
        public void UnSupressSound(float unSupressTime) =>
            VolumeToOriginal(unSupressTime);
        public void UnSupressSound() =>
            VolumeToOriginal(UnSupressTime);
        public void UnSupressSound(AudioKind audioKind) =>
            UnSupressSound(UnSupressTime, audioKind);
        public void UnSupressSound(float unSupressTime, AudioKind audioKind)
        {
            if (currentCoroutines.ContainsKey(audioKind))
                StopCoroutine(currentCoroutines[audioKind]);
            currentCoroutines.Add(audioKind, StartCoroutine(ChangeVolumeSmoothly(originalVolume, unSupressTime, audioKind)));
        }
    }
}