using EnemyBaseNS;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyHandNS
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandMovement : EnemyMovement
    {

        public float HiddenSightRange = 10f;
        [HideInInspector]
        public Vector3 walkPoint;
        [HideInInspector]
        public bool walkPointIsSet;

        protected override void HandleStateChange(EnemyState oldState, EnemyState newState)
        {
            if (oldState != EnemyState.Idle)
                walkPointIsSet = false;
            base.HandleStateChange(oldState, newState);


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
            if (distanceToPlayer <= HiddenSightRange)
            {
                walkPoint = Player.position;
                walkPointIsSet = true;
            }
            else
            {
                BackToDefaultPosition();
            }
        }
        protected override bool BackToDefaultPosition()
        {
    
            if ((transform.position - Player.transform.position).magnitude > HiddenSightRange * 2.5)
            {
             
                walkPointIsSet = false;
                Agent.Warp(DefaultPositon);
                return true;
            }
            else if ((transform.position - Player.transform.position).magnitude > HiddenSightRange)
            {
              
                walkPointIsSet = true;
                walkPoint = DefaultPositon;
                return true;
            }
            return false;
        }
    }

}