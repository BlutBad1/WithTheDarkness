using AYellowpaper;
using DamageableNS;
using UnityEngine;

namespace EnemyNS.Base.StateBehaviourNS.Investigate
{
    public class InvestigateOnDamage : StateBehaviour
    {
        [SerializeField, RequireInterface(typeof(IDamageable))]
        private MonoBehaviour damageable;
        [SerializeField]
        private StateHandler stateHandler;

        protected IDamageable Damageable { get => (IDamageable)damageable; }
        private void OnEnable()
        {
            Damageable.OnTakeDamageWithDamageData += OnTakeDamageHandBehaviour;
        }
        private void OnDisable()
        {
            Damageable.OnTakeDamageWithDamageData -= OnTakeDamageHandBehaviour;
        }
        private void OnTakeDamageHandBehaviour(TakeDamageData takeDamageData)
        {
            if (takeDamageData.FromGameObject && stateHandler.State != EnemyState.Chase)
            {
                stateHandler.Destination = takeDamageData.FromGameObject.transform.position;
                stateHandler.State = EnemyState.Investigate;
            }
        }
    }
}