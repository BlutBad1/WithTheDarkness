using EntityNS.Base;
using EntityNS.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Type.Hand
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandMovement : EntityMovement
    {
    }
}