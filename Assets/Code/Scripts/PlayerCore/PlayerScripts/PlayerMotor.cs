using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private float gravity = -9.8f;
        [SerializeField]
        private float groundCheckDistance;
        [SerializeField]
        private LayerMask slopeLayer;
        [SerializeField]
        private float jumpHeight = 3f;
        [SerializeField, Range(0f, 1f)]
        private float inertiaWeight = 0.65f;
        [Header("Speed")]
        [SerializeField, FormerlySerializedAs("DefaultSpeed")]
        public float defaultSpeed = 5;
        [SerializeField, FormerlySerializedAs("SprintingSpeed")]
        public float sprintingSpeed = 8;
        [SerializeField]
        private float crounchingSpeed = 2.5f;

        private bool isGrounded;
        private Vector3 lastVelocity;
        private float speedCoef = 1f;
        private float speed;
        private bool lerpCrounch;
        private float crounchTimer;
        private CharacterController character;
        private Vector3 playerYVelocity;

        public event Action OnSprintStartEvent;
        public event Action OnSprintCanceltEvent;

        public float CurrentSpeed
        {
            get { return speed * speedCoef; }
            set { speed = value; }
        }
        public bool IsCrounching { get; protected set; }
        public bool IsSprinting { get; protected set; }
        public bool IsGrounded
        {
            get => isGrounded;
            set
            {
                isGrounded = value;
                if (!IsGrounded && IsSprinting)
                    OnCancelSprint();
            }
        }
        public Vector3 CurrentScaledByTimeVelocity { get; protected set; }

        private void Awake()
        {
            character = GetComponent<CharacterController>();
            CurrentSpeed = defaultSpeed;
            IsSprinting = false;
        }
        private void Update()
        {
            IsGrounded = character.isGrounded;
            if (lerpCrounch)
            {
                crounchTimer += Time.deltaTime;
                float p = crounchTimer / 0.5f;
                p *= 2 * p;
                if (IsCrounching)
                {
                    character.height = Mathf.Lerp(character.height, 1, p);
                    CurrentSpeed = crounchingSpeed;
                }
                else
                {
                    character.height = Mathf.Lerp(character.height, 2, p);
                    CurrentSpeed = defaultSpeed;
                }
                if (p > 1)
                {
                    lerpCrounch = false;
                    crounchTimer = 0f;
                }
            }
        }
        /// <summary>
        /// Default is 1, a speed is multiplied by this coefficient
        /// </summary>
        /// <param name="coef"></param>
        public void SetSpeedCoef(float coef = 1f) =>
            speedCoef = coef;
        public Vector3 GetCharacterVelocity() =>
            character.velocity;
        public void Jump()
        {
            if (IsGrounded && !IsCrounching)
                playerYVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        public void Crounch()
        {
            IsCrounching = !IsCrounching;
            crounchTimer = 0;
            lerpCrounch = true;
        }
        public void OnStartSprint()
        {
            if (!IsCrounching && IsGrounded && defaultSpeed != sprintingSpeed)
            {
                IsSprinting = true;
                CurrentSpeed = sprintingSpeed;
                OnSprintStartEvent?.Invoke();
            }
        }
        public void OnCancelSprint()
        {
            OnSprintCanceltEvent?.Invoke();
            CancelSprint();
        }
        public void CancelSprint()
        {
            IsSprinting = false;
            CurrentSpeed = defaultSpeed;
        }
        public void ProcessMove(Vector2 input)
        {
            // Calculate target movement
            Vector3 horizontalMovement = new Vector3(input.x, 0, input.y);
            Vector3 targetVelocity = horizontalMovement * CurrentSpeed;
            // Apply inertia by combining the target velocity with last frame's velocity
            Vector3 velocityWithInertia = Vector3.Lerp(targetVelocity, lastVelocity, inertiaWeight);
            Vector3 velocity = SlopeCalculation(velocityWithInertia);
            lastVelocity = velocity;
            CurrentScaledByTimeVelocity = character.velocity * Time.deltaTime;
            // Set Y component to 0
            velocity.y = 0;
            // Gravity application
            if (IsGrounded && playerYVelocity.y < 0)
                playerYVelocity.y = -2f;
            else
                playerYVelocity.y += gravity * Time.deltaTime;
            velocity += playerYVelocity;
            velocity = transform.TransformDirection(velocity);
            character.Move(velocity * Time.deltaTime);
        }
        public void ResetVelocity()
        {
            lastVelocity = Vector3.zero;
            character.Move(lastVelocity * Time.deltaTime);
        }
        public CharacterController GetCharacterController() => character;
        private Vector3 SlopeCalculation(Vector3 calculatedMovement)
        {
            if (IsGrounded)
            {
                float maxDistance = character.height / 2 - character.radius + groundCheckDistance;
                Physics.SphereCast(transform.position, character.radius, Vector3.down, out RaycastHit groundCheckHit, maxDistance, slopeLayer);
                Vector3 localGroundCheckHitNormal = transform.InverseTransformDirection(groundCheckHit.normal);
                float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, transform.up);
                if (groundSlopeAngle > character.slopeLimit)
                {
                    Quaternion slopeAngleRotation = Quaternion.FromToRotation(transform.up, localGroundCheckHitNormal);
                    calculatedMovement = slopeAngleRotation * calculatedMovement;
                    float relativeSlopeAngle = Vector3.Angle(calculatedMovement, transform.up) - 90.0f;
                    calculatedMovement += calculatedMovement * (relativeSlopeAngle / character.slopeLimit);
                }
            }
            return calculatedMovement;
        }
        //NOTE: dev purpose 
        //private void OnDrawGizmos()
        //{
        //    float maxDistance = character.height/2 - character.radius + groundCheckDistance;
        //    Gizmos.color = Color.gray;
        //    Gizmos.DrawSphere(transform.position  + (Vector3.down * maxDistance), character.radius);
        //    Gizmos.color = Color.yellow;
        //    RaycastHit hit;
        //    if (Physics.SphereCast(transform.position, character.radius, Vector3.down, out hit, maxDistance, slopeLayer))
        //    {
        //        Vector3 sphereCastMidpoint = transform.position  + (Vector3.down * hit.distance);
        //        Gizmos.DrawWireSphere(sphereCastMidpoint, character.radius);
        //        Gizmos.DrawSphere(hit.point, 0.1f);
        //        // Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
        //    }
        //    else
        //    {
        //        Vector3 sphereCastMidpoint = transform.position + (Vector3.down * (maxDistance));
        //        Gizmos.DrawWireSphere(sphereCastMidpoint, character.radius);
        //        // Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
        //    }
        //}
    }
}