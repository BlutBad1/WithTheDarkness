using UnityEngine;
using WeaponManagement;

namespace PlayerScriptsNS
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMotor motor;
        private PlayerLook look;
        private WeaponManager weaponManager;
        public PlayerInput.OnFootActions OnFoot;
        public PlayerInput.UIActions UIActions;
        [HideInInspector]
        public bool IsMovingEnable = true;
        void Awake()
        {
            playerInput = SettingsNS.GameSettings.PlayerInput;
            OnFoot = playerInput.OnFoot;
            UIActions = playerInput.UI;
            motor = GetComponent<PlayerMotor>();
            look = GetComponent<PlayerLook>();
            if (!TryGetComponent(out weaponManager))
                weaponManager = GetComponentInChildren<WeaponManager>();
            OnFoot.Jump.performed += ctx => motor.Jump();
            OnFoot.Crouch.performed += ctx => motor.Crounch();
            OnFoot.Sprint.performed += ctx => motor.Sprint();
            OnFoot.SwitchWeapon.performed += ctx => weaponManager.ChangeWeaponSelection(((int)ctx.ReadValue<float>()) - 1);
        }
        public ref readonly PlayerInput GetPlayerInput() =>
             ref playerInput;
        //void BindTest()
        //{
        // OnFoot.SwitchWeapon.ChangeBinding(1).WithPath("<Keyboard>/k");
        //}
        void FixedUpdate()
        {
            if (IsMovingEnable)
                motor.ProcessMove(OnFoot.Movement.ReadValue<Vector2>());
        }
        private void LateUpdate()
        {
            look.ProcessLook(OnFoot.Look.ReadValue<Vector2>());
        }
        private void OnEnable()
        {
            OnFoot.Enable();
        }
        private void OnDisable()
        {
            OnFoot.Disable();
        }
    }
}