using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandMovement :  EnemyMovement
{

    public float hiddenSightRange = 10f;
    

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
    public void ChasePlayer()
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
    public override void BackToDefaultPosition()
    {
        if ((transform.position - Player.transform.position).magnitude > hiddenSightRange * 2.5)
        {
            walkPointIsSet = false;
            Agent.Warp(defaultPositon);
        }
        else if ((transform.position - Player.transform.position).magnitude > hiddenSightRange)
        {
           
                walkPointIsSet = true;
                walkPoint = defaultPositon;
          
        }

    }
}
