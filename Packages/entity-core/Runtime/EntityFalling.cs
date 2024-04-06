using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EntityNS.Base
{
	public class EntityFalling : MonoBehaviour
	{
		[SerializeField]
		private NavMeshAgent agent;
		[SerializeField]
		private float gravity = -9.8f;
		[SerializeField, Tooltip("How high, relative to the character's pivot point the start of the ray is.")]
		private float groundCheckHeight = 0.5f;
		[SerializeField, Tooltip("What is the radius of the ray.")]
		private float groundCheckRadius = 0.5f;
		[SerializeField, Tooltip("How far the ray is casted.")]
		private float groundCheckDistance = 0.3f;
		[SerializeField, Tooltip("What are the layers that should be taken into account when checking for ground.")]
		private LayerMask groundLayers;
		[SerializeField]
		private bool debugModeOn = false;

		private void FixedUpdate()
		{
			if (agent.enabled && !agent.isOnNavMesh)
			{
				if (!CheckGround())
					transform.position = new Vector3(transform.position.x, transform.position.y + gravity * Time.deltaTime, transform.position.z);
			}
		}
		private bool CheckGround()
		{
			Ray ray = new Ray(transform.position + Vector3.up * groundCheckHeight, Vector3.down);
			if (Physics.SphereCast(ray, groundCheckRadius, groundCheckDistance, groundLayers))
				return true;
			else
				return false;
		}
		private void OnDrawGizmos()
		{
			if (debugModeOn)
			{
				Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
			}
		}
	}
}
