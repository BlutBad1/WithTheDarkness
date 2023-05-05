using UnityEngine;
namespace PlayerScriptsNS
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        public PlayerInput.OnFootActions OnFoot;
        private PlayerMotor motor;
        private PlayerLook look;
        [HideInInspector]
        public bool IsMovingEnable = true;
        void Awake()
        {

            playerInput = new PlayerInput();
            OnFoot = playerInput.OnFoot;
            motor = GetComponent<PlayerMotor>();
            look = GetComponent<PlayerLook>();
            OnFoot.Jump.performed += ctx => motor.Jump();
            OnFoot.Crouch.performed += ctx => motor.Crounch();
            OnFoot.Sprint.performed += ctx => motor.Sprint();
        }

        // Update is called once per frame

        void FixedUpdate()
        {
            if (IsMovingEnable)
            {
                motor.ProcessMove(OnFoot.Movement.ReadValue<Vector2>());
            }

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