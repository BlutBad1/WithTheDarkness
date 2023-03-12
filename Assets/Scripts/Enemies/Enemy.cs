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
    public SkillScriptableObject[] Skills;
    private Coroutine lookCoroutine;
    protected const string ATTACK_TRIGGER = "Attack";
    protected const string DEATH_TRIGGER = "Death";
    protected const string PLAYER = "Player";
    [SerializeField]
    private RagdollEnabler ragdollEnabler;
    public delegate void TakeDamageEvent(GunData weapon, RaycastHit hit);
    public TakeDamageEvent OnTakeDamage;
    public delegate void DeathEvent();
    public DeathEvent OnDeath;
    [HideInInspector]
    public int Health = 100;
    [Tooltip("Time while object in ragdoll.")]
    public float RagdollTime=1f;
    [Tooltip("Delay to fade out.")]
    public float FadeOutDelay=1f;
    [Tooltip("Fade out speed.")]
    public float FadeOutSpeed = 0.05f;
    private void Awake()
    {
        
        EnemyAttack.OnAttack += OnAttack;
      
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
        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }
       
            lookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
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

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
       
        if (Health <= 0)
        {
            Agent.enabled = false;
            Movement.enabled = false;
            //EnemyAttack.enabled = false;
            OnDeath?.Invoke();
            ragdollEnabler.EnableRagdoll();
            StartCoroutine(FadeOut());
        }
    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual Transform GetTransform()
    {
        return transform;
    }

    public virtual void TakeDamage(GunData weapon, RaycastHit hit)
    {
        TakeDamage(weapon.damage);
        OnTakeDamage?.Invoke(weapon, hit);


    }
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(RagdollTime);
        if (ragdollEnabler != null)
        {
            ragdollEnabler.DisableAllRigidbodies();
        }
        yield return new WaitForSeconds(FadeOutDelay);



        float time = 0;
        while (time < 1)
        {
            transform.position += (Vector3.down * Time.deltaTime) * FadeOutSpeed;
            time += Time.deltaTime * FadeOutSpeed;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}