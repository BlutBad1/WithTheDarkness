using AYellowpaper;
using DamageableNS;
using UnityEngine;

namespace EntityNS.Base.StateBehaviourNS.Investigate
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
            if (takeDamageData.FromGameObject && stateHandler.CurrentState != EntityState.Chase)
            {
                stateHandler.Destination = takeDamageData.FromGameObject.transform.position;
                stateHandler.CurrentState = EntityState.Investigate;
            }
        }
    }
}