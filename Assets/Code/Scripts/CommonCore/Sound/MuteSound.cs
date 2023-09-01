using SettingsNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class MuteSound : MonoBehaviour
    {
        [EnumMask]
        public AudioKind AudioKind;
        //public AudioKind AudioKind;
        protected Dictionary<AudioKind, float> originalVolume = new Dictionary<AudioKind, float>();
        Dictionary<AudioKind, float> changedVolume = new Dictionary<AudioKind, float>();
        protected Dictionary<AudioKind, Coroutine> currentCoroutines = new Dictionary<AudioKind, Coroutine>();
        private void OnDisable()
        {
            DetachMethodsFromEvents();
            StopCoroutines();
            SoundVolumeToOriginal();
        }
        private void Start()
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                originalVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false));
            AttachMethodsToEvents();
        }
        public void DetachMethodsFromEvents()
        {
            InGameMenu.OnGameMenuOpenEvent -= GameMenuOpenEvent;
            InGameMenu.OnGameMenuCloseEvent -= GameMenuCloseEvent;
            SettingsNS.AudioSettings.OnVolumeChangeEvent -= ReadOriginalVolume;
        }
        public void AttachMethodsToEvents()
        {
            InGameMenu.OnGameMenuOpenEvent += GameMenuOpenEvent;
            InGameMenu.OnGameMenuCloseEvent += GameMenuCloseEvent;
            SettingsNS.AudioSettings.OnVolumeChangeEvent += ReadOriginalVolume;
        }
        void StopCoroutines()
        {
            if (currentCoroutines.Count != 0)
            {
                var coroutineArr = currentCoroutines.Values.ToArray();
                for (int i = 0; i < coroutineArr.Length; i++)
                {
                    if (coroutineArr[i] != null)
                        StopCoroutine(coroutineArr[i]);
                }
                currentCoroutines.Clear();
            }
        }
        void GameMenuOpenEvent()
        {
            changedVolume.Clear();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                changedVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false));
            SoundVolumeToOriginal();
        }
        void GameMenuCloseEvent()
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                if (!currentCoroutines.ContainsKey(arr[i]))
                    SetVolume(arr[i], changedVolume[arr[i]]);
            }
        }
        protected void SoundVolumeToOriginal()
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                SetVolume(arr[i], originalVolume[arr[i]]);
        }
        /// <summary>
        ///  Reads original volumes to variables, but not changes them
        /// </summary>
        public void ReadOriginalVolume()
        {
            if (InGameMenu.IsInGameMenuOpened)
            {
                originalVolume.Clear();
                var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
                for (int i = 0; i < arr.Count; i++)
                    originalVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false));
            }
        }
        public void MuteVolume(float transitionMuteTime)
        {
            Dictionary<AudioKind, float> zeroVolume = new Dictionary<AudioKind, float>();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                zeroVolume.Add(arr[i], 0);
            ChangeVolumeTo(zeroVolume, transitionMuteTime);
        }
        public void VolumeToOriginal(float transitionUnMuteTime) =>
            ChangeVolumeTo(originalVolume, transitionUnMuteTime);
        protected void ChangeVolumeTo(Dictionary<AudioKind, float> wantedVolume, float transitionTime)
        {
            StopCoroutines();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                var value = ((int)AudioKind & (1 << i)) != 0;
                if (value)
                    currentCoroutines.Add(arr[i], StartCoroutine(ChangeVolumeSmoothly(wantedVolume, transitionTime, (AudioKind)arr[i])));
            }
        }
        protected IEnumerator ChangeVolumeSmoothly(Dictionary<AudioKind, float> wantedVolume, float transitionTime, AudioKind AudioKind)
        {
            float percentage = 0;
            while (Mathf.Abs(GetVolumeOfType(AudioKind, false) - wantedVolume[AudioKind]) != 0)
            {
                if (!InGameMenu.IsInGameMenuOpened)
                {
                    SetVolume(AudioKind, Mathf.Lerp(GetVolumeOfType(AudioKind, false), wantedVolume[AudioKind], percentage));
                    percentage += Time.deltaTime / transitionTime;
                    if (Mathf.Abs(GetVolumeOfType(AudioKind, false) - wantedVolume[AudioKind]) <= 0.001f)
                        SetVolume(AudioKind, wantedVolume[AudioKind]);
                }
                yield return null;
            }
            if (currentCoroutines.ContainsKey(AudioKind))
                currentCoroutines.Remove(AudioKind);
        }
    }
}

