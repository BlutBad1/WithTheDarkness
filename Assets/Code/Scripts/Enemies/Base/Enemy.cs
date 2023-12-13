using EnemyNS.Attack;
using EnemyNS.Skills;
using MyConstants;
using MyConstants.CreatureConstants.EnemyConstants;
using ScriptableObjectNS.Enemy;
using System.Collections;
using UnityEngine;
using DamageableNS;
namespace EnemyNS.Base
{
    public class Enemy : Damageable
    {
        public Animator Animator;
        public EnemyMovement Movement;
        public EnemyAttack EnemyAttack;
        public EnemyScriptableObject EnemyScriptableObject;
        [HideInInspector]
        public SkillScriptableObject[] Skills;
        public EnemySize EnemySize;
        public Coroutine skillCoroutine;
        private Coroutine lookCoroutine;
        protected new virtual void Start()
        {
            base.Start();
            if (EnemyAttack)
                EnemyAttack.OnAttack += OnAttack;
            //if (!NearbyCreatures)
            //    NearbyCreatures = GameObject.Find(CommonConstants.PLAYER);
            if (EnemyScriptableObject != null)
                SetupAgentFromConfiguration();
        }
        private void Update()
        {
            if (Skills != null)
            {
                for (int i = 0; i < Skills.Length; i++)
                {
                    if (Movement.PursuedTarget)
                    {
                        if (Skills[i].CanUseSkill(this, Movement.PursuedTarget))
                            Skills[i].UseSkill(this, Movement.PursuedTarget);
                    }
                }
            }
        }
        public virtual void SetupAgentFromConfiguration()
        {
            Movement.Agent.acceleration = EnemyScriptableObject.Acceleration;
            Movement.Agent.angularSpeed = EnemyScriptableObject.AngularSpeed;
            Movement.Agent.areaMask = EnemyScriptableObject.AreaMask;
            Movement.Agent.avoidancePriority = EnemyScriptableObject.AvoidancePriority;
            Movement.Agent.baseOffset = EnemyScriptableObject.BaseOffset;
            Movement.Agent.height = EnemyScriptableObject.Height;
            Movement.Agent.obstacleAvoidanceType = EnemyScriptableObject.ObstacleAvoidanceType;
            Movement.Agent.radius = EnemyScriptableObject.Radius;
            Movement.Agent.speed = EnemyScriptableObject.Speed;
            Movement.Agent.stoppingDistance = EnemyScriptableObject.StoppingDistance;
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
        protected virtual void OnAttack(IDamageable Target)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(LookAt(Target.GetGameObject().transform));
            Animator.SetTrigger(MainEnemyConstants.ATTACK_TRIGGER);
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
    }
}