using EnemyNS.Attack;
using EnemyNS.Skills;
using ScriptableObjectNS.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Base
{
    public class EnemyStatsInitializer : MonoBehaviour
    {
        [SerializeField]
        private EnemyScriptableObject enemyScriptableObject;
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private Enemy enemy;
        [SerializeField]
        private EnemyAttack enemyAttack;
        [SerializeField]
        private EnemyUseSkills enemyUseSkills;

        private void Start()
        {
            SetupAgentFromConfiguration();
        }
        public virtual void SetupAgentFromConfiguration()
        {
            agent.acceleration = enemyScriptableObject.Acceleration;
            agent.angularSpeed = enemyScriptableObject.AngularSpeed;
            agent.areaMask = enemyScriptableObject.AreaMask;
            agent.avoidancePriority = enemyScriptableObject.AvoidancePriority;
            agent.baseOffset = enemyScriptableObject.BaseOffset;
            agent.height = enemyScriptableObject.Height;
            agent.obstacleAvoidanceType = enemyScriptableObject.ObstacleAvoidanceType;
            agent.radius = enemyScriptableObject.Radius;
            agent.speed = enemyScriptableObject.Speed;
            agent.stoppingDistance = enemyScriptableObject.StoppingDistance;
            enemy.Health = enemyScriptableObject.Health;
            enemy.EnemySize = enemyScriptableObject.EnemySize;
            enemyAttack.AttackRadius = enemyScriptableObject.AttackRadius;
            enemyAttack.AttackDistance = enemyScriptableObject.AttackDistance;
            enemyAttack.AttackDelay = enemyScriptableObject.AttackDelay;
            enemyAttack.Damage = enemyScriptableObject.Damage;
            enemyAttack.AttackForce = enemyScriptableObject.AttackForce;
            //NOTE: It's instantiate all skills and all enemies that have simillar skill can use it independently, but it might take a lot of resources
            //Skills = EnemyScriptableObject.Skills;
            if (enemyUseSkills)
            {
                enemyUseSkills.Skills = new SkillScriptableObject[enemyScriptableObject.Skills.Length];
                for (int i = 0; i < enemyScriptableObject.Skills.Length; i++)
                    enemyUseSkills.Skills[i] = Instantiate(enemyScriptableObject.Skills[i]);
            }
        }
    }
}