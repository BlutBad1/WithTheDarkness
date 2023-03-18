using EnemyBaseNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyAttackNS
{
    public class EnemyAttack : MonoBehaviour
    {


        public int Damage = 10;
        public float AttackDelay = 0.5f;
        public float AttackDistance = 2f;
        public float AttackRadius = 2f;
        public delegate void AttackEvent(IDamageable Target);
        public AttackEvent OnAttack;
        protected IDamageable playerDamageable;
        protected Coroutine attackCoroutine;
        [HideInInspector]
        public bool IsAttacking = false;
        public virtual bool CanAttack(GameObject enemy, GameObject player)
        {


            if (enemy.GetComponent<EnemyMovement>().State == EnemyState.Chase)
            {
                return true;
            }

            return false;



        }
        protected virtual IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

            while (playerDamageable != null)
            {

                OnAttack?.Invoke(playerDamageable);
                playerDamageable.TakeDamage(Damage);
                yield return Wait;
            }

        }

    }
}