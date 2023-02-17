using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
      public Transform Player;
    public EnemyLineOfSightChecker LineOfSightChecker;
    public NavMeshTriangulation Triangulation = new NavMeshTriangulation();
    public float UpdateRate = 0.1f;
    [HideInInspector]
    public NavMeshAgent Agent;
    private AgentLinkMover LinkMover;
    [SerializeField]
    private Animator Animator = null;


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
    public float IdleLocationRadius = 4f;
    public float IdleMovespeedMultiplier = 0.5f;
    public Vector3[] Waypoints = new Vector3[4];
    [SerializeField]
    private int WaypointIndex = 0;

    private const string IsWalking = "IsWalking";
    public const string Jump = "Jump";
    public const string Landed = "Landed";
    [HideInInspector]
    public Vector3 walkPoint;
    [HideInInspector]
    public bool walkPointIsSet;
    [HideInInspector]
    public Vector3 defaultPositon;
    private Coroutine FollowCoroutine;
 
    private void Awake()
    {
        defaultPositon = transform.position;
        Agent = GetComponent<NavMeshAgent>();
        LinkMover = GetComponent<AgentLinkMover>();

        LinkMover.OnLinkStart += HandleLinkStart;
        LinkMover.OnLinkEnd += HandleLinkEnd;

        LineOfSightChecker.OnGainSight += HandleGainSight;
        LineOfSightChecker.OnLoseSight += HandleLoseSight;
        OnStateChange += HandleStateChange;
        HandleStateChange(State, DefaultState);
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i].Set(transform.position.x, transform.position.y, transform.position.z);
        }
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
            Animator.SetBool(IsWalking, true);
        }
        else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
        {
            Animator.SetTrigger(Jump);
        }
    }

    private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
    {
        if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
        {
            Animator.SetTrigger(Landed);
        }
    }

    private void Update()
    {
        if (!Agent.isOnOffMeshLink)
        {
            Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.01f);
        }
        if (State== EnemyState.Idle)
        {
            BackToDefaultPosition();
        }
    }
    public virtual void BackToDefaultPosition()
    {
        if ((transform.position - Player.transform.position).magnitude > 50)
        {
            walkPointIsSet = false;
            Agent.Warp(defaultPositon);
        }
    }
    public virtual void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (oldState != newState)
        {

            if (FollowCoroutine != null)
            {
                StopCoroutine(FollowCoroutine);
            }

            if (oldState == EnemyState.Idle)
            {
                Agent.speed /= IdleMovespeedMultiplier;
            }

            switch (newState)
            {
                case EnemyState.Idle:
                    Agent.speed *= IdleMovespeedMultiplier;
                    FollowCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                case EnemyState.Patrol:
                    FollowCoroutine = StartCoroutine(DoPatrolMotion());
                    break;
                case EnemyState.Chase:
                    FollowCoroutine = StartCoroutine(FollowTarget());
                    break;
            }
        }
    }

    protected virtual IEnumerator DoIdleMotion()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        Agent.speed *= IdleMovespeedMultiplier;

        while (true)
        {
            if (!Agent.enabled || !Agent.isOnNavMesh)
            {
                yield return Wait;
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Vector2 point = Random.insideUnitCircle * IdleLocationRadius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(Agent.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, Agent.areaMask))
                {
                    Agent.SetDestination(hit.position);
                }
            }

            yield return Wait;
        }
    }

    protected virtual IEnumerator DoPatrolMotion()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        yield return new WaitUntil(() => Agent.enabled && Agent.isOnNavMesh);
        Agent.SetDestination(Waypoints[WaypointIndex]);

        while (true)
        {
            if (Agent.isOnNavMesh && Agent.enabled && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                WaypointIndex++;

                if (WaypointIndex >= Waypoints.Length)
                {
                    WaypointIndex = 0;
                }

                Agent.SetDestination(Waypoints[WaypointIndex]);
            }

            yield return Wait;
        }
    }

    protected virtual IEnumerator FollowTarget()
    {

       
            while (true)
            {
            if (Agent.enabled)
            {
                Agent.SetDestination(Player.transform.position);

            }
            yield return null;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(Waypoints[i], 0.25f);
            if (i + 1 < Waypoints.Length)
            {
                Gizmos.DrawLine(Waypoints[i], Waypoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(Waypoints[i], Waypoints[0]); 
            }
        }
    }
}
