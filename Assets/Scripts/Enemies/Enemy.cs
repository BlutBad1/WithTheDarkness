using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy :  MonoBehaviour, IDamageable
{
  //  public AttackRadius AttackRadius;
    public Animator Animator;
    public GameObject Player;
    public EnemyMovement Movement;
    public EnemyAttack EnemyAttack;
    public NavMeshAgent Agent;
    public EnemyScriptableObject EnemyScriptableObject;
    [HideInInspector]
    public int Health = 100;
    [HideInInspector]
    public SkillScriptableObject[] Skills;
    public bool knockoutEnable =false, knockoutPhysics;
    public float force;
    [HideInInspector]
    public bool isInKnockout = false;
    public float inKnockoutTime = 0.3f;
    public bool doesHaveKnockout { get; set; }
    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";
    private const string PLAYER = "Player";


    private void Awake()
    {
        EnemyAttack.OnAttack += OnAttack;
        doesHaveKnockout = knockoutEnable;
        Player = GameObject.Find(PLAYER);
        
    }
   
    private void Update()
    {
        for (int i = 0; i < Skills.Length; i++)
        {
          
            if (Skills[i].CanUseSkill(this,Player))
            {
               
                Skills[i].UseSkill(this, Player);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new Ray(transform.position, Player.transform.position - transform.position);
        Gizmos.DrawWireSphere(ray.origin + ray.direction * (Player.transform.position - transform.position).magnitude, 0.6f);
    }
    protected virtual void OnAttack(IDamageable Target)
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
        Animator.SetTrigger(ATTACK_TRIGGER);

       
     
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }


    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }
    public virtual void SetupAgentFromConfiguration()
    {
        Agent.acceleration = EnemyScriptableObject.Acceleration;
        Agent.angularSpeed = EnemyScriptableObject.AngularSpeed;
        Agent.areaMask = EnemyScriptableObject.AreaMask;
        Agent.avoidancePriority = EnemyScriptableObject.AvoidancePriority;
        Agent.baseOffset = EnemyScriptableObject.BaseOffset;
        Agent.height = EnemyScriptableObject.Height;
        Agent.obstacleAvoidanceType = EnemyScriptableObject.ObstacleAvoidanceType;
        Agent.radius = EnemyScriptableObject.Radius;
        Agent.speed = EnemyScriptableObject.Speed;
        Agent.stoppingDistance = EnemyScriptableObject.StoppingDistance;
        Health = EnemyScriptableObject.Health;
        EnemyAttack.AttackRadius = EnemyScriptableObject.AttackRadius;
        EnemyAttack.AttackDistance = EnemyScriptableObject.AttackDistance;
        EnemyAttack.AttackDelay = EnemyScriptableObject.AttackDelay;
        EnemyAttack.Damage = EnemyScriptableObject.Damage;

        Skills = EnemyScriptableObject.Skills;
    }

    public virtual void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual Transform GetTransform()
    {
        return transform;
    }

    public virtual void TakeDamage(int damage, RaycastHit hit)
    {
        if (doesHaveKnockout)
        {
            Vector3 moveDirection = transform.position - hit.point;
            TakeDamage((int)damage);
            if (!isInKnockout)
            {
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
                isInKnockout = true;
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                if (knockoutPhysics)
                    GetComponent<Rigidbody>().AddForce(moveDirection.normalized * force, ForceMode.Impulse);
              
                StartCoroutine(KnockBackTimer());

            }


        }

    }
    IEnumerator KnockBackTimer()
    {
        float timeElapsed = 0f;
        while (timeElapsed < inKnockoutTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;

        }

        GetComponent<NavMeshAgent>().enabled = true;
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.isKinematic = true;

        isInKnockout = false;
    }
}