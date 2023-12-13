using DamageableNS;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Type.Hand
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
    public class HandBehaviour : Enemy
    {
        protected sealed override void Start()
        {
            base.Start();
            OnTakeDamageWithDamageData += OnTakeDamageHandBehaviour;
            // OnTakeDamageWithoutDamageData += OnTakeDamageHandBehaviour;
        }
        Coroutine currentCheckCoroutine;
        private void OnTakeDamageHandBehaviour(TakeDamageData takeDamageData)
        {
            if (!IsDead && takeDamageData.FromGameObject && Movement.State == Movement.DefaultState)
            {
                if (currentCheckCoroutine == null)
                    StartCoroutine(CheckWhileAgentNotEnabled(takeDamageData));
            }
        }
        IEnumerator CheckWhileAgentNotEnabled(TakeDamageData takeDamageData)
        {
            while (!Movement.Agent.isActiveAndEnabled || Movement.Agent.isOnOffMeshLink)
                yield return new WaitForSeconds(0.1f);
            Movement.Agent.SetDestination(takeDamageData.FromGameObject.transform.position);
            currentCheckCoroutine = null;
        }
        //private void OnTakeDamageHandBehaviour(TakeDamageData takeDamageData)
        //{
        //    if (!IsDead && takeDamageData.FromGameObject && Movement.State == Movement.DefaultState)
        //    {
        //        if (takeDamageData.FromGameObject.GetComponentInParent<PlayerHealth>() || takeDamageData.FromGameObject.GetComponent<PlayerHealth>())
        //        {
        //            if (currentCheckCoroutine == null && runAwayCoroutine == null)
        //                StartCoroutine(CheckWhileAgentNotEnabled(takeDamageData));
        //        }
        //        else
        //        {
        //            if (Movement.State == Movement.DefaultState && Movement.Agent.enabled && Movement.Agent.isOnNavMesh)
        //            {
        //                if (currentCheckCoroutine != null)
        //                {
        //                    StopCoroutine(currentCheckCoroutine);
        //                    currentCheckCoroutine = null;
        //                }
        //                if (runAwayCoroutine == null)
        //                    runAwayCoroutine = StartCoroutine(RunAwayFromGameObjectCoroutine(takeDamageData.FromGameObject));
        //            }
        //        }
        //    }
        //}
        //private void OnTakeDamageHandBehaviour()
        //{
        //    if (Movement.State == Movement.DefaultState && Movement.Agent.enabled && Movement.Agent.isOnNavMesh)
        //    {
        //        if (currentCoroutine != null)
        //        {
        //            StopCoroutine(currentCoroutine);
        //            currentCoroutine = null;
        //        }
        //        if (runAwayCoroutine == null)
        //            runAwayCoroutine = StartCoroutine(RunAwayFromGameObjectCoroutine());
        //    }
        //}
        //private IEnumerator RunAwayFromGameObjectCoroutine(GameObject fromGameObject)
        //{
        //    Vector3 randomDirection = Random.insideUnitSphere * 50f;
        //    randomDirection += fromGameObject.transform.position;
        //    NavMeshHit hit;
        //    NavMesh.SamplePosition(randomDirection, out hit, 50f, 1);
        //    Vector3 finalPosition = hit.position;
        //    Movement.Agent.SetDestination(finalPosition);
        //    while (Movement.Agent.destination == finalPosition && Movement.Agent.remainingDistance <= Movement.Agent.stoppingDistance)
        //        yield return new WaitForSeconds(0.1f);
        //    runAwayCoroutine = null;
        //}
    }
}