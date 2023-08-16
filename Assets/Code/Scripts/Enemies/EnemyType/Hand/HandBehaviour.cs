using EnemyBaseNS;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyHandNS
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandBehaviour : Enemy
    {
        private void Start() =>
            OnTakeDamage += OnTakeDamageHandBehaviour;
        private void OnTakeDamageHandBehaviour(float damage, float force, Vector3 hit)
        {
            if (Health > 0)
            {
                GetComponent<HandMovement>().walkPointIsSet = true;
                GetComponent<HandMovement>().walkPoint = Player.transform.position;
            }
            else
                GetComponent<HandMovement>().walkPointIsSet = false;
        }
    }
}