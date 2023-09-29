using UnityEngine;
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
        [SerializeField]
        public float DefaultSpeed = 5;
        [SerializeField]
        private float sprintingSpeed = 8;
        [SerializeField]
        private float crounchingSpeed = 2.5f;
        [HideInInspector]
        public bool IsGrounded;
        [HideInInspector]
        public Vector3 CurrentScaledByTimeVelocity;
        private Vector3 lastVelocity = Vector3.zero;
        private float speedCoef = 1f;
        private float speed;
        private bool lerpCrounch;
        private bool crounching;
        private bool sprinting;
        private float crounchTimer;
        private CharacterController character;
        private Vector3 playerYVelocity;
        public float CurrentSpeed
        {
            get { return speed * speedCoef; }
            set { speed = value; }
        }
        private void Awake()
        {
            character = GetComponent<CharacterController>();
            CurrentSpeed = DefaultSpeed;
            sprinting = false;
        }
        private void Update()
        {
            IsGrounded = character.isGrounded;
            if (lerpCrounch)
            {
                crounchTimer += Time.deltaTime;
                float p = crounchTimer / 0.5f;
                p *= 2 * p;
                if (crounching)
                {
                    character.height = Mathf.Lerp(character.height, 1, p);
                    CurrentSpeed = crounchingSpeed;
                }
                else
                {
                    character.height = Mathf.Lerp(character.height, 2, p);
                    CurrentSpeed = DefaultSpeed;
                }
                if (p > 1)
                {
                    lerpCrounch = false;
                    crounchTimer = 0f;
                }
            }
        }
        /// <summary>
        /// Default is 1, speedt is multiplied by this coefficient
        /// </summary>
        /// <param name="coef"></param>
        public void SetSpeedCoef(float coef = 1f) =>
            speedCoef = coef;
        public Vector3 GetCharacterVelocity() =>
            character.velocity;
        public void Jump()
        {
            if (IsGrounded)
                playerYVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        public void Crounch()
        {
            crounching = !crounching;
            crounchTimer = 0;
            lerpCrounch = true;
        }
        public void Sprint()
        {
            if (!crounching)
            {
                sprinting = !sprinting;
                if (sprinting) CurrentSpeed = sprintingSpeed;
                else CurrentSpeed = DefaultSpeed;
            }
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