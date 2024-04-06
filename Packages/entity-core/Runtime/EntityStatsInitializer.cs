using EntityNS.Attack;
using EntityNS.Skills;
using ScriptableObjectNS.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Base
{
    public class EntityStatsInitializer : MonoBehaviour
    {
        [SerializeField]
        private EntityScriptableObject entityScriptableObject;
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        [SerializeField]
        private EntityAttack entityAttack;
        [SerializeField]
        private EntityUseSkills entityUseSkills;

        private void Start()
        {
            SetupAgentFromConfiguration();
        }
        public virtual void SetupAgentFromConfiguration()
        {
            agent.acceleration = entityScriptableObject.Acceleration;
            agent.angularSpeed = entityScriptableObject.AngularSpeed;
            agent.areaMask = entityScriptableObject.AreaMask;
            agent.avoidancePriority = entityScriptableObject.AvoidancePriority;
            agent.baseOffset = entityScriptableObject.BaseOffset;
            agent.height = entityScriptableObject.Height;
            agent.obstacleAvoidanceType = entityScriptableObject.ObstacleAvoidanceType;
            agent.radius = entityScriptableObject.Radius;
            agent.speed = entityScriptableObject.Speed;
            agent.stoppingDistance = entityScriptableObject.StoppingDistance;
            entityBehaviour.Health = entityScriptableObject.Health;
            entityBehaviour.EntitySize = entityScriptableObject.EntitySize;
            entityAttack.AttackRadius = entityScriptableObject.AttackRadius;
            entityAttack.AttackDistance = entityScriptableObject.AttackDistance;
            entityAttack.AttackDelay = entityScriptableObject.AttackDelay;
            entityAttack.Damage = entityScriptableObject.Damage;
            entityAttack.AttackForce = entityScriptableObject.AttackForce;
            //NOTE: It's instantiate all skills and all enemies that have simillar skill can use it independently, but it might take a lot of resources
            //Skills = EnemyScriptableObject.Skills;
            if (entityUseSkills)
            {
                entityUseSkills.Skills = new SkillScriptableObject[entityScriptableObject.Skills.Length];
                for (int i = 0; i < entityScriptableObject.Skills.Length; i++)
                    entityUseSkills.Skills[i] = Instantiate(entityScriptableObject.Skills[i]);
            }
        }
    }
}