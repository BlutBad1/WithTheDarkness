using EntityNS.Skills;
using UnityEngine;
using UnityEngine.AI;

namespace ScriptableObjectNS.Enemy
{
    public enum EntitySize
    {
        Small, Medium, Large
    }
    [CreateAssetMenu(fileName = "Entity Configuration", menuName = "ScriptableObject/Entity/Entity Configuration")]
    public class EntityScriptableObject : ScriptableObject
    {
        [Header("Entity Stats")]
        public int Health = 100;
        public float AttackDelay = 1f;
        public int Damage = 5;
        public float AttackForce = 5f;
        public float AttackRadius = 1.5f;
        public float AttackDistance = 1.5f;
        public EntitySize EntitySize;
        public SkillScriptableObject[] Skills;
        [Header("NavMeshAgent Configs")]
        public float AIUpdateInterval = 0.1f;
        public float Acceleration = 8;
        public float AngularSpeed = 120;
        [Tooltip("-1 means everything")]
        public int AreaMask = -1;
        public int AvoidancePriority = 50;
        public float BaseOffset = 0;
        public float Height = 2f;
        public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        public float Radius = 0.5f;
        public float Speed = 3f;
        public float StoppingDistance = 0.5f;
    }
}