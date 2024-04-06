using PlayerScriptsNS;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponNS.ShootingWeaponNS
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerBattleInput : MonoBehaviour
    {
        public static Action AttackInputStarted;
        public static Action AttackInputCanceled;
        public static Action AltAttackInputStarted;
        public static Action AltAttackInputCanceled;
        public static Action ReloadInputStarted;
        public static Action ReloadInputCanceled;

        private InputManager inputManager;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            AttachActions();
        }
        private void OnEnable()
        {
            if (inputManager != null)
                AttachActions();
        }
        private void OnDisable()
        {
            DettachActions();
        }
        public void AttachActions()
        {
            AttackInputStarted = null;
            AttackInputCanceled = null;
            AltAttackInputStarted = null;
            AltAttackInputCanceled = null;
            ReloadInputStarted = null;
            ReloadInputCanceled = null;
            inputManager.OnFoot.Firing.started += HandleFiringStarted;
            inputManager.OnFoot.Firing.canceled += HandleFiringCanceled;
            inputManager.OnFoot.AlternativeFiring.started += HandleAltFiringStarted;
            inputManager.OnFoot.AlternativeFiring.canceled += HandleAltFiringCanceled;
            inputManager.OnFoot.Reloading.started += HandleReloadingStarted;
            inputManager.OnFoot.Reloading.canceled += HandleReloadingCanceled;
        }
        public void DettachActions()
        {
            inputManager.OnFoot.Firing.started -= HandleFiringStarted;
            inputManager.OnFoot.Firing.canceled -= HandleFiringCanceled;
            inputManager.OnFoot.AlternativeFiring.started -= HandleAltFiringStarted;
            inputManager.OnFoot.AlternativeFiring.canceled -= HandleAltFiringCanceled;
            inputManager.OnFoot.Reloading.started -= HandleReloadingStarted;
            inputManager.OnFoot.Reloading.canceled -= HandleReloadingCanceled;
        }
        private void HandleFiringStarted(InputAction.CallbackContext context)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            AttackInputStarted?.Invoke();
        }
        private void HandleFiringCanceled(InputAction.CallbackContext context) =>
            AttackInputCanceled?.Invoke();
        private void HandleAltFiringStarted(InputAction.CallbackContext context) =>
            AltAttackInputStarted?.Invoke();
        private void HandleAltFiringCanceled(InputAction.CallbackContext context) =>
            AltAttackInputCanceled?.Invoke();
        private void HandleReloadingStarted(InputAction.CallbackContext context) =>
            ReloadInputStarted?.Invoke();
        private void HandleReloadingCanceled(InputAction.CallbackContext context) =>
            ReloadInputCanceled?.Invoke();
    }
}