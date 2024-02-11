using DataSaving.SceneSaving;
using SceneConstantsNS;
using ScenesManagementNS;
using SoundNS;
using System;
using System.Collections;
using UIControlling;
using UnityEngine;
using UnityEngine.Serialization;

namespace UINS
{
	public class MainMenuFunctionality : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("MuteSound")]
		private MuteSound muteSound;
		[SerializeField, FormerlySerializedAs("TransitionMuteTime")]
		private float transitionMuteTime = 0.5f;
		[SerializeField, FormerlySerializedAs("ContinueButton_Normal")]
		private GameObject continueButtonNormal;
		[SerializeField, FormerlySerializedAs("ContinueButton_Disabled")]
		private GameObject continueButtonDisabled;
		[SerializeField, FormerlySerializedAs("WindowsManagement")]
		private WindowsManagement windowsManagement;
		[SerializeField, FormerlySerializedAs("SureMenu_NewGame")]
		private GameObject sureMenuNewGame;

		private void Start()
		{
			WindowEscape.Instance.EnableUI();
			ProgressSaving.LoadProgressData();
			if (ProgressSaving.IsDataLoaded())
			{
				continueButtonNormal.SetActive(true);
				continueButtonDisabled.SetActive(false);
			}
			else
			{
				continueButtonNormal.SetActive(false);
				continueButtonDisabled.SetActive(true);
			}
		}
		public void GameContinue()
		{
			if (ProgressSaving.IsDataLoaded())
				LoadScene(ProgressSaving.LoadedScene.SceneName);
		}
		public void TryNewGame()
		{
			if (!ProgressSaving.IsDataLoaded())
				StartNewGame();
			else
				windowsManagement.ChangeWindow(sureMenuNewGame.name);
		}

		public void StartNewGame()
		{
			ProgressSaving.LoadedScene = new ProgressData(SceneConstants.LEVEL1);
			ProgressSaving.SaveProgressData();
			LoadScene(SceneConstants.LEVEL1);
		}
		public void GameExit()
		{
			if (muteSound)
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
			if (muteSound)
				StartCoroutine(WaitSounds(Loader.Load, scene));
			else
				Loader.Load(scene);
		}
		private IEnumerator WaitSounds<T>(Action<T> MethodName, T agument)
		{
			muteSound.MuteVolume(transitionMuteTime);
			yield return new WaitForSeconds(transitionMuteTime);
			MethodName(agument);
		}
	}
}