using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Base
{
    public class EntityStateHandler : StateHandler
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

        protected SpotTarget SpotTarget { get => spotTarget; }

        public override GameObject PursuedTarget
        {
            get => pursuedTarget;
            set
            {
                pursuedTarget = value;
                if (pursuedTarget)
                    CurrentState = EntityState.Chase;
                else
                    CurrentState = DefaultState;
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
        protected override void HandleNewState(EntityState oldState, EntityState newState)
        {
            if (oldState != newState && oldState != EntityState.Dead)
            {
                if (!isStateChangeBlocked || newState == EntityState.Dead)
                {
                    switch (newState)
                    {
                        case EntityState.Idle:
                            HandleState(DoIdleMotion());
                            break;
                        case EntityState.Investigate:
                            HandleState(DoInvestigateMotion());
                            break;
                        case EntityState.Patrol:
                            HandleState(DoPatrolMotion());
                            break;
                        case EntityState.Chase:
                            HandleState(DoFollowTarget());
                            break;
                        case EntityState.Dead:
                            DefaultState = EntityState.Dead;
                            HandleState(DeadCoroutine());
                            break;
                        case EntityState.DoPriority:
                            HandleState(DoPriority());
                            break;
                        case EntityState.LostTarget:
                            HandleState(DoLostTarget());
                            break;
                        case EntityState.UsingAbility:
                            break;
                        default:
                            CurrentState = DefaultState;
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
                CurrentState = EntityState.LostTarget;
            }
        }
        public override StateInfo GenerateStateInfo() =>
            new StateInfo(PursuedTarget, CurrentState);
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
            CurrentState = DefaultState;
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
            CurrentState = DefaultState;
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
            CurrentState = DefaultState;
        }
        protected virtual IEnumerator DoPriority()
        {
            isStateChangeBlocked = true;
            while (InvokeDoPriorityEvent())
                yield return null;
            isStateChangeBlocked = false;
            CurrentState = DefaultState;
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
                CurrentState = EntityState.Chase;
        }
    }
}