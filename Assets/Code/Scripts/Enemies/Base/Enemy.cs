using EnemyAttackNS;
using EnemySkillsNS;
using MyConstants;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBaseNS
{
    public class Enemy : Damageable
    {
        public Animator Animator;
        public GameObject Player;
        public EnemyMovement Movement;
        public EnemyAttack EnemyAttack;
        public NavMeshAgent Agent;
        public EnemyScriptableObject EnemyScriptableObject;
        [HideInInspector]
        public SkillScriptableObject[] Skills;
        public EnemySize EnemySize;
        private Coroutine lookCoroutine;
        public Coroutine skillCoroutine;
        private void Awake()
        {
            if (EnemyAttack)
                EnemyAttack.OnAttack += OnAttack;
            if (!Player)
                Player = GameObject.Find(CommonConstants.PLAYER);
            if (EnemyScriptableObject != null)
                SetupAgentFromConfiguration();
        }
        private void Update()
        {
            if (Skills != null)
            {
                for (int i = 0; i < Skills.Length; i++)
                {
                    if (Skills[i].CanUseSkill(this, Player))
                        Skills[i].UseSkill(this, Player);
                }
            }
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Ray ray = new Ray(transform.position, Player.transform.position - transform.position);
        //    Gizmos.DrawWireSphere(ray.origin + ray.direction * (Player.transform.position - transform.position).magnitude, 0.6f);
        //}
        protected virtual void OnAttack(IDamageable Target)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
            Animator.SetTrigger(EnemyConstants.ATTACK_TRIGGER);
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
        //public virtual void OnEnable()
        //{
        //    if (EnemyScriptableObject!=null)
        //     SetupAgentFromConfiguration();
        //}
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
            EnemySize = EnemyScriptableObject.EnemySize;
            EnemyAttack.AttackRadius = EnemyScriptableObject.AttackRadius;
            EnemyAttack.AttackDistance = EnemyScriptableObject.AttackDistance;
            EnemyAttack.AttackDelay = EnemyScriptableObject.AttackDelay;
            EnemyAttack.Damage = EnemyScriptableObject.Damage;
            EnemyAttack.AttackForce = EnemyScriptableObject.AttackForce;
            //NOTE: It's instantiate all skills and all enemies that have simillar skill can use it independently, but it might take a lot of resources
            //Skills = EnemyScriptableObject.Skills;
            Skills = new SkillScriptableObject[EnemyScriptableObject.Skills.Length];
            for (int i = 0; i < EnemyScriptableObject.Skills.Length; i++)
                Skills[i] = Instantiate(EnemyScriptableObject.Skills[i]);
        }
    }
}