using UnityEngine.Events;

namespace DamageableNS.OnActions
{
    public class DamageablesUnityEvents : DamageablesEvents
    {
        public UnityEvent EventOnTakeDamage;
        public UnityEvent EventOnDead;
        protected override void Start()
        {
            base.Start();
            OnTakeDamageWithDataEvent += OnTakeDamage;
            OnDeadEvent += OnDead;
        }
        protected void OnTakeDamage(TakeDamageData takeDamageData) =>
            EventOnTakeDamage?.Invoke();
        protected void OnDead() =>
            EventOnDead?.Invoke();
    }
}
