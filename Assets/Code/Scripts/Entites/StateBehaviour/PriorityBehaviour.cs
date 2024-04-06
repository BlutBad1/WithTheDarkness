using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityNS.Base.StateBehaviourNS.Priority
{
    public class PriorityBehaviour : StateBehaviour
    {
        [SerializeField]
        private StateHandler stateHandler;

        private List<PriorityTask> priorityTasks = new List<PriorityTask>();
        private PriorityTask currentTast;

        private void OnEnable()
        {
            stateHandler.OnDoPriority += OnDoPriority;
        }
        private void OnDisable()
        {
            stateHandler.OnDoPriority -= OnDoPriority;
        }
        public void AddNewPriorityTasks(PriorityTask priorityTask)
        {
            priorityTasks.Add(priorityTask);
            stateHandler.CurrentState = EntityState.DoPriority;
        }
        public bool CheckIfHasPriority() =>
            priorityTasks.Count > 0;
        private bool OnDoPriority()
        {
            if (priorityTasks.Count > 0)
            {
                if (stateHandler.IsArrived() || stateHandler.Destination != currentTast.Destination)
                {
                    priorityTasks.Remove(currentTast);
                    currentTast = priorityTasks.OrderByDescending(t => t.Priority).FirstOrDefault();
                    if (currentTast != null)
                    {
                        Vector3 currentDestination = currentTast.Destination;
                        stateHandler.Destination = currentDestination;
                    }
                }
                return true;
            }
            return false;
        }
    }
}