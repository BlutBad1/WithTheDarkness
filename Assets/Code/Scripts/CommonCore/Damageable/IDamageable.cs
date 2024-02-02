using UnityEngine;

namespace DamageableNS
{
    public class HitData
    {
        public RaycastHit HitPoint;
        public HitData(RaycastHit hitPoint)
        {
            HitPoint = hitPoint;
        }
        public HitData(Vector3 hitPosition)
        {
            HitPoint = new RaycastHit();
            HitPoint.point = hitPosition;
        }
    }
    public class TakeDamageData
    {
        public IDamageable DamagedObject;
        public float Damage;
        public float Force;
        public HitData HitData;
        public GameObject FromGameObject;
        public TakeDamageData(IDamageable damagedObject, float damage, float force, HitData hitPoint, GameObject fromGameObject = null)
        {
            DamagedObject = damagedObject;
            Damage = damage;
            Force = force;
            HitData = hitPoint;
            FromGameObject = fromGameObject;
        }
    }
    public interface IDamageable
    {
        public delegate void DamageWIthData(TakeDamageData takeDamageData);
        public delegate void DamageWithoutData();
        public event DamageWIthData OnTakeDamageWithDamageData;
        public event DamageWithoutData OnTakeDamageWithoutDamageData;
        public event DamageWithoutData OnDead;
        public void TakeDamage(TakeDamageData takeDamageData);
        public void TakeDamage(float damage);
        public GameObject GetGameObject();
    }
}
