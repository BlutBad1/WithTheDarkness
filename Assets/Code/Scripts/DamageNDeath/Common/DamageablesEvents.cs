using AYellowpaper;
using System.Collections.Generic;
using UnityEngine;

namespace DamageableNS.OnActions
{
    public class DamageablesEvents : MonoBehaviour
    {
        public List<InterfaceReference<IDamageable, MonoBehaviour>> Damageables;
        public delegate void IDamageableEventWithData(TakeDamageData takeDamageData);
        public delegate void IDamageableEventWithoutData();
        public event IDamageableEventWithData OnTakeDamageWithDataEvent;
        public event IDamageableEventWithoutData OnTakeDamageWithoutDataEvent;
        public event IDamageableEventWithoutData OnDeadEvent;
        protected virtual void Start()
        {
            foreach (var damageable in Damageables)
            {
                damageable.Value.OnTakeDamageWithDamageData += OnTakeDamageWithData;
                damageable.Value.OnTakeDamageWithoutDamageData += OnTakeDamageWithoutData;
                damageable.Value.OnDead += OnDead;
            }
        }
        private void OnTakeDamageWithData(TakeDamageData takeDamageData) =>
           OnTakeDamageWithDataEvent?.Invoke(takeDamageData);
        private void OnTakeDamageWithoutData() =>
           OnTakeDamageWithoutDataEvent?.Invoke();
        private void OnDead() =>
          OnDeadEvent?.Invoke();
    }
}