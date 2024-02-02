using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerMotor)), RequireComponent(typeof(PlayerLook)), DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("OnFoot")]
        private PlayerInput.OnFootActions onFoot;
        [SerializeField, FormerlySerializedAs("UIActions")]
        private PlayerInput.UIActions uIActions;

        private PlayerInput playerInput;
        private PlayerMotor motor;
        private PlayerLook look;
        private bool IsMovingLocked = false;

        public PlayerInput.OnFootActions OnFoot { get => onFoot;  }
        public PlayerInput.UIActions UIActions { get => uIActions; }

        private void Awake()
        {
            InitializeFields();
        }
        private void FixedUpdate()
        {
            if (!IsMovingLocked)
                motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
        private void LateUpdate() =>
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        private void OnEnable() =>
            onFoot.Enable();
        private void OnDisable()
        {
            onFoot.Jump.performed -= PefrormJump;
            onFoot.Crouch.performed -= PefrormCrounch;
            onFoot.Sprint.started -= StartSprint;
            onFoot.Sprint.canceled -= CancelSprint;
            onFoot.Disable();
        }
        public void SetMovingLock(bool lockStatus, bool isResetVelocity = false)
        {
            IsMovingLocked = lockStatus;
            if (isResetVelocity)
                motor.ResetVelocity();
        }
        private void PefrormJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Jump();
        private void PefrormCrounch(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.Crounch();
        private void StartSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            motor.OnStartSprint();
        private void CancelSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
          motor.OnCancelSprint();
        private void InitializeFields()
        {
            playerInput = SettingsNS.GameSettings.PlayerInput;
            onFoot = playerInput.OnFoot;
            uIActions = playerInput.UI;
            motor = GetComponent<PlayerMotor>();
            look = GetComponent<PlayerLook>();
            onFoot.Jump.performed += PefrormJump;
            onFoot.Crouch.performed += PefrormCrounch;
            onFoot.Sprint.started += StartSprint;
            onFoot.Sprint.canceled += CancelSprint;
        }
    }
}