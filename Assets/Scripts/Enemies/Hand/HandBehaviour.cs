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
  

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GetComponent<HandMovement>().walkPointIsSet=true;
        GetComponent<HandMovement>().walkPoint = Player.transform.position;
   
    }
  
}
