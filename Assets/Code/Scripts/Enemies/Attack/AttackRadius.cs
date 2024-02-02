using DamageableNS;
using EnemyNS.Base;
using UnityEngine;

namespace EnemyNS.Attack
{
    [RequireComponent(typeof(SphereCollider))]
    public class AttackRadius : EnemyStateAttack
    {
        private SphereCollider sphereCollider;
        private StateInfo stateInfo;

        protected void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = AttackRadius;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == stateInfo.PursuedTarget)
            {
                if (isAttacking && stateInfo.PursuedTarget.TryGetComponent(out IDamageable damageable))
                {
                    HitData hitData = new HitData((stateInfo.PursuedTarget.transform.position - gameObject.transform.position).normalized);
                    TakeDamageData takeDamageData = new TakeDamageData(damageable, Damage, AttackForce,
                      hitData, gameObject);
                    damageable.TakeDamage(takeDamageData);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == stateInfo.PursuedTarget)
                StopAttack();
        }
        public override void TryAttack()
        {
            base.TryAttack();
            StateInfo stateInfo = enemyStateHandler.GenerateStateInfo();
            this.stateInfo = stateInfo;
        }
        protected override bool CanAttack() =>
            Vector3.Distance(transform.position, stateInfo.PursuedTarget.transform.position) > AttackDistance;
    }
}