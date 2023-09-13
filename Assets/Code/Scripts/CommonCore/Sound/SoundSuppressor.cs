using System;
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
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                ChangeVolumeTo(GetVolumeOfType((AudioKind)arr[i], false) * coefficientOfSupressing, supressTime, arr[i]);
        }
        public void SupressSound(float coefficientOfSupressing, AudioKind audioKind) =>
            SupressSound(coefficientOfSupressing, SupressTime, audioKind);
        public void SupressSound(AudioKind audioKind) =>
         SupressSound(CoefficientOfSupressing, SupressTime, audioKind);
        public void SupressSound(float coefficientOfSupressing, float supressTime, AudioKind audioKind)
        {
            ChangeVolumeTo(GetVolumeOfType((AudioKind)audioKind, false) * coefficientOfSupressing, supressTime, audioKind);
        }
        public void UnSupressSound(float unSupressTime) =>
            UnMuteVolume(unSupressTime);
        public void UnSupressSound() =>
            UnMuteVolume(UnSupressTime);
        public void UnSupressSound(AudioKind audioKind) =>
            UnSupressSound(UnSupressTime, audioKind);
        public void UnSupressSound(float unSupressTime, AudioKind audioKind) =>
            UnMuteVolume(unSupressTime, audioKind);
    }
}