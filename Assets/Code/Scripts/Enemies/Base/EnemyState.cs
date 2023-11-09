using UnityEngine;

namespace EnemyNS.Base
{
    public enum EnemyState
    {
        Spawn,
        Idle,
        Patrol,
        Chase,
        UsingAbility,
        LostTarget,
        DoPriority,
        Dead
    }
    public class PriorityTask
    {
        public Vector3 Destination;
        public int Priority;
        public PriorityTask(Vector3 Destination, int Priority)
        {
            this.Destination = Destination;
            this.Priority = Priority;
        }
    }
}