using AYellowpaper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DamageableNS.OnActions
{
    public class DamageableUnityEvents : MonoBehaviour
    {
        public List<InterfaceReference<Damageable, MonoBehaviour>> Damageables;
        public UnityEvent EventOnTakeDamage;
        public UnityEvent EventOnDead;
        protected virtual void Start()
        {
            foreach (var damageable in Damageables)
            {
                damageable.Value.OnTakeDamageWithDamageData += OnTakeDamage;
                damageable.Value.OnDead += OnDead;
            }
        }
        protected virtual void OnTakeDamage(TakeDamageData takeDamageData) =>
            EventOnTakeDamage?.Invoke();
        protected virtual void OnDead() =>
            EventOnDead?.Invoke();
    }
}
