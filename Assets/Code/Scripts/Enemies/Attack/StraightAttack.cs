using DamageableNS;
using EnemyNS.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Attack
{
    public class StraightAttack : EnemyStateAttack
    {
        [SerializeField]
        private SphereCollider attackColider;
        [SerializeField, FormerlySerializedAs("WhatIsRayCastIgnore")]
        private LayerMask whatIsRayCastIgnore;

        private float timeBetweenAttacks = 0;

        protected void Start()
        {
            attackColider.radius = AttackRadius;
            attackColider.enabled = false;
        }
        private void Update()
        {
            timeBetweenAttacks += Time.deltaTime;
        }
        private void OnTriggerEnter(Collider other)
        {
            Attack(other);
        }
        public override void StopAttack()
        {
            base.StopAttack();
            attackColider.enabled = false;
        }
        public override void TryAttack()
        {
            StateInfo stateInfo = enemyStateHandler.GenerateStateInfo();
            if (CanAttack() || stateInfo.State == EnemyState.UsingAbility)
                base.TryAttack();
        }
        public void AttackAnim() =>
                attackColider.enabled = true;
        protected override bool CanAttack()
        {
            if (base.CanAttack())
            {
                StateInfo stateInfo = enemyStateHandler.GenerateStateInfo();
                Transform targetTransform = stateInfo.PursuedTarget.transform;
                Vector3 origin = transform.position;
                origin.y = (targetTransform.position.y + origin.y) / 2;
                Ray ray = new Ray(origin, targetTransform.position - origin);
                // if (Physics.SphereCast(ray, 0.3f, (player.transform.position - enemy.transform.position).magnitude, ~WhatIsRayCastIgnore))
                if (Physics.Raycast(ray, out RaycastHit hit, (targetTransform.position - transform.position).magnitude, ~whatIsRayCastIgnore))
                {
                    if (stateInfo.PursuedTarget.layer != hit.collider.gameObject.layer)
                        return false;
                }
                return true;
            }
            return false;
        }
        private void Attack(Collider other)
        {
            if (CheckColliderConditions(other))
            {
                if (timeBetweenAttacks > AttackDelay)
                {
                    StateInfo stateInfo = enemyStateHandler.GenerateStateInfo();
                    HitData hitData = new HitData((stateInfo.PursuedTarget.transform.position - gameObject.transform.position).normalized);
                    IDamageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<IDamageable>(other.gameObject);
                    TakeDamageData takeDamageData = new TakeDamageData(damageable, Damage, AttackForce, hitData, gameObject);
                    if (isAttacking && damageable != null)
                        damageable.TakeDamage(takeDamageData);
                    timeBetweenAttacks = 0;
                }
            }
        }
        private bool CheckColliderConditions(Collider other)
        {
            StateInfo stateInfo = enemyStateHandler.GenerateStateInfo();
            return other.gameObject == stateInfo.PursuedTarget;
        }
    }
}