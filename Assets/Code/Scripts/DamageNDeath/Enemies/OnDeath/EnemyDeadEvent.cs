using EnemyNS.Base;
using UnityEngine;

namespace EnemyNS.Death
{
    public class EnemyDeadEvent : MonoBehaviour
    {
        public Enemy Enemy;
        protected virtual void Start()
        {
            if (!Enemy)
                Enemy = GetComponent<Enemy>();
            Enemy.OnDead += OnDead;
        }
        public virtual void OnDead()
        {
            if (Enemy.skillCoroutine != null)
                Enemy.StopCoroutine(Enemy.skillCoroutine);
            if (Enemy.Movement.LineOfSightChecker)
                Enemy.Movement.LineOfSightChecker.enabled = false;
            Enemy.Movement.State = EnemyState.Dead;
            Enemy.Movement.Agent.enabled = false;
            Enemy.EnemyAttack.StopAttack();
            Enemy.EnemyAttack.enabled = false;
            Enemy.Movement.enabled = false;
        }
    }
}