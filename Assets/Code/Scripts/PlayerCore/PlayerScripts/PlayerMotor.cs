using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerMotor : MonoBehaviour
    {
        [HideInInspector]
        public bool isGrounded;
        [SerializeField]
        private float gravity = -9.8f;
        [SerializeField]
        private float groundCheckDistance;
        [SerializeField]
        LayerMask slopeLayer;
        [SerializeField]
        private float jumpHeight = 3f;
        [SerializeField]
        private float sprintingSpeed = 8;
        [SerializeField]
        private float defaultSpeed = 5;
        [SerializeField]
        private float crounchingSpeed = 2.5f;
        [HideInInspector]
        public Vector3 currentVelocity;
        [HideInInspector]
        public Vector3 moveDirection = Vector3.zero;
        private float speedCoef = 1f;
        private float speed;
        private bool lerpCrounch;
        private bool crounching;
        private bool sprinting;
        private float crounchTimer;
        private CharacterController character;
        private Vector3 playerVelocity;
        protected float Speed
        {
            get { return speed * speedCoef; }
            set { speed = value; }
        }

        void Start()
        {
            character = GetComponent<CharacterController>();
            Speed = defaultSpeed;
            sprinting = false;
        }
        void Update()
        {
            isGrounded = character.isGrounded;
            if (lerpCrounch)
            {
                crounchTimer += Time.deltaTime;
                float p = crounchTimer / 0.5f;
                p *= 2 * p;
                if (crounching)
                {
                    character.height = Mathf.Lerp(character.height, 1, p);
                    Speed = crounchingSpeed;
                }
                else
                {
                    character.height = Mathf.Lerp(character.height, 2, p);
                    Speed = defaultSpeed;
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
        public void Jump()
        {
            if (isGrounded)
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
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
                if (sprinting) Speed = sprintingSpeed;
                else Speed = defaultSpeed;
            }
        }
        public void ProcessMove(Vector2 input)
        {
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            character.Move(transform.TransformDirection(SlopeCalculation(moveDirection * Speed * Time.deltaTime)));
            playerVelocity.y += gravity * Time.deltaTime;
            character.Move(playerVelocity * Time.deltaTime);
            if (isGrounded && playerVelocity.y < 0)
                playerVelocity.y = -2f;
            currentVelocity = (transform.TransformDirection(moveDirection) * Speed * Time.deltaTime);
        }
        private Vector3 SlopeCalculation(Vector3 calculatedMovement)
        {
            if (isGrounded)
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