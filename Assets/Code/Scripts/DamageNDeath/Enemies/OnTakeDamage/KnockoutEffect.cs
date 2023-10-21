using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.OnTakeDamage
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnockoutEffect : MonoBehaviour
    {
        public bool KnockoutEnable = false;
        public float InKnockoutTime = 0.3f;
        public float KnockoutForce = 1f;
        private Enemy enemy;
        private bool isInKnockout = false;
        private Rigidbody mainRigidbody;
        private Vector3 agentBeforeDestination;
        void Start()
        {
            enemy = GetComponent<Enemy>();
            mainRigidbody = GetComponent<Rigidbody>();
            enemy.OnTakeDamageWithDamageData += OnTakeDamage;
        }
        private void OnTakeDamage(TakeDamageData takeDamageData)
        {
            if (KnockoutEnable && enemy.Health > 0 && takeDamageData.Hit != Vector3.zero)
            {
                Vector3 moveDirection = transform.position - takeDamageData.FromGameObject.transform.position;
                if (!isInKnockout)
                {
                    agentBeforeDestination = enemy.Movement.Agent.destination;
                    enemy.Movement.Agent.enabled = false;
                    mainRigidbody.isKinematic = false;
                    isInKnockout = true;
                    enemy.Movement.Agent.velocity = Vector3.zero;
                    if (mainRigidbody != null)
                        mainRigidbody.AddForce(moveDirection.normalized * KnockoutForce * takeDamageData.Force, ForceMode.Impulse);
                    StartCoroutine(KnockBackTimer(mainRigidbody));
                }
            }
        }
        private IEnumerator KnockBackTimer(Rigidbody hittedRigidbody)
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
        }
    }
}