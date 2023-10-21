using EnemyNS.Attack;
using EnemyNS.Base;
using PlayerScriptsNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Type.Gramophone.Attack
{
    public class GramophoneSphereAttack : EnemyAttack
    {
        public SphereCollider SphereColliderAttack;
        [Tooltip("The closer to the gramophone, the stronger the influence.")]
        public float MaxDistanceMultiplier = 2f;
        public float SpeedMultiplier = 1f;
        private Dictionary<GameObject, Coroutine> currentDamageCoroutines;
        //float - default speed
        private Dictionary<GameObject, float> defaultSpeedMultipliers;
        protected new void Start()
        {
            base.Start();
            if (!SphereColliderAttack)
                SphereColliderAttack = GetComponent<SphereCollider>();
            SphereColliderAttack.radius = AttackRadius;
            currentDamageCoroutines = new Dictionary<GameObject, Coroutine>();
            defaultSpeedMultipliers = new Dictionary<GameObject, float>();
        }
        public override void TryAttack()
        {
            foreach (var opponent in Enemy.Movement.KnownCreatures.Keys)
            {
                if (Enemy.Movement.KnownCreatures[opponent].GetCreatureName() != Enemy.Movement.GetCreatureName())
                {
                    if (!currentDamageCoroutines.ContainsKey(opponent) && Vector3.Distance(gameObject.transform.position, opponent.transform.position) <= AttackRadius)
                    {
                        Damageable damageable = (Damageable)IDamageable.GetDamageableFromGameObject(opponent);
                        if (damageable)
                            currentDamageCoroutines.Add(opponent, StartCoroutine(Attack(damageable)));
                    }
                }
            }
        }
        public override void StopAttack()
        {
            base.StopAttack();
            foreach (var gameObject in defaultSpeedMultipliers.Keys)
                SetGameObjectSpeed(gameObject, defaultSpeedMultipliers[gameObject]);
            defaultSpeedMultipliers.Clear();
            currentDamageCoroutines.Clear();
            StopAllCoroutines();
        }
        protected override IEnumerator Attack(IDamageable objectToDamage)
        {
            NavMeshAgent navMeshAgent = objectToDamage.GetGameObject().GetComponent<NavMeshAgent>();
            float gameObjectDefaultSpeed = navMeshAgent ? navMeshAgent.speed : 1f;
            defaultSpeedMultipliers.Add(objectToDamage.GetGameObject(), gameObjectDefaultSpeed);
            Coroutine currentCoroutine = null;
            while (Vector3.Distance(gameObject.transform.position, objectToDamage.GetGameObject().transform.position) <= AttackRadius)
            {
                OnAttack?.Invoke(objectToDamage);
                float distanceCoeff = Mathf.Lerp(MaxDistanceMultiplier, 1, Vector3.Distance(gameObject.transform.position, objectToDamage.GetGameObject().transform.position) / AttackRadius);
                objectToDamage.TakeDamage(Damage * distanceCoeff);
                if (!objectToDamage.GetGameObject().GetComponent<PlayerMotor>())
                {
                    EnemyMovement enemyMovement = objectToDamage.GetGameObject().GetComponent<EnemyMovement>() != null ? objectToDamage.GetGameObject().GetComponent<EnemyMovement>()
                        : objectToDamage.GetGameObject().GetComponentInParent<EnemyMovement>() != null ? objectToDamage.GetGameObject().GetComponentInParent<EnemyMovement>()
                        : objectToDamage.GetGameObject().GetComponentInChildren<EnemyMovement>();
                    if (enemyMovement && currentCoroutine == null)
                        currentCoroutine = StartCoroutine(RunAwayFromGameObjectCoroutine(gameObject, enemyMovement, currentCoroutine));
                }
                SetGameObjectSpeed(objectToDamage.GetGameObject(), gameObjectDefaultSpeed * (SpeedMultiplier / distanceCoeff));
                yield return new WaitForSeconds(AttackDelay);
            }
            SetGameObjectSpeed(objectToDamage.GetGameObject(), gameObjectDefaultSpeed);
            defaultSpeedMultipliers.Remove(objectToDamage.GetGameObject());
            currentDamageCoroutines.Remove(objectToDamage.GetGameObject());
        }
        private IEnumerator RunAwayFromGameObjectCoroutine(GameObject fromGameObject, EnemyMovement enemyMovement, Coroutine currentCoroutine)
        {
            Vector3 directionToFlee = enemyMovement.gameObject.transform.position - fromGameObject.transform.position;
            directionToFlee.Normalize();
            Vector3 destination = enemyMovement.gameObject.transform.position + directionToFlee * (AttackRadius * 2);
            NavMesh.SamplePosition(destination, out NavMeshHit hit, AttackRadius * 2, enemyMovement.Agent.areaMask);
            Vector3 finalDestination = hit.position;
            enemyMovement.PriorityTasks.Add(new PriorityTask(finalDestination, 1));
            enemyMovement.State = EnemyState.DoPriority;
            while (enemyMovement.Agent.destination == finalDestination && enemyMovement.Agent.remainingDistance <= enemyMovement.Agent.stoppingDistance)
                yield return new WaitForSeconds(0.1f);
            currentCoroutine = null;
        }
        //Vector3 randomDirection = Random.insideUnitSphere * (AttackRadius * 2);
        //randomDirection += fromGameObject.transform.position;
        //    NavMeshHit hit;
        //NavMesh.SamplePosition(randomDirection, out hit, AttackRadius* 2, 1);
        //    Vector3 finalPosition = hit.position;
        private void SetGameObjectSpeed(GameObject gameObject, float speed)
        {
            if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
                navMeshAgent.speed = speed;
            else if (gameObject.TryGetComponent(out PlayerMotor playerMotor))
                playerMotor.SetSpeedCoef(speed);
            else if (gameObject.GetComponentInParent<NavMeshAgent>() != null)
                gameObject.GetComponentInParent<NavMeshAgent>().speed = speed;
        }
    }
}
