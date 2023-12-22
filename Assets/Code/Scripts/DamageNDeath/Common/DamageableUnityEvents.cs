using AYellowpaper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DamageableNS.OnActions
{
    public class DamageableUnityEvents : MonoBehaviour
    {
        public List<InterfaceReference<IDamageable, MonoBehaviour>> Damageables;
        public UnityEvent EventOnTakeDamage;
        public UnityEvent EventOnDead;
        protected virtual void Start()
        {
            foreach (var damageable in Damageables)
            {
                Damageable damageable1 = (Damageable)damageable.Value;
                damageable1.OnTakeDamageWithDamageData += OnTakeDamage;
                damageable1.OnDead += OnDead;
            }
        }
        protected virtual void OnTakeDamage(TakeDamageData takeDamageData) =>
            EventOnTakeDamage?.Invoke();
        protected virtual void OnDead() =>
            EventOnDead?.Invoke();
    }
}
