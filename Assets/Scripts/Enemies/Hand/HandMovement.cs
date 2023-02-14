using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandMovement :  EnemyMovement
{

    public float hiddenSightRange = 10f;
     [HideInInspector]
    public Vector3 walkPoint;
    [HideInInspector]
    public bool walkPointIsSet;

    public override void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (oldState != EnemyState.Idle)
            walkPointIsSet = false;
        base.HandleStateChange(oldState, newState);
        
    
    }

    protected override void HandleGainSight(GameObject player)
    {
      
        base.HandleGainSight(player);
    }
    public  void ChasePlayer()
    {
      
        Agent.SetDestination(Player.position);
       
    }
    protected override IEnumerator DoIdleMotion()
    {
        while (true)
        {
            if (Agent.enabled)
            {
                if (walkPointIsSet)
                {
                    Agent.SetDestination(walkPoint);
                }
                if (!walkPointIsSet)
                {
                    SearchWalkPoint();
                }

                Vector3 distanceToWalkPoint = transform.position - walkPoint;

                //Walkpoint reached
                if (distanceToWalkPoint.magnitude <= Agent.stoppingDistance + 0.3f)
                {
                    walkPointIsSet = false;
                }

            }
            yield return null;
        }

      
    }
    protected void SearchWalkPoint()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer <= hiddenSightRange)
        {
            walkPoint = Player.position;
            walkPointIsSet = true;
        }

    }
}
