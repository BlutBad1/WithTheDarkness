using DamageableNS;
using UnityEngine;

namespace EnemyNS.Attack
{
    [RequireComponent(typeof(SphereCollider))]
    public class AttackRadius : EnemyAttack
    {
        [HideInInspector]
        public SphereCollider Collider;
        protected void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            Collider.radius = AttackRadius;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Enemy.Movement.PursuedTarget)
            {
                if (IsAttacking && Enemy.Movement.PursuedTarget.TryGetComponent(out IDamageable damageable))
                {
                    TakeDamageData takeDamageData = new TakeDamageData(damageable, Damage, AttackForce,
                        (Enemy.Movement.PursuedTarget.transform.position - gameObject.transform.position).normalized, gameObject);
                    damageable.TakeDamage(takeDamageData);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Enemy.Movement.gameObject)
                StopAttack();
        }
    }
}