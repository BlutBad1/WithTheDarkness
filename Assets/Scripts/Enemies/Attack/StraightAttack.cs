using EnemyBaseNS;
using MyConstants;
using System.Collections;
using UnityEngine;

namespace EnemyAttackNS
{
    public class StraightAttack : EnemyAttack
    {

        public LayerMask WhatIsPlayer;

        public GameObject ObjectUnderAttack;
        [HideInInspector]
        public SphereCollider AttackColider;

        protected void Start()
        {
            if (!ObjectUnderAttack)
            {
                ObjectUnderAttack = GameObject.Find(CommonConstants.PLAYER);
                if (!ObjectUnderAttack)
                {
                    Debug.LogWarning("ObjectUnderAttack is not found");
                }
            }
            
            objectDamageable = ObjectUnderAttack?.GetComponent<IDamageable>();

            if (!TryGetComponent(out AttackColider))
            {
                AttackColider = GetComponentInChildren(typeof(SphereCollider)) as SphereCollider;
                if (AttackColider == null)
                    Debug.LogError("SphereCollider is not found!");


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
                    origin.y = player.transform.position.y;

                    Ray ray = new Ray(origin, player.transform.position - origin);
                    if (Physics.SphereCast(ray, 0.3f, (player.transform.position - enemy.transform.position).magnitude, ~(1 << 11 | 1 << 12 | 1 << 20 | 1 << 8 | 1 << 7)))
                    {
                        return false;
                    }

                    return true;
                }

            }
            StopAttack();
            return false;


        }

        protected void Update()
        {


            if (ObjectUnderAttack != null && (transform.position - ObjectUnderAttack.transform.position).magnitude <= AttackDistance)
            {

                if (CanAttack(gameObject, ObjectUnderAttack))
                {
                    if (attackCoroutine == null)
                        attackCoroutine = StartCoroutine(Attack());
                    IsAttacking = true;
                }



            }

        }
        public void StopAttack()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = null;
            AttackColider.enabled = false;
            IsAttacking = false;


        }


        public void TryAttack()
        {
            AttackColider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {

            if ((1 << other.gameObject.layer) == WhatIsPlayer)
            {

                if (ObjectUnderAttack == null)
                {
                    ObjectUnderAttack = other.gameObject;
                    objectDamageable = ObjectUnderAttack.GetComponent<IDamageable>();
                }
                if (IsAttacking)
                {


                    objectDamageable.TakeDamage(Damage);
                }


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