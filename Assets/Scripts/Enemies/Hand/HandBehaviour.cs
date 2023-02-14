using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum HandState
{

}
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandBehaviour : Enemy
{
    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        GetComponent<HandMovement>().walkPointIsSet=true;
        GetComponent<HandMovement>().walkPoint = Player.transform.position;
       
    }
}
