using CreatureNS;
using DamageableNS;
using EnemyNS.Attack;
using EnemyNS.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyNS.Type.Gramophone.Attack
{
    public class GramophoneSphereAttack : EnemyStateAttack
    {
        [SerializeField, FormerlySerializedAs("SphereColliderAttack")]
        private SphereCollider sphereColliderAttack;
        [SerializeField]
        private EnemySpotTarget enemySpotTarget;
        [SerializeField]
        private EnemyCreature enemyCreature;
        [SerializeField, FormerlySerializedAs("MaxDistanceMultiplier"), Tooltip("The closer to the gramophone, the stronger the influence.")]
        private float maxDistanceMultiplier = 2f;
        [SerializeField, FormerlySerializedAs("SpeedMultiplier")]
        private float speedMultiplier = 1f;

        private bool isEnable = false;
        private Dictionary<GameObject, Coroutine> currentDamageCoroutines = new Dictionary<GameObject, Coroutine>();
        //float - default speed
        private Dictionary<GameObject, float> defaultSpeedMultipliers = new Dictionary<GameObject, float>();

        public bool IsEnable { get => isEnable; set => isEnable = value; } //is using in an event

        private void Start()
        {
            sphereColliderAttack.radius = AttackRadius;
        }
        private void Update()
        {
            if (IsEnable)
                TryAttack();
        }
        public override void TryAttack()
        {
            foreach (GameObject opponent in enemySpotTarget.KnownCreatures.Keys)
            {
                if (CheckTryAttackConditions(opponent))
                {
                    Damageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<Damageable>(opponent);
                    if (damageable)
                        currentDamageCoroutines[opponent] = StartCoroutine(Attack(damageable));
                }
            }
        }
        public override void StopAttack()
        {
            base.StopAttack();
            foreach (var gameObject in defaultSpeedMultipliers.Keys)
                SetObjectDefaultSpeed(gameObject);
            defaultSpeedMultipliers.Clear();
            currentDamageCoroutines.Clear();
            StopAllCoroutines();
        }
        private bool CheckTryAttackConditions(GameObject opponent)
        {
            bool result = (enemySpotTarget.KnownCreatures[opponent].GetCreatureName() != enemyCreature.GetCreatureName());
            if (result)
                result = (!currentDamageCoroutines.ContainsKey(opponent) || currentDamageCoroutines[opponent] == null) && Vector3.Distance(transform.position, opponent.transform.position) <= AttackRadius;
            return result;
        }
        private IEnumerator Attack(IDamageable objectToDamage)
        {
            GameObject damageObject = objectToDamage.GetGameObject();
            Transform damageObjectTran = damageObject.transform;
            NavMeshAgent navMeshAgent = damageObject.GetComponent<NavMeshAgent>();
            EnemyStateHandler enemyStateHandler = UtilitiesNS.Utilities.GetComponentFromGameObject<EnemyStateHandler>(damageObject);
            Coroutine runAwayCoroutine = null;
            float gameObjectDefaultSpeed = 1f;
            defaultSpeedMultipliers[damageObject] = gameObjectDefaultSpeed;
            ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(damageObject);
            while (Vector3.Distance(gameObject.transform.position, damageObjectTran.position) <= AttackRadius)
            {
                float distanceCoeff = CalculateDistanceCoeff(damageObjectTran);
                objectToDamage.TakeDamage(Damage * distanceCoeff);
                float currentCreauteSpeed = gameObjectDefaultSpeed * (speedMultiplier / distanceCoeff);
                SetOtherEnemyBehaviour(enemyStateHandler, navMeshAgent, runAwayCoroutine);
                SetGameObjectSpeed(creature, currentCreauteSpeed);
                yield return new WaitForSeconds(AttackDelay);
            }
            SetGameObjectSpeed(creature, gameObjectDefaultSpeed);
            currentDamageCoroutines[damageObject] = null;
        }
        private void SetObjectDefaultSpeed(GameObject gameObject)
        {
            ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(gameObject);
            SetGameObjectSpeed(creature, defaultSpeedMultipliers[gameObject]);
        }
        private void SetOtherEnemyBehaviour(EnemyStateHandler enemyStateHandler, NavMeshAgent navMeshAgent, Coroutine currentCoroutine)
        {
            if (enemyStateHandler && navMeshAgent && currentCoroutine == null)
                currentCoroutine = StartCoroutine(RunAwayFromGameObjectCoroutine(enemyStateHandler, navMeshAgent, currentCoroutine));
        }
        private float CalculateDistanceCoeff(Transform attackObjectTran) =>
            Mathf.Lerp(maxDistanceMultiplier, 1, Vector3.Distance(gameObject.transform.position, attackObjectTran.position) / AttackRadius);
        private void SetGameObjectSpeed(ICreature creature, float speed) =>
            creature.SetSpeedCoef(speed);
        private IEnumerator RunAwayFromGameObjectCoroutine(EnemyStateHandler enemyStateHandler, NavMeshAgent navMeshAgent, Coroutine currentCoroutine)
        {
            Vector3 destination = CalculateRunAwayPosition(enemyStateHandler);
            NavMesh.SamplePosition(destination, out NavMeshHit hit, AttackRadius * 2, navMeshAgent.areaMask);
            Vector3 finalDestination = hit.position;
            SetEnemyPriority(enemyStateHandler, finalDestination);
            while (!UtilitiesNS.Utilities.CheckIfAgentHasArrived(navMeshAgent))
                yield return new WaitForSeconds(0.1f);
            currentCoroutine = null;
        }
        private Vector3 CalculateRunAwayPosition(EnemyStateHandler enemyStateHandler)
        {
            Vector3 enemyPosition = enemyStateHandler.gameObject.transform.position;
            Vector3 directionToFlee = enemyPosition - gameObject.transform.position;
            directionToFlee.Normalize();
            Vector3 destination = enemyPosition + directionToFlee * (AttackRadius * 2);
            return destination;
        }
        private void SetEnemyPriority(EnemyStateHandler enemyStateHandler, Vector3 finalDestination)
        {
            enemyStateHandler.PriorityTasks.Add(new PriorityTask(finalDestination, 1));
            enemyStateHandler.State = EnemyState.DoPriority;
        }
    }
}
