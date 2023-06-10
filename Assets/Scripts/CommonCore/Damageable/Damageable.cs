using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public delegate void TakeDamageEvent(int damage, float force, Vector3 hit);
    public delegate void DeathEvent();
    public TakeDamageEvent OnTakeDamage;
    public DeathEvent OnDeath;
    public int Health = 100;
    private bool HasEventStarted = false;
    public virtual Transform GetTransform()
    {
        return transform;
    }
    public virtual void TakeDamage(int damage, float force, Vector3 hit)
    {
        TakeDamage(damage);
        OnTakeDamage?.Invoke(damage, force, hit);
    }
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            if (!HasEventStarted)
            {
                OnDeath?.Invoke();
                HasEventStarted = true;
            }
        }
    }
}
