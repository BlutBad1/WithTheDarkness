using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public delegate void TakeDamageEvent(float damage, float force, Vector3 hit);
    public delegate void DeathEvent();
    public TakeDamageEvent OnTakeDamage;
    public DeathEvent OnDeath;
    public float Health = 100;
    public virtual Transform GetTransform()
    {
        return transform;
    }
    public virtual void TakeDamage(float damage, float force, Vector3 hit)
    {
        TakeDamage(damage);
        OnTakeDamage?.Invoke(damage, force, hit);
    }
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDeath?.Invoke();
            OnTakeDamage = null;
            OnDeath = null;
        }
    }
}
