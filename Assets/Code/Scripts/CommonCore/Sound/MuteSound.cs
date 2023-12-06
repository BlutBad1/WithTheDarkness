using DifferentUnityMethods;
using SettingsNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class MuteSound : MethodsBeforeQuit
    {
        [EnumMask]
        public AudioKind AudioKind;
        protected Dictionary<AudioKind, float> originalVolume = new Dictionary<AudioKind, float>();
        protected Dictionary<AudioKind, Coroutine> currentCoroutines = new Dictionary<AudioKind, Coroutine>();
        private Dictionary<AudioKind, float> changedVolume = new Dictionary<AudioKind, float>();
        private HashSet<AudioKind> changedKinds = new HashSet<AudioKind>();
        private void Start()
        {
            ReadOriginalVolume();
            AttachMethodsToEvents();
        }
        public override void OnDestroyBeforeQuit()
        {
            StopCoroutines();
            DetachMethodsFromEvents();
            SoundVolumeToOriginal();
        }
        public override void OnDisableBeforeQuit()
        {
            StopCoroutines();
            DetachMethodsFromEvents();
            SoundVolumeToOriginal();
        }
        public new void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            StopCoroutines();
            DetachMethodsFromEvents();
            SoundVolumeToOriginal();
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
        /// <summary>
        ///  Reads original volumes to variables, but not changes them
        /// </summary>
        public void ReadOriginalVolume()
        {
            if (InGameMenu.IsInGameMenuOpened || currentCoroutines.Count == 0)
            {
                originalVolume.Clear();
                var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
                for (int i = 0; i < arr.Count; i++)
                    originalVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false));
            }
        }
        public void MuteVolume(float transitionMuteTime)
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                var value = ((int)AudioKind & (1 << i)) != 0;
                if (value)
                {
                    ChangeVolumeTo(0, transitionMuteTime, arr[i]);
                    changedKinds.Add(arr[i]);
                }
            }
        }
        public void UnMuteVolume(float transitionMuteTime)
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                UnMuteVolume(transitionMuteTime, arr[i]);
        }
        public void UnMuteVolume(float transitionMuteTime, AudioKind audioKind)
        {
            ChangeVolumeTo(originalVolume[audioKind], transitionMuteTime, audioKind);
            changedKinds.Remove(audioKind);
        }
        protected void ChangeVolumeTo(float wantedVolume, float transitionTime, AudioKind AudioKind)
        {
            if (currentCoroutines.ContainsKey(AudioKind))
            {
                StopCoroutine(currentCoroutines[AudioKind]);
                currentCoroutines.Remove(AudioKind);
            }
            currentCoroutines.Add(AudioKind, StartCoroutine(ChangeVolumeSmoothly(wantedVolume, transitionTime, AudioKind)));
        }
        private void StopCoroutines()
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
        private void GameMenuOpenEvent()
        {
            changedVolume.Clear();
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                if (changedKinds.Contains(arr[i]))
                    changedVolume.Add(arr[i], GetVolumeOfType((AudioKind)arr[i], false));
            }
            SoundVolumeToOriginal();
        }
        private void GameMenuCloseEvent()
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                if (!currentCoroutines.ContainsKey(arr[i]) && changedKinds.Contains(arr[i]) && changedVolume.Keys.Contains(arr[i]))
                    SetVolume(arr[i], changedVolume[arr[i]]);
            }
        }
        private void SoundVolumeToOriginal()
        {
            var arr = Enum.GetValues(typeof(AudioKind)).Cast<AudioKind>().ToList();
            for (int i = 0; i < arr.Count; i++)
                SetVolume(arr[i], originalVolume[arr[i]]);
        }
        private IEnumerator ChangeVolumeSmoothly(float wantedVolume, float transitionTime, AudioKind AudioKind)
        {
            float percentage = 0;
            while (Mathf.Abs(GetVolumeOfType(AudioKind, false) - wantedVolume) != 0)
            {
                if (!InGameMenu.IsInGameMenuOpened)
                {
                    SetVolume(AudioKind, Mathf.Lerp(GetVolumeOfType(AudioKind, false), wantedVolume, percentage));
                    percentage += Time.deltaTime / transitionTime;
                    if (Mathf.Abs(GetVolumeOfType(AudioKind, false) - wantedVolume) <= 0.001f)
                        SetVolume(AudioKind, wantedVolume);
                }
                yield return null;
            }
            if (currentCoroutines.ContainsKey(AudioKind))
                currentCoroutines.Remove(AudioKind);
        }
    }
}

