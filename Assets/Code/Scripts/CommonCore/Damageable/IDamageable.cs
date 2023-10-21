using UnityEngine;

public class TakeDamageData
{
    public float Damage;
    public float Force;
    public Vector3 Hit;
    public GameObject FromGameObject;
    public TakeDamageData(float damage, float force, Vector3 hit, GameObject fromGameObject = null)
    {
        Damage = damage;
        Force = force;
        Hit = hit;
        FromGameObject = fromGameObject;
    }
}
public interface IDamageable
{
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
