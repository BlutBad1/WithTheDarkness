using MyConstants;
using System;
using UnityEngine;

namespace UIControlling
{
    public class WindowEscape : MonoBehaviour
    {
        public PlayerInput Input;
        public static Action OnEscape;
        public static WindowEscape instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        public void DefineAndBindAction()
        {
            GameObject player = GameObject.Find(CommonConstants.PLAYER);
            Input = SettingsNS.GameSettings.PlayerInput;
            Input.UI.Cancel.performed += ActionInvoke;
        }
        void ActionInvoke(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
           OnEscape?.Invoke();
        public void EnableUI()
        {
            if (Input == null)
                DefineAndBindAction();
            Input.OnFoot.Disable();
            Input.UI.Enable();
        }
        public void DisableUI()
        {
            if (Input == null)
                DefineAndBindAction();
            Input.OnFoot.Enable();
            Input.UI.Disable();
        }
    }
}