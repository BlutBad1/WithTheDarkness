using EnemyBaseNS;
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
        float reachDestinationHelper = 0.3f;
        protected override void HandleStateChange(EnemyState oldState, EnemyState newState)
        {
            if (oldState != EnemyState.Idle)
                walkPointIsSet = false;
            base.HandleStateChange(oldState, newState);
        }

        protected override void Start()
        {
            base.Start();
            OnIdle += OnIdleState;
        }
        void OnIdleState()
        {
            if (Agent.enabled)
            {
                if (walkPointIsSet)
                    Agent.SetDestination(walkPoint);
                if (!walkPointIsSet)
                    SearchWalkPoint();
                Vector3 distanceToWalkPoint = transform.position - walkPoint;
                //Walkpoint reached
                if (distanceToWalkPoint.magnitude <= Agent.stoppingDistance + reachDestinationHelper)
                {
                    walkPointIsSet = false;
                    reachDestinationHelper = 0.3f;
                }
                else if (Agent.velocity.magnitude <= 0.01f && walkPointIsSet)
                    reachDestinationHelper += 0.1f;
            }
        }
        void SearchWalkPoint()
        {
            float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
            if (distanceToPlayer <= HiddenSightRange)
            {
                walkPoint = Player.transform.position;
                walkPointIsSet = true;
            }
            else
                BackToDefaultPosition();
        }
        public override void BackToDefaultPosition()
        {
            if ((transform.position - Player.transform.position).magnitude > HiddenSightRange * 2.5)
            {
                walkPointIsSet = false;
                Agent.Warp(DefaultPositon);
            }
            else if ((transform.position - Player.transform.position).magnitude > HiddenSightRange)
            {
                walkPointIsSet = true;
                walkPoint = DefaultPositon;
            }
        }
    }

}