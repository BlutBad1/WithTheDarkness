using UnityEngine;
using UnityEngine.AI;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField]
    public Transform RagdollRoot;
    public Animator Animator;
    public NavMeshAgent Agent;
    public bool EnableRagdollOnStart = true;
    private bool isRagdollEnabled = false;
    public bool IsRagdollEnabled
    {
        get { return isRagdollEnabled; }
        set { isRagdollEnabled = value; }
    }
    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    private Collider[] colliders;
    private void Start()
    {
        rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        if (EnableRagdollOnStart)
            EnableRagdoll();
        else
            DisableAllRigidbodies();
    }
    public void EnableAnimator()
    {
        isRagdollEnabled = false;
        Animator.enabled = true;
        if (Agent)
            Agent.enabled = true;
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
        isRagdollEnabled = false;
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
        isRagdollEnabled = true;
        if (Animator)
            Animator.enabled = false;
        if (Agent)
            Agent.enabled = false;
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
