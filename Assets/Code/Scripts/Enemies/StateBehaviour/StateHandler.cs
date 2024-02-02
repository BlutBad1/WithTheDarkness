using UnityEngine;

namespace EnemyNS.Base
{
    public class StateInfo
    {
        public GameObject PursuedTarget;
        public EnemyState State;
        public StateInfo(GameObject pursuedTarget, EnemyState state)
        {
            PursuedTarget = pursuedTarget;
            State = state;
        }
    }
    public abstract class StateHandler : MonoBehaviour, IStateHandler
    {
        private EnemyState state;

        public abstract GameObject PursuedTarget
        {
            get;
            set;
        }
        public EnemyState State { get => GetState(); set => SetState(value); }
        public EnemyState DefaultState { get; protected set; }
        public abstract Vector3 Destination { get; set; }

        public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);
        public event StateChangeEvent OnStateChange;
        public delegate void StateEvent();
        public event StateEvent OnIdle;
        public event StateEvent OnPatrol;
        public event StateEvent OnInvestigate;
        public event StateEvent OnDoPriority;
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
        public void SetState(EnemyState newState)
        {
            OnStateChange?.Invoke(state, newState);
            state = newState;
        }
        public EnemyState GetState() =>
             state;
        public void SetDefaultState(EnemyState newState) =>
            DefaultState = newState;
        protected abstract void HandleNewState(EnemyState oldState, EnemyState newState);
        public abstract StateInfo GenerateStateInfo();
        public abstract bool IsArrived();
        protected void InvokeIdleEvent() =>
            InvokeEvent(OnIdle);
        protected void InvokePatrolEvent() =>
            InvokeEvent(OnPatrol);
        protected void InvokeInvestigateEvent() =>
            InvokeEvent(OnInvestigate);
        protected void InvokeDoPriorityEvent() =>
            InvokeEvent(OnDoPriority);
        protected void InvokeDoLostTargetEvent() =>
           InvokeEvent(OnDoLostTarget);
        protected void InvokeOnFollowEvent() =>
           InvokeEvent(OnFollow);
        private void InvokeEvent(StateEvent stateEvent) =>
          stateEvent?.Invoke();
    }
}