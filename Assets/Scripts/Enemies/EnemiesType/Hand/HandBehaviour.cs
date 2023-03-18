using EnemyBaseNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyHandNS
{
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandBehaviour : Enemy
{

    private void Start()
    {
        OnTakeDamage += OnTakeDamageHandBehaviour;
    }

      private void OnTakeDamageHandBehaviour(float force, Vector3 hit)
    {
        
        GetComponent<HandMovement>().walkPointIsSet = true;
        GetComponent<HandMovement>().walkPoint = Player.transform.position;
    }
}

}