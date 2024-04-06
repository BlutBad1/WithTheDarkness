using AYellowpaper;
using EnemyConstantsNS;
using EntityNS.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Base
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public abstract class EntityMovement : MonoBehaviour, IEntityMovement
    {
        [SerializeField, RequireInterface(typeof(IStateHandler))]
        private MonoBehaviour stateHandler;
        [SerializeField]
        private Animator animator = null;
        [SerializeField]
        protected NavMeshTriangulation triangulation = new NavMeshTriangulation();
        [SerializeField]
        private EntityState defaultState;

        protected Vector3 defaultPositon;
        private NavMeshAgent agent;
        private AgentLinkMover linkMover;

        public NavMeshAgent Agent
        {
            get { return agent == null ? agent = GetComponent<NavMeshAgent>() : agent; }
        }
        public EntityState CurrentState { get => StateHandler.CurrentState; set => StateHandler.CurrentState = value; }
        public Animator Animator { get => animator; }
        public EntityState DefaultState { get => defaultState; }
        protected IStateHandler StateHandler { get => (IStateHandler)stateHandler; }

        private void Awake()
        {
            defaultPositon = transform.position;
            linkMover = GetComponent<AgentLinkMover>();
            linkMover.OnLinkStart += HandleLinkStart;
            linkMover.OnLinkEnd += HandleLinkEnd;
            StateHandler.DefaultState = defaultState;
            CurrentState = defaultState;
        }
        protected virtual void Start()
        {
            if (!animator)
                TryGetComponent(out animator);
        }
        private void Update()
        {
            WalkAnimation();
        }
        protected virtual void OnDisable()
        {
            if (CurrentState != EntityState.Dead && gameObject.scene.IsValid())
                BackToDefaultPosition();
        }
        public virtual void BackToDefaultPosition() =>
          Agent.Warp(defaultPositon);
        protected virtual void WalkAnimation()
        {
            if (!Agent.isOnOffMeshLink)
            {
                if (animator)
                    animator?.SetBool(EntityConstants.IS_WALKING, Agent.velocity.magnitude > 0.01f);
            }
        }
        private void HandleLinkStart(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
                animator.SetBool(EntityConstants.IS_WALKING, true);
            else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
                animator.SetTrigger(EntityConstants.JUMP);
        }
        private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
                animator.SetTrigger(EntityConstants.LANDED);
        }
    }
}