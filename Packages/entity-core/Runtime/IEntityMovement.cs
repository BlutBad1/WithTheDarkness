using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Base
{
    public interface IEntityMovement
    {
        public NavMeshAgent Agent { get; }
        public EntityState CurrentState { get; set; }
        public EntityState DefaultState { get; }
        public void BackToDefaultPosition();
    }
}