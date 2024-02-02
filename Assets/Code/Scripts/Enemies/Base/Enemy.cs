using AYellowpaper;
using DamageableNS;
using ScriptableObjectNS.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Base
{
    public class Enemy : Damageable
    {
        [SerializeField, FormerlySerializedAs("Movement")]
        protected EnemyMovement movement;
        [SerializeField, RequireInterface(typeof(ISpotTarget))]
        private MonoBehaviour spotTarget;
        [SerializeField, FormerlySerializedAs("EnemySize")]
        private EnemySize enemySize;

        public ISpotTarget SpotTarget { get => (ISpotTarget)spotTarget; }
        public EnemySize EnemySize { get => enemySize; set => enemySize = value; }

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