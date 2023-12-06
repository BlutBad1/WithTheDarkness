using CreatureNS;
using EnemyNS.Attack;
using MyConstants.CreatureConstants;
using MyConstants.CreatureConstants.EnemyConstants;
using ScriptableObjectNS.Creature;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Base
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class EnemyMovement : MonoBehaviour, ICreature, ISerializationCallbackReceiver
    {
        //!!!!CREATURE!!!!
        [Header("Creature")]
        [HideInInspector]
        public static List<string> CreatureNames;
        [ListToPopup(typeof(EnemyMovement), "CreatureNames")]
        public string CreatureType;
        [ListToMultiplePopup(typeof(EnemyMovement), "CreatureNames")]
        public int OpponentsMask;
        public Dictionary<GameObject, ICreature> KnownCreatures;
        public Dictionary<GameObject, ICreature> CreaturesInSight;
        public Dictionary<GameObject, Damageable> Opponents;
        //!!!!MECHANICS VARIABLES!!!!
        [Header("Mechanics Variables")]
        [HideInInspector]
        public GameObject PursuedTarget;
        //Routes that enemy would follow in priority, than higher int that more priority its has
        public List<PriorityTask> PriorityTasks;
        public EnemyLineOfSightChecker LineOfSightChecker;
        public float DontLoseSightIfDistanceLess = 1f;
        public float IdleMovespeedMultiplier = 0.5f;
        [HideInInspector]
        private bool isStateChangeBlocked = false;
        [SerializeField]
        public Animator Animator = null;
        [HideInInspector]
        public Vector3 DefaultPositon;
        [HideInInspector]
        public Transform LastPursuedTargetPosition;
        //!!!!NAVMESH!!!!
        [Header("NavMesh")]
        public NavMeshTriangulation Triangulation = new NavMeshTriangulation();
        [HideInInspector]
        public NavMeshAgent Agent;
        private AgentLinkMover linkMover;
        //!!!!STATE!!!!
        [Header("State")]
        [Tooltip("State update delay")]
        public float UpdateRate = 0.1f;
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
        //!!!!DELEGATES!!!!
        public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);
        public event StateChangeEvent OnStateChange;
        public delegate void OngoingStateEvent();
        public event OngoingStateEvent OnIdle;
        public event OngoingStateEvent OnFollow;
        public event OngoingStateEvent OnPatrol;
        public event OngoingStateEvent OnDoPriority;
        public event OngoingStateEvent OnDoLostTarget;
        //!!!!COROUTINES!!!!
        private Coroutine followCoroutine;

        ///////UNITY METHODS///////
        private void Awake()
        {
            DefaultPositon = transform.position;
            Agent = GetComponent<NavMeshAgent>();
            linkMover = GetComponent<AgentLinkMover>();
            linkMover.OnLinkStart += HandleLinkStart;
            linkMover.OnLinkEnd += HandleLinkEnd;
            if (LineOfSightChecker == null)
                LineOfSightChecker = GetComponent<EnemyLineOfSightChecker>();
            if (LineOfSightChecker != null)
            {
                LineOfSightChecker.OnGainSight += HandleGainCreatureInSight;
                LineOfSightChecker.OnLoseSight += HandleLoseCreatureFromSight;
            }
            OnStateChange += HandleStateChange;
            State = DefaultState;
        }
        protected virtual void Start()
        {
            if (!Animator)
                TryGetComponent(out Animator);
            KnownCreatures = new Dictionary<GameObject, ICreature>();
            CreaturesInSight = new Dictionary<GameObject, ICreature>();
            Opponents = new Dictionary<GameObject, Damageable>();
            PriorityTasks = new List<PriorityTask>();
        }
        private void Update()
        {
            if (!Agent.isOnOffMeshLink)
            {
#if UNITY_EDITOR
                if (!Animator)
                    Debug.LogWarning("Enemy movement animator is not set!");
#endif
                Animator?.SetBool(MainEnemyConstants.IS_WALKING, Agent.velocity.magnitude > 0.01f);
            }
        }
        protected virtual void OnDisable()
        {
            if (State != EnemyState.Dead && gameObject.scene.IsValid())
                BackToDefaultPosition();
            //Opponents.Clear();
            //CreaturesInSight.Clear();
        }
        ///////////END///////////

        public GameObject GetClosestKnownCreature(IEnumerable objects)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject potentialTarget in objects)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            return bestTarget;
        }
        public GameObject TryGetNewTarget()
        {
            if (Opponents != null && Opponents.Count > 0)
            {
                foreach (var entry in Opponents.Where(entry => entry.Value.IsDead).ToList())
                    Opponents.Remove(entry.Key);
                Dictionary<GameObject, Damageable> creaturesOpponents = Opponents.Where
                (x => !x.Value.IsDead && x.Key != PursuedTarget && GetOpponentsList().Contains(ICreature.GetICreatureComponent(x.Key).GetCreatureName()))
                .ToDictionary(p => p.Key, p => p.Value);
                if (creaturesOpponents != null)
                    return GetClosestKnownCreature(creaturesOpponents.Keys.ToArray());
                else
                    return PursuedTarget;
            }
            else
                return null;
        }
        public List<string> GetOpponentsList()
        {
            List<string> list = new List<string>();
            if (CreatureType == MainCreatureConstants.ALONE_CREATURE_TYPE)
                list = CreatureNames;
            else
            {
                for (int i = 0; i < CreatureNames.Count; i++)
                {
                    if ((OpponentsMask & (1 << i)) != 0)
                        list.Add(CreatureNames[i]);
                }
            }
            return list;
        }
        public virtual void BackToDefaultPosition() =>
          Agent.Warp(DefaultPositon);
        public string GetCreatureName() =>
          CreatureType;
        public GameObject GetCreatureGameObject() =>
          gameObject;
        public void BlockMovement()
        {
            if (Agent)
                Agent.enabled = false;
        }
        public void UnBlockMovement()
        {
            if (Agent)
                Agent.enabled = true;
        }
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            BlockMovement();
            transform.position = position;
            transform.rotation = rotation;
            UnBlockMovement();
        }
        public void OnBeforeSerialize() =>
            CreatureNames = CreatureTypes.Instance.Names;
        public void OnAfterDeserialize()
        {
        }
        ///////SIGHT HANDLES///////
        public virtual void HandleGainCreatureInSight(GameObject spottedTarget)
        {
            ICreature creatureComponent = ICreature.GetICreatureComponent(spottedTarget);
            Damageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<Damageable>(spottedTarget);
            if (spottedTarget == gameObject || CreaturesInSight.ContainsValue(creatureComponent))
                return;
            if (creatureComponent != null)
            {
                if (!CreaturesInSight.ContainsKey(spottedTarget))
                    CreaturesInSight.Add(spottedTarget, creatureComponent);
                if (!KnownCreatures.ContainsKey(spottedTarget))
                    KnownCreatures.Add(spottedTarget, creatureComponent);
                if (GetOpponentsList().Contains(creatureComponent.GetCreatureName()))
                {
                    if (damageable != null && damageable.IsDead)
                        return;
                    if (!Opponents.ContainsKey(spottedTarget))
                        Opponents.Add(spottedTarget, damageable);
                    if (PursuedTarget && PursuedTarget != spottedTarget)
                    {
                        if (Vector3.Distance(transform.position, TryGetNewTarget().transform.position) <
                            Vector3.Distance(transform.position, PursuedTarget.transform.position))
                            PursuedTarget = TryGetNewTarget();
                    }
                    else
                        PursuedTarget = TryGetNewTarget();
                    State = EnemyState.Chase;
                }
            }
        }
        public virtual void HandleLoseCreatureFromSight(GameObject lostTarget)
        {
            if (lostTarget == PursuedTarget)
            {
                if (Vector3.Distance(gameObject.transform.position, PursuedTarget.transform.position) > DontLoseSightIfDistanceLess)
                {
                    Opponents.Remove(PursuedTarget);
                    LastPursuedTargetPosition = PursuedTarget.transform;
                    PursuedTarget = null;
                    State = EnemyState.LostTarget;
                }
                else
                    return;
            }
            CreaturesInSight.Remove(lostTarget);
        }
        public bool CheckIfAgentHasArrived()
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    return true;
            }
            return false;
        }
        ///////////END///////////

        //////STATE AND IENUMERATORS//////
        protected virtual void HandleStateChange(EnemyState oldState, EnemyState newState)
        {
            if (oldState != newState && oldState != EnemyState.Dead)
            {
                if (!isStateChangeBlocked || newState == EnemyState.Dead)
                {
                    if (followCoroutine != null)
                        StopCoroutine(followCoroutine);
                    if (oldState == EnemyState.Idle)
                        Agent.speed /= IdleMovespeedMultiplier;
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
                            followCoroutine = StartCoroutine(DoFollowTarget());
                            break;
                        case EnemyState.Dead:
                            DefaultState = EnemyState.Dead;
                            followCoroutine = StartCoroutine(DeadCoroutine());
                            break;
                        case EnemyState.DoPriority:
                            followCoroutine = StartCoroutine(DoPriority());
                            break;
                        case EnemyState.LostTarget:
                            followCoroutine = StartCoroutine(DoLostTarget());
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
        protected virtual IEnumerator DeadCoroutine()
        {
            while (true)
            {
                Agent.enabled = false;
                yield return null;
            }
        }
        protected virtual IEnumerator DoLostTarget()
        {
            if (LastPursuedTargetPosition)
            {
                WaitForSeconds Wait = new WaitForSeconds(UpdateRate);
                yield return new WaitUntil(() => Agent.enabled && Agent.isOnNavMesh);
                Agent.SetDestination(LastPursuedTargetPosition.position);
                while (Agent.enabled && Agent.isOnNavMesh && !CheckIfAgentHasArrived())
                {
                    if (TryGetNewTarget() && PursuedTarget != TryGetNewTarget())
                    {
                        PursuedTarget = TryGetNewTarget();
                        State = EnemyState.Chase;
                    }
                    OnDoLostTarget?.Invoke();
                    yield return Wait;
                }
                LastPursuedTargetPosition = null;
            }
            if (!PursuedTarget)
                State = DefaultState;
        }
        protected virtual IEnumerator DoIdleMotion()
        {
            WaitForSeconds Wait = new WaitForSeconds(UpdateRate);
            Agent.speed *= IdleMovespeedMultiplier;
            while (true)
            {
                if (TryGetNewTarget() && PursuedTarget != TryGetNewTarget())
                {
                    PursuedTarget = TryGetNewTarget();
                    State = EnemyState.Chase;
                }
                else if (LastPursuedTargetPosition)
                {
                    State = EnemyState.LostTarget;
                    break;
                }
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
                if (TryGetNewTarget() && PursuedTarget != TryGetNewTarget())
                {
                    PursuedTarget = TryGetNewTarget();
                    State = EnemyState.Chase;
                }
                else if (LastPursuedTargetPosition)
                {
                    State = EnemyState.LostTarget;
                    break;
                }
                OnPatrol?.Invoke();
                yield return Wait;
            }
        }
        protected virtual IEnumerator DoFollowTarget()
        {
            Damageable damageable = null;
            GameObject currentGameObject = null;
            while (PursuedTarget)
            {
                if (currentGameObject != PursuedTarget)
                {
                    damageable = (Damageable)IDamageable.GetDamageableFromGameObject(PursuedTarget);
                    currentGameObject = PursuedTarget;
                }
                if (damageable && damageable.IsDead)
                {
                    Opponents.Remove(PursuedTarget);
                    PursuedTarget = TryGetNewTarget() == PursuedTarget ? null : TryGetNewTarget();
                    if (!PursuedTarget)
                    {
                        State = DefaultState;
                        break;
                    }
                }
                OnFollow?.Invoke();
                if (Agent.enabled && Agent.isOnNavMesh)
                    Agent.SetDestination(PursuedTarget.transform.position);
                yield return null;
            }
            State = DefaultState;
        }
        protected virtual IEnumerator DoPriority()
        {
            isStateChangeBlocked = true;
            Vector3 currentDestination = Vector3.zero;
            while (PriorityTasks.Count > 0)
            {
                OnDoPriority?.Invoke();
                if (Agent.enabled && Agent.isOnNavMesh)
                {
                    if (Agent.remainingDistance <= Agent.stoppingDistance || Agent.destination != currentDestination || !Agent.CalculatePath(Agent.destination, Agent.path))
                    {
                        PriorityTask highestPriorityTask = PriorityTasks.OrderByDescending(t => t.Priority).FirstOrDefault();
                        if (Agent.CalculatePath(highestPriorityTask.Destination, Agent.path))
                        {
                            currentDestination = highestPriorityTask.Destination;
                            Agent.SetDestination(currentDestination);
                        }
                        PriorityTasks.Remove(highestPriorityTask);
                    }
                }
                yield return null;
            }
            isStateChangeBlocked = false;
            if (State == EnemyState.DoPriority)
                State = DefaultState;
        }
        ///////////END///////////
        private void HandleLinkStart(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
                Animator.SetBool(MainEnemyConstants.IS_WALKING, true);
            else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
                Animator.SetTrigger(MainEnemyConstants.JUMP);
        }
        private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
        {
            if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
                Animator.SetTrigger(MainEnemyConstants.LANDED);
        }
    }
}