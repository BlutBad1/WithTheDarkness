using PlayerScriptsNS;
using ScenesManagementNS;
using SettingsNS;
using TMPro;
using UIControlling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UINS
{
    public class InGameMenuManager : MonoBehaviour
    {
        public GameObject GameMenu;
        //public WindowsManagement WindowsManagement;
        InputManager playerInputManager;
        WindowEscape windowEscape;
        private void Start()
        {
            playerInputManager = GameObject.Find(MyConstants.CommonConstants.PLAYER).GetComponent<InputManager>();
            if (playerInputManager != null)
                playerInputManager.OnFoot.EscapeMenu.performed += MethodToSubscribe;
            windowEscape = WindowEscape.instance;
            //if (WindowsManagement.Windows == null)
            //    WindowsManagement.Windows = new Window[0];
            //Array.Resize(ref WindowsManagement.Windows, WindowsManagement.Windows.Length + 1);
            //WindowsManagement.Windows[^1] = new Window(MyConstants.UIConstants.SETTINGS_MENU, SettingsMenuInitialize.instance.SettingsMenuGameObject);
            windowEscape.DefineAndBindAction();
        }
        private void OnDisable()
        {
            playerInputManager.OnFoot.EscapeMenu.performed -= MethodToSubscribe;
        }
        public void BackToMainMenu()
        {
            Time.timeScale = 1f;
            Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
        }
        private void MethodToSubscribe(InputAction.CallbackContext context) =>
            EnableInGameMenu();
        public void EnableInGameMenu()
        {
            InGameMenu.OnGameMenuOpenEvent?.Invoke();
            InGameMenu.IsInGameMenuOpened = true;
            windowEscape.EnableUI();
            Time.timeScale = 0f;
            GameObject interactableText = GameObject.Find(MyConstants.HUDConstants.INTERACTABLE_TEXT);
            if (interactableText && interactableText.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
                textMeshProUGUI.text = "";
            GameMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        public void DisableInGameMenu()
        {
            InGameMenu.IsInGameMenuOpened = false;
            GameMenu.SetActive(false);
            InGameMenu.OnGameMenuCloseEvent?.Invoke();
            Time.timeScale = 1f;
            windowEscape.DisableUI();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
