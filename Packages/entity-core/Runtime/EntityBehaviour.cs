using AYellowpaper;
using DamageableNS;
using ScriptableObjectNS.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Base
{
    public abstract class EntityBehaviour : Damageable
    {
        [SerializeField, RequireInterface(typeof(IEntityMovement))]
        private MonoBehaviour movement;
        [SerializeField, RequireInterface(typeof(ISpotTarget))]
        private MonoBehaviour spotTarget;
        [SerializeField]
        private EntitySize entitySize;

        public EntitySize EntitySize { get => entitySize; set => entitySize = value; }
        protected ISpotTarget SpotTarget { get => (ISpotTarget)spotTarget; }
        protected IEntityMovement Movement { get => (IEntityMovement)movement; }

        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }
        public void HandleGainCreatureInSight(GameObject spottedTarget) =>
            SpotTarget.HandleGainCreatureInSight(spottedTarget);
        public void HandleLoseCreatureFromSight(GameObject lostTarget) =>
            SpotTarget.HandleLoseCreatureFromSight(lostTarget);
    }
}