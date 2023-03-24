using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnabler : MonoBehaviour
{

    private Animator animator;
    [SerializeField]
    public Transform RagdollRoot;
    private NavMeshAgent agent;
    [SerializeField]
    private bool StartRagdoll = false;
    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;

    void Awake()
    {
        
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        if (StartRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
        agent.enabled = true;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
        }
    }
    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }
    public void EnableRagdoll()
    {
        animator.enabled = false;
        agent.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }
}
