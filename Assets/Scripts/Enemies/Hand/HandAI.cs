
using UnityEngine;
using UnityEngine.AI;

public class HandAI : EnemyBase
{



    public  float health;
    public  float damage;
    [HideInInspector]
    public  NavMeshAgent enemy;
    [HideInInspector]
    public  Transform player;
    public  LayerMask whatIsGround;
    public  LayerMask whatIsPlayer;
    //Patroling
    [HideInInspector]
    public  Vector3 walkPoint;
    [HideInInspector]
    protected  bool walkPointSet;

    public  float walkPointRange;
    //Attacking
    public  float timeBetweenAttacks;
    [HideInInspector]
    protected  bool alreadyAttacked;
    //States
    public  float sightRange;
    public float hiddenSightRange;
    public  float attackRange;
    [HideInInspector]
    public  bool playerInSightRange;
    [HideInInspector]
    public  bool playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
      
    }

    private void Update()
    {
        //Check for sight and attack range
        if (!isInKnockout)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
      
    }

    protected override void Patroling()
    {
        
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            enemy.SetDestination(walkPoint);
       

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    protected override void SearchWalkPoint()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer<=hiddenSightRange)
        {
            walkPoint= player.position;
            walkPointSet = true;
        }
     
    }

    protected override void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        enemy.SetDestination(transform.position);

        transform.LookAt(player);

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
