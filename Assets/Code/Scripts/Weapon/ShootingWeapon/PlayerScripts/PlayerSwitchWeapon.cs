using UnityEngine;
using WeaponNS;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerSwitchWeapon : MonoBehaviour
    {
        private InputManager inputManager;
        public WeaponManagement.WeaponManager WeaponManager;
        private void Start()
        {
            inputManager = GetComponent<InputManager>();
            inputManager.OnFoot.SwitchWeapon.performed += PerformChangeWeaponSelection;
        }
        private void OnDisable()
        {
            inputManager.OnFoot.SwitchWeapon.performed -= PerformChangeWeaponSelection;
        }
        private void PerformChangeWeaponSelection(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            WeaponManager.ChangeActiveWeaponSelection(((WeaponType)obj.ReadValue<float>()) - 1);
    }
}