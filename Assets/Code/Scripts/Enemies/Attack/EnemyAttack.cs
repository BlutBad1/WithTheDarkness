using EnemyBaseNS;
using System.Collections;
using UnityEngine;
namespace EnemyAttackNS
{
    public class EnemyAttack : MonoBehaviour
    {
        public int Damage = 10;
        public float AttackForce = 5f;
        public float AttackDelay = 0.5f;
        public float AttackDistance = 2f;
        public float AttackRadius = 2f;
        public delegate void AttackEvent(IDamageable Target);
        public AttackEvent OnAttack;
        protected IDamageable objectDamageable;
        protected Coroutine attackCoroutine;
        [HideInInspector]
        public bool IsAttacking = false;
        public virtual bool CanAttack(GameObject enemy, GameObject player)
        {
            if (enemy.GetComponent<EnemyMovement>().State == EnemyState.Chase)
                return true;
            return false;
        }
        public virtual void TryAttack() =>
            IsAttacking = true;
        public virtual void StopAttack()
        {
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            IsAttacking = false;
        }

        protected virtual IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);
            while (objectDamageable != null)
            {
                OnAttack?.Invoke(objectDamageable);
                objectDamageable.TakeDamage(Damage);
                yield return Wait;
            }
        }
    }
}