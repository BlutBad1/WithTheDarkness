using DamageableNS;
using UnityEngine;

namespace InteractableNS.Common
{
    public class ItemDamagable : MonoBehaviour, IDamageable
    {
        public event IDamageable.DamageWIthData OnTakeDamageWithDamageData;
        public event IDamageable.DamageWithoutData OnTakeDamageWithoutDamageData;
        public GameObject GetGameObject() =>
             gameObject;
        public void TakeDamage(TakeDamageData takeDamageData) =>
            OnTakeDamageWithDamageData?.Invoke(takeDamageData);
        public void TakeDamage(float damage)
        {
        }
    }
}