using DataSaving.SceneSaving;
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
        public GameObject ContinueButton_Normal;
        public GameObject ContinueButton_Disabled;
        public WindowsManagement WindowsManagement;
        public GameObject SureMenu_NewGame;
        ProgressSaving ProgressSaving;
        private void Start()
        {
            WindowEscape.instance.EnableUI();
            ProgressSaving = new ProgressSaving();
            ProgressSaving.LoadProgressData();
            if (ProgressSaving.DataIsLoaded())
            {
                ContinueButton_Normal.SetActive(true);
                ContinueButton_Disabled.SetActive(false);
            }
            else
            {
                ContinueButton_Normal.SetActive(false);
                ContinueButton_Disabled.SetActive(true);
            }
        }
        public void GameContinue()
        {
            if (ProgressSaving.DataIsLoaded())
                LoadScene(ProgressSaving.LoadedScene.SceneName);
        }
        public void TryNewGame()
        {
            if (!ProgressSaving.DataIsLoaded())
                StartNewGame();
            else
                WindowsManagement.ChangeWindow(SureMenu_NewGame.name);
        }

        public void StartNewGame()
        {
            ProgressSaving.LoadedScene = new ProgressData(SceneConstants.LEVEL1);
            ProgressSaving.SaveProgressData();
            LoadScene(SceneConstants.LEVEL1);
        }
        public void GameExit()
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
            yield return new WaitForSeconds(0.03f);
            MethodName(agument);
        }
    }
}