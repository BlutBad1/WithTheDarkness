using EnemyNS.Base;
using UnityEngine;

namespace EnemyNS.Death
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyDeadEvent : MonoBehaviour
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
            if (enemy.Movement.LineOfSightChecker)
                enemy.Movement.LineOfSightChecker.enabled = false;
            enemy.Movement.State = EnemyState.Dead;
            enemy.Movement.Agent.enabled = false;
            enemy.EnemyAttack.StopAttack();
            enemy.EnemyAttack.enabled = false;
            enemy.Movement.enabled = false;
        }
    }
}