using MyConstants;
using ScenesManagementNS;
using SoundNS;
using System;
using System.Collections;
using UIControlling;
using UnityEngine;

namespace UINS
{
    public class MainMenuFunctionality : MonoBehaviour
    {
        public AudioManager AudioManager;
        private void Start()
        {
            WindowEscape.instance.EnableUI();
        }
        public void ExitTheGame()
        {
            if (!AudioManager)
                GameObject.Find(UIConstants.UI_SOUNDS)?.TryGetComponent(out AudioManager);
            if (AudioManager != null)
                StartCoroutine(WaitSounds(delegate (string s)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                     Application.Quit();
#endif
                }, ""));
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        public void LoadScene(string scene)
        {
            if (AudioManager != null)
                StartCoroutine(WaitSounds(Loader.Load, scene));
            else
                Loader.Load(scene);
        }
        IEnumerator WaitSounds<T>(Action<T> MethodName, T agument)
        {
            foreach (var s in AudioManager.sounds)
            {
                if (s.source.isPlaying)
                    yield return new WaitForSeconds(0.03f);
            }
            MethodName(agument);
        }
    }
}