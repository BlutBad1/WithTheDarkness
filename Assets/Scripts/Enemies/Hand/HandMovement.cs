using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandMovement :  EnemyMovement
{
  

  
    public LayerMask whatIsPlayer;
    //Patroling
    [HideInInspector]
    public Vector3 walkPoint;
    [HideInInspector]
    public bool walkPointSet;

    //States
    public float sightRange;
    public float hiddenSightRange;
    [HideInInspector]
    public bool playerInSightRange;

    [HideInInspector]
    public bool isInKnockout =false;
  



  
    protected override IEnumerator FollowTarget()
    {
      
        while (enabled)
        {
            if (Agent.enabled)
            {
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

                Vector3 distanceToPlayer = transform.position - Player.position;
              
                if (!playerInSightRange) Patroling();
                if (playerInSightRange ) ChasePlayer();
             


            }
            yield return null;
        }
    }
    protected  void Patroling()
    {

        if (!walkPointSet) 
            SearchWalkPoint();
        if (walkPointSet)
            Agent.SetDestination(walkPoint);


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    protected void SearchWalkPoint()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer <= hiddenSightRange)
        {
            walkPoint = Player.position;
            walkPointSet = true;
        }

    }

    public  void ChasePlayer()
    {
        Agent.SetDestination(Player.position);
       
    }

  
}
