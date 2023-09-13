using EnemyNS.Base;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Type.Hand
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandMovement : EnemyMovement
    {
    }
}