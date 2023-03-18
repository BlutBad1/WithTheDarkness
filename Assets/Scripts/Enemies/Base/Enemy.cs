using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using EnemyAttackNS;
using EnemySkillsNS;

namespace EnemyBaseNS
{
    public class Enemy : Damageable
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
   
        private void Awake()
        {

            EnemyAttack.OnAttack += OnAttack;
            Player = GameObject.Find(PLAYER);

        }

        private void Update()
        {
            for (int i = 0; i < Skills.Length; i++)
            {

                if (Skills[i].CanUseSkill(this, Player))
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

        public override void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                OnDeath?.Invoke();
            

            }
        }

        public override void TakeDamage(int damage, float force, Vector3 hit)
        {
            TakeDamage(damage);
            OnTakeDamage?.Invoke(force, hit);
        }
     
    }
}