using AYellowpaper;
using EnemyNS.Navigation;
using MyConstants.CreatureConstants.EnemyConstants;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyNS.Base
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IStateHandler))]
        private MonoBehaviour stateHandler;
        [SerializeField, FormerlySerializedAs("Animator")]
        private Animator animator = null;
        [SerializeField, FormerlySerializedAs("Triangulation")]
        protected NavMeshTriangulation triangulation = new NavMeshTriangulation();
        [SerializeField, FormerlySerializedAs("DefaultState")]
        private EnemyState defaultState;

        protected Vector3 defaultPositon;
        private NavMeshAgent agent;
        private AgentLinkMover linkMover;

        public NavMeshAgent Agent
        {
            get { return agent == null ? agent = GetComponent<NavMeshAgent>() : agent; }
        }
        public EnemyState State { get => StateHandler.GetState(); set => StateHandler.SetState(value); }
        public Animator Animator { get => animator; }
        public EnemyState DefaultState { get => defaultState; }
        protected IStateHandler StateHandler { get => (IStateHandler)stateHandler; }
        private void Awake()
        {
            defaultPositon = transform.position;
            linkMover = GetComponent<AgentLinkMover>();
            linkMover.OnLinkStart += HandleLinkStart;
            linkMover.OnLinkEnd += HandleLinkEnd;
            StateHandler.SetDefaultState(defaultState);
            State = defaultState;
        }
        protected virtual void Start()
        {
            if (!animator)
                TryGetComponent(out animator);
        }
        private void Update()
        {
            if (!Agent.isOnOffMeshLink)
            {
#if UNITY_EDITOR
                if (!animator)
                    Debug.LogWarning("Enemy movement animator is not set!");
#endif
                animator?.SetBool(MainEnemyConstants.IS_WALKING, Agent.velocity.magnitude > 0.01f);
            }
        }
        protected virtual void OnDisable()
        {
            if (State != EnemyState.Dead && gameObject.scene.IsValid())
                BackToDefaultPosition();
        }
        public virtual void BackToDefaultPosition() =>
          Agent.Warp(defaultPositon);
        private void HandleLinkStart(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
                animator.SetBool(MainEnemyConstants.IS_WALKING, true);
            else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
                animator.SetTrigger(MainEnemyConstants.JUMP);
        }
        private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
                animator.SetTrigger(MainEnemyConstants.LANDED);
        }
    }
}