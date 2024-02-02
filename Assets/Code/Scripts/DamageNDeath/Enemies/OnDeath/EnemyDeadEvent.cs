using EnemyNS.Attack;
using EnemyNS.Base;
using EnemyNS.Base.StateBehaviourNS;
using EnemyNS.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Death
{
    public class EnemyDeadEvent : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("Enemy")]
        protected Enemy enemy;
        [SerializeField]
        protected EnemyMovement enemyMovement;
        [SerializeField]
        protected CreatureInSightChecker creatureInSightChecker;
        [SerializeField]
        protected EnemyAttack enemyAttack;
        [SerializeField]
        protected EnemyUseSkills enemyUseSkills;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected StateBehaviour[] stateBehaviours;
        protected virtual void OnEnable()
        {
            enemy.OnDead += OnDeadEvent;
        }
        protected virtual void OnDisable()
        {
            enemy.OnDead -= OnDeadEvent;
        }
        protected virtual void OnDeadEvent()
        {
            enemyUseSkills?.StopSkills();
            creatureInSightChecker.enabled = false;
            enemyMovement.State = EnemyState.Dead;
            enemyMovement.Agent.enabled = false;
            enemyAttack.StopAttack();
            enemyAttack.enabled = false;
            enemyMovement.enabled = false;
            enemy.enabled = false;
            animator.enabled = false;
            foreach (var behaviour in stateBehaviours)
                behaviour.enabled = false;
        }
    }
}