using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Base
{
    public class EnemyStateHandler : StateHandler
    {
        [SerializeField, Tooltip("State update delay")]
        private float updateRate = 0.1f;
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private SpotTarget spotTarget;

        private GameObject pursuedTarget;
        private Transform lastPursuedTargetPosition;
        private Coroutine followCoroutine;
        private bool isStateChangeBlocked = false;
        private List<PriorityTask> priorityTasks = new List<PriorityTask>();

        protected SpotTarget SpotTarget { get => spotTarget; }
        public List<PriorityTask> PriorityTasks { get => priorityTasks; }
        public override GameObject PursuedTarget
        {
            get => pursuedTarget;
            set
            {
                pursuedTarget = value;
                if (pursuedTarget)
                    State = EnemyState.Chase;
                else
                    State = DefaultState;
            }
        }
        public override Vector3 Destination
        {
            get => agent.destination;
            set { if (agent.isOnNavMesh) agent.SetDestination(value); }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SpotTarget.OnTargetSpot += OnTargetSpot;
            SpotTarget.OnTargetLost += OnTargetLost;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            SpotTarget.OnTargetSpot -= OnTargetSpot;
            SpotTarget.OnTargetLost -= OnTargetLost;
        }
        public override bool IsArrived() =>
           UtilitiesNS.Utilities.CheckIfAgentHasArrived(agent);
        protected override void HandleNewState(EnemyState oldState, EnemyState newState)
        {
            if (oldState != newState && oldState != EnemyState.Dead)
            {
                if (!isStateChangeBlocked || newState == EnemyState.Dead)
                {
                    switch (newState)
                    {
                        case EnemyState.Idle:
                            HandleState(DoIdleMotion());
                            break;
                        case EnemyState.Investigate:
                            HandleState(DoInvestigateMotion());
                            break;
                        case EnemyState.Patrol:
                            HandleState(DoPatrolMotion());
                            break;
                        case EnemyState.Chase:
                            HandleState(DoFollowTarget());
                            break;
                        case EnemyState.Dead:
                            DefaultState = EnemyState.Dead;
                            HandleState(DeadCoroutine());
                            break;
                        case EnemyState.DoPriority:
                            HandleState(DoPriority());
                            break;
                        case EnemyState.LostTarget:
                            HandleState(DoLostTarget());
                            break;
                        case EnemyState.UsingAbility:
                            break;
                        default:
                            State = DefaultState;
                            break;
                    }
                }
            }
        }
        protected virtual void OnTargetSpot(GameObject spottedTarget) =>
            PursuedTarget = spottedTarget;
        protected virtual void OnTargetLost(GameObject spottedTarget)
        {
            if (spottedTarget == PursuedTarget)
            {
                PursuedTarget = null;
                State = EnemyState.LostTarget;
            }
        }
        public override StateInfo GenerateStateInfo() =>
            new StateInfo(PursuedTarget, State);
        protected virtual IEnumerator DoIdleMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(updateRate);
            while (true)
            {
                CheckIfHasPursuedTarget();
                InvokeIdleEvent();
                yield return Wait;
            }
        }
        protected virtual IEnumerator DoPatrolMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(updateRate);
            while (true)
            {
                CheckIfHasPursuedTarget();
                InvokePatrolEvent();
                yield return Wait;
            }
        }
        protected virtual IEnumerator DoInvestigateMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(updateRate);
            yield return Wait;
            while (!IsArrived())
            {
                CheckIfHasPursuedTarget();
                InvokeInvestigateEvent();
                yield return Wait;
            }
            State = DefaultState;
        }
        protected virtual IEnumerator DeadCoroutine()
        {
            while (true)
            {
                agent.enabled = false;
                yield return null;
            }
        }
        protected virtual IEnumerator DoLostTarget()
        {
            WaitForSeconds Wait = new WaitForSeconds(updateRate);
            Destination = lastPursuedTargetPosition.position;
            yield return Wait;
            while (!IsArrived())
            {
                InvokeDoLostTargetEvent();
                yield return Wait;
            }
            State = DefaultState;
        }
        protected virtual IEnumerator DoFollowTarget()
        {
            while (PursuedTarget)
            {
                InvokeOnFollowEvent();
                Transform pursuedTargetTransform = PursuedTarget.transform;
                lastPursuedTargetPosition = pursuedTargetTransform;
                Destination = pursuedTargetTransform.position;
                yield return null;
            }
            State = DefaultState;
        }
        protected virtual IEnumerator DoPriority()
        {
            isStateChangeBlocked = true;
            Vector3 currentDestination = Vector3.zero;
            while (PriorityTasks.Count > 0 || !IsArrived())
            {
                InvokeDoPriorityEvent();
                if (IsArrived() || agent.destination != currentDestination)
                {
                    PriorityTask highestPriorityTask = PriorityTasks.OrderByDescending(t => t.Priority).FirstOrDefault();
                    if (highestPriorityTask != null && agent.CalculatePath(highestPriorityTask.Destination, agent.path))
                    {
                        currentDestination = highestPriorityTask.Destination;
                        Destination = currentDestination;
                    }
                    PriorityTasks.Remove(highestPriorityTask);
                }
                yield return null;
            }
            isStateChangeBlocked = false;
            State = DefaultState;
        }
        private void HandleState(IEnumerator stateCoroutine)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(stateCoroutine);
        }
        private void CheckIfHasPursuedTarget()
        {
            if (PursuedTarget != null)
                State = EnemyState.Chase;
        }
    }
}