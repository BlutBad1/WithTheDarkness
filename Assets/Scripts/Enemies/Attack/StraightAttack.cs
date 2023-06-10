using EnemyBaseNS;
using MyConstants;
using System.Collections;
using UnityEngine;

namespace EnemyAttackNS
{
    public class StraightAttack : EnemyAttack
    {
        public LayerMask WhatIsPlayer;
        [Tooltip("Default is player")]
        public GameObject ObjectToAttack;
        [HideInInspector]
        public SphereCollider AttackColider;
        public LayerMask WhatIsRayCastIgnore;
        protected void Start()
        {
            if (!ObjectToAttack)
            {
                ObjectToAttack = GameObject.Find(CommonConstants.PLAYER);
#if UNITY_EDITOR
                if (!ObjectToAttack)
                    Debug.LogWarning("Object to attack is not found");
#endif
            }
            objectDamageable = ObjectToAttack?.GetComponent<IDamageable>();
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
        public override bool CanAttack(GameObject enemy, GameObject player)
        {
            if (base.CanAttack(enemy, player))
            {
                enemy.TryGetComponent(out Enemy enemyScript);
                if (enemyScript != null && enemyScript.Health > 0)
                {
                    Vector3 origin = enemy.transform.position;
                    origin.y = (player.transform.position.y + origin.y)/2;
                    Ray ray = new Ray(origin, player.transform.position - origin);
                    // if (Physics.SphereCast(ray, 0.3f, (player.transform.position - enemy.transform.position).magnitude, ~WhatIsRayCastIgnore))
                    if (Physics.Raycast(ray, (player.transform.position - enemy.transform.position).magnitude, ~WhatIsRayCastIgnore))
                        return false;
                    return true;
                }
            }
            StopAttack();
            return false;
        }
        protected void Update()
        {
            if (ObjectToAttack != null && (transform.position - ObjectToAttack.transform.position).magnitude <= AttackDistance)
            {
                if (CanAttack(gameObject, ObjectToAttack))
                {
                    if (attackCoroutine == null)
                        attackCoroutine = StartCoroutine(Attack());
                    IsAttacking = true;
                }
            }
        }
        public override void StopAttack()
        {
            base.StopAttack();
            AttackColider.enabled = false;
        }
        public override void TryAttack()
        {
            base.TryAttack();
            AttackColider.enabled = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (((WhatIsPlayer | (1 << other.gameObject.layer)) == WhatIsPlayer) && other.GetComponent<ILastTouched>().iLastEntered == AttackColider)
            {
                if (ObjectToAttack == null)
                {
                    ObjectToAttack = other.gameObject;
                    objectDamageable = ObjectToAttack.GetComponent<IDamageable>();
                }
                if (IsAttacking)
                    objectDamageable.TakeDamage(Damage, AttackForce, (ObjectToAttack.transform.position - gameObject.transform.position).normalized);
            }
        }
        protected override IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);
            while (objectDamageable != null)
            {
                OnAttack?.Invoke(objectDamageable);
                yield return Wait;
            }
        }
    }
}