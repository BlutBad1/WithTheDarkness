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
        public MuteSound MuteSound;
        public float TransitionMuteTime = 0.5f;
        public GameObject ContinueButton_Normal;
        public GameObject ContinueButton_Disabled;
        public WindowsManagement WindowsManagement;
        public GameObject SureMenu_NewGame;
        private void Start()
        {
            WindowEscape.instance.EnableUI();
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
            if (MuteSound)
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
            if (MuteSound)
                StartCoroutine(WaitSounds(Loader.Load, scene));
            else
                Loader.Load(scene);
        }
        IEnumerator WaitSounds<T>(Action<T> MethodName, T agument)
        {
            MuteSound.MuteVolume(TransitionMuteTime);
            yield return new WaitForSeconds(TransitionMuteTime);
            MethodName(agument);
        }
    }
}