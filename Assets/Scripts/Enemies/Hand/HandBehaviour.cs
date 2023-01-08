using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class HandBehaviour : EnemyBase
{
    public float health;
    public Transform Player;
    [SerializeField]
    private Animator Animator;
 
    private NavMeshAgent Agent;
    private AgentLinkMover LinkMover;
    private const string IsWalking = "IsWalking";
    private const string Jump = "Jump";
    private const string Landed = "Landed";

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    //Patroling
    [HideInInspector]
    public Vector3 walkPoint;
    [HideInInspector]
    protected bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;
    [HideInInspector]
    protected bool alreadyAttacked;
    //States
    public float sightRange;
    public float hiddenSightRange;
    public float attackRange;
    [HideInInspector]
    public bool playerInSightRange;
    [HideInInspector]
    public bool playerInAttackRange;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        LinkMover = GetComponent<AgentLinkMover>();
        LinkMover.OnLinkEnd += HandleLinkEnd;
        LinkMover.OnLinkStart += HandleLinkStart;
    }
    void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void HandleLinkStart()
    {
        Animator.SetTrigger(Jump);
    }
    private void HandleLinkEnd()
    {
        Animator.SetTrigger(Landed);
    }
    private void Update()
    {
        Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.01f);
    }
    private IEnumerator FollowTarget()
    {
      
        while (enabled)
        {
            if (!isInKnockout)
            {
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if (!playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInAttackRange && playerInSightRange) AttackPlayer();
            }
            yield return null;
        }
    }
    protected override void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            Agent.SetDestination(walkPoint);


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    protected override void SearchWalkPoint()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer <= hiddenSightRange)
        {
            walkPoint = Player.position;
            walkPointSet = true;
        }

    }

    protected override void ChasePlayer()
    {
        Agent.SetDestination(Player.position);
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        Agent.SetDestination(transform.position);

        transform.LookAt(Player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    protected override void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void TakeDamage(float damage, RaycastHit hit)
    {
        base.TakeDamage(damage, hit);
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    protected override void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
