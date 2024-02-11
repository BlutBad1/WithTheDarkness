using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class RagdollEnabler : MonoBehaviour
{
	[SerializeField, FormerlySerializedAs("RagdollRoot")]
	private Transform ragdollRoot;
	[SerializeField, FormerlySerializedAs("Animator")]
	private Animator animator;
	[SerializeField, FormerlySerializedAs("Agent")]
	private NavMeshAgent agent;
	[SerializeField, FormerlySerializedAs("EnableRagdollOnStart")]
	private bool enableRagdollOnStart = true;

	private Rigidbody[] rigidbodies;
	private CharacterJoint[] joints;
	private Collider[] colliders;

	public bool IsRagdollEnabled { get; private set; }

	private void Start()
	{
		rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
		joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
		colliders = ragdollRoot.GetComponentsInChildren<Collider>();
		if (enableRagdollOnStart)
			EnableRagdoll();
		else
			DisableAllRigidbodies();
	}
	public void EnableAnimator()
	{
		IsRagdollEnabled = false;
		animator.enabled = true;
		if (agent)
			agent.enabled = true;
		foreach (CharacterJoint joint in joints)
			joint.enableCollision = false;
		foreach (Rigidbody rigidbody in rigidbodies)
		{
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
		}
	}
	public void DisableAllRigidbodies()
	{
		IsRagdollEnabled = false;
		foreach (Rigidbody rigidbody in rigidbodies)
		{
			//rigidbody.detectCollisions = false;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
		foreach (Collider collider in colliders)
			collider.enabled = false;
	}
	public void EnableRagdoll()
	{
		IsRagdollEnabled = true;
		if (animator)
			animator.enabled = false;
		if (agent)
			agent.enabled = false;
		foreach (CharacterJoint joint in joints)
			joint.enableCollision = true;
		foreach (Rigidbody rigidbody in rigidbodies)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.detectCollisions = true;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
		}
		foreach (Collider collider in colliders)
			collider.enabled = true;
	}
}
