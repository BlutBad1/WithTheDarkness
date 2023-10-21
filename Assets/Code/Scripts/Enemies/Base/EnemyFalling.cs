using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Base
{
    public class EnemyFalling : MonoBehaviour
    {
        [SerializeField]
        public NavMeshAgent Agent;
        [HideInInspector]
        public bool IsGrounded = true;
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
            if (Agent.enabled && !Agent.isOnNavMesh)
            {
                if (CheckGround())
                {
                    Agent.enabled = false;
                    Agent.enabled = true;
                }
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y + gravity * Time.deltaTime, transform.position.z);
            }
        }
        private bool CheckGround()
        {
            Ray ray = new Ray(transform.position + Vector3.up * groundCheckHeight, Vector3.down);
            if (Physics.SphereCast(ray, groundCheckRadius, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))
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
