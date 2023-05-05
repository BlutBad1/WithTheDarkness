using MyConstants;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBaseNS
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class EnemyMovement : MonoBehaviour
    {

        public GameObject Player;
        public EnemyLineOfSightChecker LineOfSightChecker;
        [SerializeField]
        private Animator animator = null;
        public NavMeshTriangulation Triangulation = new NavMeshTriangulation();
        [Tooltip("Delayed status updates")]
        public float UpdateRate = 0.1f;
        [HideInInspector]
        public NavMeshAgent Agent;
        private AgentLinkMover linkMover;
        [SerializeField]
        public EnemyState DefaultState;
        private EnemyState _state;
        public EnemyState State
        {
            get
            {
                return _state;
            }
            set
            {
                OnStateChange?.Invoke(_state, value);
                _state = value;
            }
        }
        public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);
        public StateChangeEvent OnStateChange;

        public float IdleMovespeedMultiplier = 0.5f;

        [HideInInspector]
        public Vector3 DefaultPositon;
        private Coroutine followCoroutine;
        public delegate void OngoingStateEvent();
        public OngoingStateEvent OnIdle;
        public OngoingStateEvent OnFollow;
        public OngoingStateEvent OnPatrol;
        private void Awake()
        {
            DefaultPositon = transform.position;
            Agent = GetComponent<NavMeshAgent>();
            linkMover = GetComponent<AgentLinkMover>();
            linkMover.OnLinkStart += HandleLinkStart;
            linkMover.OnLinkEnd += HandleLinkEnd;
            if (LineOfSightChecker == null)
                LineOfSightChecker = GetComponent<EnemyLineOfSightChecker>();
            LineOfSightChecker.OnGainSight += HandleGainSight;
            LineOfSightChecker.OnLoseSight += HandleLoseSight;
            OnStateChange += HandleStateChange;
            HandleStateChange(State, DefaultState);
            if(!Player)
            Player = GameObject.Find(CommonConstants.PLAYER);
        }
        protected virtual void Start()
        {
            if (!animator)
                TryGetComponent(out animator);
        }
        protected virtual void HandleGainSight(GameObject player)
        {
            State = EnemyState.Chase;
        }

        protected virtual void HandleLoseSight(GameObject player)
        {

            State = DefaultState;
        }



        private void HandleLinkStart(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
            {
                animator.SetBool(EnemyConstants.IS_WALKING, true);
            }
            else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
            {
                animator.SetTrigger(EnemyConstants.JUMP);
            }
        }

        private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
            {
                animator.SetTrigger(EnemyConstants.LANDED);
            }
        }

        private void Update()
        {

            if (!Agent.isOnOffMeshLink)
            {
                if (!animator)
                    Debug.LogWarning("Enemy movement animator is not set!");
                animator?.SetBool(EnemyConstants.IS_WALKING, Agent.velocity.magnitude > 0.01f);
            }

        }
        public virtual bool BackToDefaultPosition()
        {


            Agent.Warp(DefaultPositon);
            return true;

        }
        protected virtual void HandleStateChange(EnemyState oldState, EnemyState newState)
        {

            if (oldState != newState && oldState != EnemyState.Dead)
            {

                if (followCoroutine != null)
                {
                    StopCoroutine(followCoroutine);
                }

                if (oldState == EnemyState.Idle)
                {
                    Agent.speed /= IdleMovespeedMultiplier;
                }

                switch (newState)
                {
                    case EnemyState.Idle:
                        Agent.speed *= IdleMovespeedMultiplier;
                        followCoroutine = StartCoroutine(DoIdleMotion());
                        break;
                    case EnemyState.Patrol:
                        followCoroutine = StartCoroutine(DoPatrolMotion());
                        break;
                    case EnemyState.Chase:
                        followCoroutine = StartCoroutine(FollowTarget());
                        break;
                    case EnemyState.Dead:
                        DefaultState = EnemyState.Dead;
                        followCoroutine = StartCoroutine(DeadCoroutine());
                        break;
                }
            }
        }
        protected virtual IEnumerator DeadCoroutine()
        {

            while (true)
            {
                Agent.enabled = false;
                yield return null;
            }
        }
        protected virtual IEnumerator DoIdleMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(UpdateRate);
            Agent.speed *= IdleMovespeedMultiplier;

            while (true)
            {

                OnIdle?.Invoke();
                yield return Wait;
            }
        }

        protected virtual IEnumerator DoPatrolMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

            yield return new WaitUntil(() => Agent.enabled && Agent.isOnNavMesh);


            while (true)
            {
                OnPatrol?.Invoke();

                yield return Wait;
            }
        }

        protected virtual IEnumerator FollowTarget()
        {


            while (true)
            {
                if (Agent.enabled)
                {
                    OnFollow?.Invoke();
                    Agent.SetDestination(Player.transform.position);

                }
                yield return null;
            }

        }


    }
}