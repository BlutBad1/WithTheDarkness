using System;
using UnityEngine;

namespace UIControlling
{
    public class WindowEscape : MonoBehaviour
    {
        public static WindowEscape Instance { get; private set; }

        public event Action OnUIEscapePressed;
        public event Action OnOnFootEscapePressed;

        private PlayerInput Input;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        private void OnEnable()
        {
            Input = SettingsNS.GameSettings.PlayerInput;
            Input.UI.Cancel.performed += InvokeOnUIEscape;
            Input.OnFoot.EscapeMenu.performed += InvokeOnFootEscape;
        }
        private void OnDisable()
        {
            if (Input != null)
            {
                Input.UI.Cancel.performed -= InvokeOnUIEscape;
                Input.OnFoot.EscapeMenu.performed -= InvokeOnFootEscape;
            }
        }
        private void InvokeOnUIEscape(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
           OnUIEscapePressed?.Invoke();
        private void InvokeOnFootEscape(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            OnOnFootEscapePressed?.Invoke();
        public void EnableUI()
        {
            Input.OnFoot.Disable();
            Input.UI.Enable();
        }
        public void DisableUI()
        {
            Input.OnFoot.Enable();
            Input.UI.Disable();
        }
    }
}