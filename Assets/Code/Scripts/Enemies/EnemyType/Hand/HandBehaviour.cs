using DamageableNS;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.Type.Hand
{
    public class HandBehaviour : Enemy
    {
        Coroutine currentCheckCoroutine;
        protected sealed override void Awake()
        {
            base.Awake();
            OnTakeDamageWithDamageData += OnTakeDamageHandBehaviour;
            // OnTakeDamageWithoutDamageData += OnTakeDamageHandBehaviour;
        }
        private void OnTakeDamageHandBehaviour(TakeDamageData takeDamageData)
        {
            if (!IsDead && takeDamageData.FromGameObject && Movement.State == Movement.DefaultState)
            {
                if (currentCheckCoroutine == null)
                    StartCoroutine(CheckWhileAgentNotEnabled(takeDamageData));
            }
        }
        private IEnumerator CheckWhileAgentNotEnabled(TakeDamageData takeDamageData)
        {
            while (!Movement.Agent.isActiveAndEnabled || Movement.Agent.isOnOffMeshLink)
                yield return new WaitForSeconds(0.1f);
            Movement.Agent.SetDestination(takeDamageData.FromGameObject.transform.position);
            currentCheckCoroutine = null;
        }
    }
}