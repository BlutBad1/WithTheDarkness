using DamageableNS;
using DamageableNS.OnTakeDamage;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.OnTakeDamage
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyKnockoutEffect : PushDamageable
    {
        public bool KnockoutEnable = false;
        public float InKnockoutTime = 0.3f;
        private bool isInKnockout = false;
        private Vector3 agentBeforeDestination;
        private Coroutine knockoutCoroutine;
        protected override void OnTakeDamage(TakeDamageData takeDamageData)
        {
            Enemy enemy = (Enemy)Damageable;
            if (KnockoutEnable && enemy.Health > 0 && takeDamageData.HitPoint != Vector3.zero)
            {
                if (!isInKnockout)
                {
                    agentBeforeDestination = enemy.Movement.Agent.destination;
                    enemy.Movement.Agent.enabled = false;
                    Rigidbody.isKinematic = false;
                    isInKnockout = true;
                    enemy.Movement.Agent.velocity = Vector3.zero;
                    PushRigidbody(Rigidbody, takeDamageData);
                    if (knockoutCoroutine == null)
                        knockoutCoroutine = StartCoroutine(InKnockout(Rigidbody, enemy));
                }
            }
        }
        private IEnumerator InKnockout(Rigidbody hittedRigidbody, Enemy enemy)
        {
            yield return new WaitForSeconds(InKnockoutTime);
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