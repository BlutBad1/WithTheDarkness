using UnityEngine;

namespace DamageableNS
{
    public class TakeDamageData
    {
        public IDamageable DamagedObject;
        public float Damage;
        public float Force;
        public Vector3 HitPoint;
        public GameObject FromGameObject;
        public TakeDamageData(IDamageable damagedObject, float damage, float force, Vector3 hitPoint, GameObject fromGameObject = null)
        {
            DamagedObject = damagedObject;
            Damage = damage;
            Force = force;
            HitPoint = hitPoint;
            FromGameObject = fromGameObject;
        }
    }
    public interface IDamageable
    {
        public delegate void DamageWIthData(TakeDamageData takeDamageData);
        public delegate void DamageWithoutData();
        public event DamageWIthData OnTakeDamageWithDamageData;
        public event DamageWithoutData OnTakeDamageWithoutDamageData;
        public void TakeDamage(TakeDamageData takeDamageData);
        public void TakeDamage(float damage);
        public GameObject GetGameObject();
        public static IDamageable GetDamageableFromGameObject(GameObject gameObject)
        {
            IDamageable damageable = gameObject.GetComponent<IDamageable>() != null ? gameObject.GetComponent<IDamageable>()
            : gameObject.GetComponentInParent<IDamageable>() != null ? gameObject.GetComponentInParent<IDamageable>()
            : gameObject.GetComponentInChildren<IDamageable>();
            return damageable;
        }
    }
}
