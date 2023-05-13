using EnemyBaseNS;
using UnityEngine;

namespace EnemyOnDeadNS
{
    [RequireComponent(typeof(Enemy))]
    public class DeadEvent : MonoBehaviour
    {
        protected Enemy enemy;
        protected virtual void Start()
        {
            enemy = GetComponent<Enemy>();
            enemy.OnDeath += OnDead;
        }
        public virtual void OnDead()
        {
            if (enemy.skillCoroutine != null)
                enemy.StopCoroutine(enemy.skillCoroutine);
            enemy.Movement.State = EnemyState.Dead;
            enemy.Agent.enabled = false;
            enemy.EnemyAttack.StopAttack();
            enemy.EnemyAttack.enabled = false;
        }
    }
}