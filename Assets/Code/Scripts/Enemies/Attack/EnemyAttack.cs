using EnemyNS.Base;
using System.Collections;
using UnityEngine;
namespace EnemyNS.Attack
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttack : MonoBehaviour
    {
        public int Damage = 10;
        public float AttackForce = 5f;
        public float AttackDelay = 0.5f;
        public float AttackDistance = 2f;
        public float AttackRadius = 2f;
        public delegate void AttackEvent(IDamageable Target);
        public AttackEvent OnAttack;
        protected Coroutine attackCoroutine;
        protected Enemy Enemy;
        [HideInInspector]
        public bool IsAttacking = false;
        protected void Start()
        {
            Enemy = GetComponent<Enemy>();
            Enemy.Movement.OnFollow += TryAttack;
        }
        public virtual bool CanAttack(GameObject creature)
        {
            if (Enemy.Movement.State == EnemyState.Chase && Vector3.Distance(transform.position, creature.transform.position) <= AttackDistance)
                return true;
            return false;
        }
        public virtual void StopAttack()
        {
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            IsAttacking = false;
        }
        public virtual void TryAttack()
        {
            if (Enemy.Movement.PursuedTarget && CanAttack(Enemy.Movement.PursuedTarget))
            {
                if (attackCoroutine == null)
                {
                    IDamageable damageable = null;
                    damageable = Enemy.Movement.PursuedTarget.GetComponent<Damageable>();
                    if (damageable == null)
                        damageable = Enemy.Movement.PursuedTarget.GetComponentInParent<Damageable>();
                    attackCoroutine = StartCoroutine(Attack(damageable));
                }
                IsAttacking = true;
            }
        }
        protected virtual IEnumerator Attack(IDamageable objectToDamage)
        {
            while (objectToDamage != null)
            {
                OnAttack?.Invoke(objectToDamage);
                objectToDamage.TakeDamage(Damage);
                yield return new WaitForSeconds(AttackDelay);
            }
        }
    }
}