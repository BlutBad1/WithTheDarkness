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
}
