using DamageableNS;
using DamageableNS.OnTakeDamage;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.OnTakeDamage
{
    public class EnemyKnockoutEffect : PushDamageable
    {
        [SerializeField]
        private bool knockoutEnable = false;
        [SerializeField]
        private float inKnockoutTime = 0.3f;

        private bool isInKnockout = false;
        private Vector3 agentBeforeDestination;
        private Coroutine knockoutCoroutine;

        public bool KnockoutEnable { get => knockoutEnable; set => knockoutEnable = value; }

        protected override void OnTakeDamage(TakeDamageData takeDamageData)
        {
            Enemy enemy = (Enemy)Damageable;
            if (KnockoutEnable && enemy.Health > 0 && takeDamageData.HitData != null)
            {
                if (!isInKnockout)
                {
                    agentBeforeDestination = enemy.Movement.Agent.destination;
                    enemy.Movement.Agent.enabled = false;
                    DamageableRigidbody.isKinematic = false;
                    isInKnockout = true;
                    enemy.Movement.Agent.velocity = Vector3.zero;
                    PushRigidbody(DamageableRigidbody, takeDamageData);
                    if (knockoutCoroutine == null)
                        knockoutCoroutine = StartCoroutine(InKnockout(DamageableRigidbody, enemy));
                }
            }
        }
        private IEnumerator InKnockout(Rigidbody hittedRigidbody, Enemy enemy)
        {
            yield return new WaitForSeconds(inKnockoutTime);
            if (enemy.Health > 0)
            {
                enemy.Movement.Agent.enabled = true;
                enemy.Movement.Agent.SetDestination(agentBeforeDestination);
                if (hittedRigidbody != null)
                    hittedRigidbody.isKinematic = true;
            }
            isInKnockout = false;
            knockoutCoroutine = null;
        }
    }
}