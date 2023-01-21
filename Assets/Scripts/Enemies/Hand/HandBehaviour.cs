using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandBehaviour : Enemy
{
    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        GetComponent<HandMovement>().walkPointSet = true;
        GetComponent<HandMovement>().walkPoint = transform.position;
        GetComponent<HandMovement>().walkPointSet = true;
    }
}
