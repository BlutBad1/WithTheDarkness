using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
	public class PlayerMotor : MonoBehaviour
	{
		[SerializeField]
		protected float gravity = -9.8f;
		[SerializeField]
		protected float groundCheckDistance;
		[SerializeField]
		protected LayerMask slopeLayer;
		[SerializeField]
		protected float jumpHeight = 3f;
		[SerializeField, Range(0f, 1f)]
		protected float inertiaWeight = 0.65f;
		[Header("Speed")]
		[SerializeField, FormerlySerializedAs("DefaultSpeed")]
		private float defaultSpeed = 5;
		[SerializeField, FormerlySerializedAs("SprintingSpeed")]
		private float sprintingSpeed = 8;
		[SerializeField]
		private float crounchingSpeed = 2.5f;

		protected bool isGrounded;
		protected Vector3 lastVelocity;
		private float speedCoef = 1f;
		protected float speed;
		protected bool lerpCrounch;
		protected float crounchTimer;
		private CharacterController character;
		protected Vector3 playerYVelocity;

		public event Action OnSprintStartEvent;
		public event Action OnSprintCanceltEvent;

		public float CurrentSpeed
		{
			get { return speed * SpeedCoef; }
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
		public CharacterController Character { get => character; protected set => character = value; }
		public float SpeedCoef { get => speedCoef; set => speedCoef = value; }
		public float CrounchingSpeed { get => crounchingSpeed; set => crounchingSpeed = value; }
		public float SprintingSpeed { get => sprintingSpeed; set => sprintingSpeed = value; }
		public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }

		private void Awake()
		{
			Character = GetComponent<CharacterController>();
			CurrentSpeed = DefaultSpeed;
			IsSprinting = false;
		}
		private void Update()
		{
			IsGrounded = Character.isGrounded;
			if (lerpCrounch)
			{
				crounchTimer += Time.deltaTime;
				float p = crounchTimer / 0.5f;
				p *= 2 * p;
				if (IsCrounching)
				{
					Character.height = Mathf.Lerp(Character.height, 1, p);
					CurrentSpeed = CrounchingSpeed;
				}
				else
				{
					Character.height = Mathf.Lerp(Character.height, 2, p);
					CurrentSpeed = DefaultSpeed;
				}
				if (p > 1)
				{
					lerpCrounch = false;
					crounchTimer = 0f;
				}
			}
		}
		public virtual void Jump()
		{
			if (IsGrounded && !IsCrounching)
				playerYVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
		}
		public virtual void Crounch()
		{
			IsCrounching = !IsCrounching;
			crounchTimer = 0;
			lerpCrounch = true;
		}
		public virtual void OnStartSprint()
		{
			if (!IsCrounching && IsGrounded && DefaultSpeed != SprintingSpeed)
			{
				IsSprinting = true;
				CurrentSpeed = SprintingSpeed;
				OnSprintStartEvent?.Invoke();
			}
		}
		public virtual void OnCancelSprint()
		{
			OnSprintCanceltEvent?.Invoke();
			CancelSprint();
		}
		public virtual void CancelSprint()
		{
			IsSprinting = false;
			CurrentSpeed = DefaultSpeed;
		}
		public virtual void ProcessMove(Vector2 input)
		{
			// Calculate target movement
			Vector3 horizontalMovement = new Vector3(input.x, 0, input.y);
			Vector3 targetVelocity = horizontalMovement * CurrentSpeed;
			// Apply inertia by combining the target velocity with last frame's velocity
			Vector3 velocityWithInertia = Vector3.Lerp(targetVelocity, lastVelocity, inertiaWeight);
			Vector3 velocity = SlopeCalculation(velocityWithInertia);
			lastVelocity = velocity;
			CurrentScaledByTimeVelocity = Character.velocity * Time.deltaTime;
			// Set Y component to 0
			velocity.y = 0;
			// Gravity application
			if (IsGrounded && playerYVelocity.y < 0)
				playerYVelocity.y = -2f;
			else
				playerYVelocity.y += gravity * Time.deltaTime;
			velocity += playerYVelocity;
			velocity = transform.TransformDirection(velocity);
			Character.Move(velocity * Time.deltaTime);
		}
		public virtual void ResetVelocity()
		{
			lastVelocity = Vector3.zero;
			Character.Move(lastVelocity * Time.deltaTime);
		}
		protected virtual Vector3 SlopeCalculation(Vector3 calculatedMovement)
		{
			if (IsGrounded)
			{
				float maxDistance = Character.height / 2 - Character.radius + groundCheckDistance;
				Physics.SphereCast(transform.position, Character.radius, Vector3.down, out RaycastHit groundCheckHit, maxDistance, slopeLayer);
				Vector3 localGroundCheckHitNormal = transform.InverseTransformDirection(groundCheckHit.normal);
				float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, transform.up);
				if (groundSlopeAngle > Character.slopeLimit)
				{
					Quaternion slopeAngleRotation = Quaternion.FromToRotation(transform.up, localGroundCheckHitNormal);
					calculatedMovement = slopeAngleRotation * calculatedMovement;
					float relativeSlopeAngle = Vector3.Angle(calculatedMovement, transform.up) - 90.0f;
					calculatedMovement += calculatedMovement * (relativeSlopeAngle / Character.slopeLimit);
				}
			}
			return calculatedMovement;
		}
	}
}