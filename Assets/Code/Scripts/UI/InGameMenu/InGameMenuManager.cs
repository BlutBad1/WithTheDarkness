using SceneConstantsNS;
using ScenesManagementNS;
using SettingsNS;
using UIControlling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityMethodsNS;

namespace UINS
{
	public class InGameMenuManager : OnEnableMethodAfterStart
	{
		[SerializeField, FormerlySerializedAs("GameMenu")]
		private GameObject gameMenu;
		[SerializeField]
		private WindowsManagement windowsManagement;

		private WindowEscape windowEscape;

		protected override void OnEnableAfterStart()
		{
			windowEscape = WindowEscape.Instance;
			windowEscape.OnOnFootEscapePressed += EnableInGameMenu;
			windowEscape.OnUIEscapePressed += DisableInGameMenu;
		}
		private void OnDisable()
		{
			windowEscape.OnOnFootEscapePressed -= EnableInGameMenu;
			windowEscape.OnUIEscapePressed -= DisableInGameMenu;
		}
		public void BackToMainMenu() // in events
		{
			Time.timeScale = 1f;
			Loader.Load(SceneConstants.MAIN_MENU);
		}
		private void EnableInGameMenu()
		{
			InGameMenu.OnGameMenuOpenEvent?.Invoke();
			InGameMenu.IsInGameMenuOpened = true;
			windowEscape.EnableUI();
			Time.timeScale = 0f;
			gameMenu.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}
		private void DisableInGameMenu()
		{
			if (windowsManagement.CurrentWindow == windowsManagement.FirstMenu)
			{
				InGameMenu.IsInGameMenuOpened = false;
				gameMenu.SetActive(false);
				InGameMenu.OnGameMenuCloseEvent?.Invoke();
				Time.timeScale = 1f;
				windowEscape.DisableUI();
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}
}
