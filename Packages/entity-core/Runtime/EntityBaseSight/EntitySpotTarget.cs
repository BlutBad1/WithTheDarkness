using CreatureNS;
using DamageableNS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static DamageableNS.IDamageable;

namespace EntityNS.Base
{
    public class EntitySpotTarget : SpotTarget
    {
        [SerializeField]
        protected EntityCreature entityCreature;
        [SerializeField]
        protected float dontLoseSightIfDistanceLess = 1f;

        protected GameObject pursuedTarget;
        private Dictionary<GameObject, ICreature> knownCreatures = new Dictionary<GameObject, ICreature>();
        protected Dictionary<GameObject, ICreature> creaturesInSight = new Dictionary<GameObject, ICreature>();
        protected Dictionary<GameObject, Damageable> availableOpponents = new Dictionary<GameObject, Damageable>();

        public Dictionary<GameObject, ICreature> KnownCreatures { get => knownCreatures; }

        public override void HandleGainCreatureInSight(GameObject spottedTarget)
        {
            ICreature creatureComponent = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(spottedTarget);
            if (CheckSpotTargetCondtions(spottedTarget, creatureComponent))
            {
                CreatureSpottedFirstTime(spottedTarget, creatureComponent);
                TryAddCreatureToOpponentList(spottedTarget, creatureComponent);
            }
        }
        public override void HandleLoseCreatureFromSight(GameObject lostTarget)
        {
            if (Vector3.Distance(transform.position, lostTarget.transform.position) > dontLoseSightIfDistanceLess)
                LoseCreatureFromSight(lostTarget);
        }
        protected void TryGetPursuedTarget()
        {
            GameObject newTarget = GetNewTarget();
            if (newTarget != pursuedTarget && newTarget)
            {
                pursuedTarget = newTarget;
                OnTargetSpot?.Invoke(pursuedTarget);
            }
            else if (!newTarget)
                pursuedTarget = null;
        }
        protected void LoseCreatureFromSight(GameObject lostTarget)
        {
            if (lostTarget == pursuedTarget)
            {
                availableOpponents.Remove(pursuedTarget);
                OnTargetLost?.Invoke(pursuedTarget);
                TryGetPursuedTarget();
            }
            creaturesInSight.Remove(lostTarget);
        }
        protected GameObject GetNewTarget()
        {
            if (availableOpponents != null && availableOpponents.Count > 0)
            {
                RemoveDeadOpponents();
                return GetClosestKnownCreature(availableOpponents.Keys.ToArray());
            }
            else
                return null;
        }
        private void TryAddCreatureToOpponentList(GameObject spottedTarget, ICreature creatureComponent)
        {
            Damageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<Damageable>(spottedTarget);
            if (DefineOpponentCondtions(damageable, creatureComponent))
            {
                availableOpponents[spottedTarget] = damageable;
                SearchNewTargetOnCurrentTargetDead(damageable);
                if (pursuedTarget != spottedTarget)
                    TryGetPursuedTarget();
            }
        }
        private void RemoveDeadOpponents()
        {
            foreach (var entry in availableOpponents.Where(entry => entry.Value.IsDead).ToList())
                availableOpponents.Remove(entry.Key);
        }
        private GameObject GetClosestKnownCreature(IEnumerable objects)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject potentialTarget in objects)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            return bestTarget;
        }
        private void SearchNewTargetOnCurrentTargetDead(Damageable damageable)
        {
            DamageWithoutData action = null;
            action = () =>
            {
                if (availableOpponents.ContainsKey(pursuedTarget) && damageable == availableOpponents[pursuedTarget])
                    LoseCreatureFromSight(pursuedTarget);
            };
            damageable.OnDead += action;
        }
        private bool CheckSpotTargetCondtions(GameObject target, ICreature creatureComponent) =>
                target != gameObject && creatureComponent != null && !creaturesInSight.ContainsValue(creatureComponent);
        private void CreatureSpottedFirstTime(GameObject spottedTarget, ICreature creatureComponent)
        {
            if (!creaturesInSight.ContainsKey(spottedTarget))
                creaturesInSight.Add(spottedTarget, creatureComponent);
            if (!knownCreatures.ContainsKey(spottedTarget))
                knownCreatures.Add(spottedTarget, creatureComponent);
        }
        private bool DefineOpponentCondtions(Damageable damageable, ICreature creatureComponent) =>
                damageable != null && !damageable.IsDead && entityCreature.DefineRelation(creatureComponent) == CreatureRelation.Enemy;
    }
}