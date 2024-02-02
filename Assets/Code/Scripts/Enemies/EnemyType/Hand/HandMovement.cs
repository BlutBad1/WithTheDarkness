using EnemyNS.Base;
using EnemyNS.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Type.Hand
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandMovement : EnemyMovement
    {
    }
}