using UnityEngine;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerMotor)), RequireComponent(typeof(PlayerLook)), DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour
    {
        public PlayerInput.OnFootActions OnFoot;
        public PlayerInput.UIActions UIActions;
        private PlayerInput playerInput;
        private PlayerMotor motor;
        private PlayerLook look;
        private bool IsMovingLocked = false;
        private void Awake()
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
        private void FixedUpdate()
        {
            if (!IsMovingLocked)
                motor.ProcessMove(OnFoot.Movement.ReadValue<Vector2>());
        }
        private void LateUpdate() =>
            look.ProcessLook(OnFoot.Look.ReadValue<Vector2>());
        private void OnEnable() =>
            OnFoot.Enable();
        private void OnDisable()
        {
            OnFoot.Jump.performed -= PefrormJump;
            OnFoot.Crouch.performed -= PefrormCrounch;
            OnFoot.Sprint.performed -= PefrormSprint;
            OnFoot.Disable();
        }
        public void SetMovingLock(bool lockStatus, bool isResetVelocity = false)
        {
            IsMovingLocked = lockStatus;
            if (isResetVelocity)
                motor.ProcessMove(new Vector2(0, 0));
        }
        private void PefrormJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Jump();
        private void PefrormCrounch(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Crounch();
        private void PefrormSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Sprint();
        //void BindTest()
        //{
        // OnFoot.SwitchWeapon.ChangeBinding(1).WithPath("<Keyboard>/k");
        //}
    }
}