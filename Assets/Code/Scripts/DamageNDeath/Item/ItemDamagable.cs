using UnityEngine;

namespace InteractableNS.Common
{
    public class ItemDamagable : MonoBehaviour, IDamageable
    {
        public delegate void TakeDamageEvent(float force, Vector3 hit);
        public TakeDamageEvent OnTakeDamage;
        public GameObject GetGameObject() =>
             gameObject;
        public void TakeDamage(TakeDamageData takeDamageData) =>
            OnTakeDamage?.Invoke(takeDamageData.Force, takeDamageData.Hit);
        public void TakeDamage(float damage)
        {
        }
    }
}