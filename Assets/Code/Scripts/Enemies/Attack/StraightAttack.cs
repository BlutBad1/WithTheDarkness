using DamageableNS;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.Attack
{
    public class StraightAttack : EnemyAttack
    {
        [HideInInspector]
        public SphereCollider AttackColider;
        public LayerMask WhatIsRayCastIgnore;
        private float timeBetweenAttacks = 0;
        protected new void Start()
        {
            base.Start();
            if (!TryGetComponent(out AttackColider))
            {
                AttackColider = GetComponentInChildren(typeof(SphereCollider)) as SphereCollider;
#if UNITY_EDITOR
                if (AttackColider == null)
                    Debug.LogError("SphereCollider is not found!");
#endif
            }
            AttackColider.radius = AttackRadius;
            AttackColider.enabled = false;
        }
        private void Update()
        {
            timeBetweenAttacks += Time.deltaTime;
        }
        public override void StopAttack()
        {
            base.StopAttack();
            AttackColider.enabled = false;
        }
        public void TryAttackAnim()
        {
            IsAttacking = true;
            AttackColider.enabled = true;
        }
        public override void TryAttack()
        {
            if (Enemy.Movement.State != EnemyState.UsingAbility)
                base.TryAttack();
            else
                TryAttackAnim();
        }
        public override bool CanAttack(GameObject creture)
        {
            if (base.CanAttack(creture))
            {
                if (Enemy.Health > 0)
                {
                    Vector3 origin = transform.position;
                    origin.y = (creture.transform.position.y + origin.y) / 2;
                    Ray ray = new Ray(origin, creture.transform.position - origin);
                    // if (Physics.SphereCast(ray, 0.3f, (player.transform.position - enemy.transform.position).magnitude, ~WhatIsRayCastIgnore))
                    if (Physics.Raycast(ray, out RaycastHit hit, (creture.transform.position - transform.position).magnitude, ~WhatIsRayCastIgnore))
                    {
                        if (creture.layer != hit.collider.gameObject.layer)
                            return false;
                    }
                    return true;
                }
            }
            StopAttack();
            return false;
        }
        protected override IEnumerator Attack(IDamageable objectToDamage)
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);
            while (objectToDamage != null)
            {
                OnAttack?.Invoke(objectToDamage);
                yield return Wait;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            ILastTouched ilt = other.GetComponent<ILastTouched>();
            if (other.gameObject == Enemy.Movement.PursuedTarget && (!ilt || ilt && ilt.iLastEntered == AttackColider))
            {
                if (timeBetweenAttacks > AttackDelay)
                {
                    IDamageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<IDamageable>(other.gameObject);
                    TakeDamageData takeDamageData = new TakeDamageData(damageable, Damage, AttackForce,
                     (Enemy.Movement.PursuedTarget.transform.position - gameObject.transform.position).normalized, gameObject);
                    if (IsAttacking && damageable != null)
                        damageable.TakeDamage(takeDamageData);
                    timeBetweenAttacks = 0;
                }
            }
        }
    }
}