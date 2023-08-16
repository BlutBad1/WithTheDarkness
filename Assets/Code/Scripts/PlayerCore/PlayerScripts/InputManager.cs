using UnityEngine;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerMotor)), RequireComponent(typeof(PlayerLook))]
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMotor motor;
        private PlayerLook look;
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
            OnFoot.Jump.performed += PefrormJump;
            OnFoot.Crouch.performed += PefrormCrounch;
            OnFoot.Sprint.performed += PefrormSprint;
        }
        //void BindTest()
        //{
        // OnFoot.SwitchWeapon.ChangeBinding(1).WithPath("<Keyboard>/k");
        //}
        private void PefrormJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Jump();
        private void PefrormCrounch(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Crounch();
        private void PefrormSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Sprint();
        public ref readonly PlayerInput GetPlayerInput() =>
             ref playerInput;
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
            OnFoot.Jump.performed -= PefrormJump;
            OnFoot.Crouch.performed -= PefrormCrounch;
            OnFoot.Sprint.performed -= PefrormSprint;
            OnFoot.Disable();
        }
    }
}