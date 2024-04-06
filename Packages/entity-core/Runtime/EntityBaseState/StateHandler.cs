using UnityEngine;

namespace EntityNS.Base
{
    public class StateInfo
    {
        public GameObject PursuedTarget;
        public EntityState State;
        public StateInfo(GameObject pursuedTarget, EntityState state)
        {
            PursuedTarget = pursuedTarget;
            State = state;
        }
    }
    public abstract class StateHandler : MonoBehaviour, IStateHandler
    {
        private EntityState state;

        public abstract GameObject PursuedTarget
        {
            get;
            set;
        }
        public EntityState CurrentState
        {
            get => state;
            set
            {
                OnStateChange?.Invoke(state, value);
                state = value;
            }
        }
        public EntityState DefaultState { get; set; }
        public abstract Vector3 Destination { get; set; }

        public delegate void StateChangeEvent(EntityState oldState, EntityState newState);
        public event StateChangeEvent OnStateChange;
        public delegate void StateEvent();
        public delegate bool PriorityEvent();
        public event StateEvent OnIdle;
        public event StateEvent OnPatrol;
        public event StateEvent OnInvestigate;
        public event PriorityEvent OnDoPriority;
        public event StateEvent OnDoLostTarget;
        public event StateEvent OnFollow;

        protected virtual void OnEnable()
        {
            OnStateChange += HandleNewState;
        }
        protected virtual void OnDisable()
        {
            OnStateChange -= HandleNewState;
        }
        protected abstract void HandleNewState(EntityState oldState, EntityState newState);
        public abstract StateInfo GenerateStateInfo();
        public abstract bool IsArrived();
        protected void InvokeIdleEvent() =>
            InvokeEvent(OnIdle);
        protected void InvokePatrolEvent() =>
            InvokeEvent(OnPatrol);
        protected void InvokeInvestigateEvent() =>
            InvokeEvent(OnInvestigate);
        protected bool InvokeDoPriorityEvent() =>
            (bool)(OnDoPriority?.Invoke());
        protected void InvokeDoLostTargetEvent() =>
           InvokeEvent(OnDoLostTarget);
        protected void InvokeOnFollowEvent() =>
           InvokeEvent(OnFollow);
        private void InvokeEvent(StateEvent stateEvent) =>
          stateEvent?.Invoke();
    }
}