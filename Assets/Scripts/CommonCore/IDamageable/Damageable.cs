using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public IDamageable.TakeDamageEvent OnTakeDamage;
    public IDamageable.DeathEvent OnDeath;
    public  int Health = 100;

    public virtual Transform GetTransform()
    {
        return transform;
    }

    public virtual void TakeDamage(int damage, float force, Vector3 hit)
    {
        TakeDamage(damage);
        OnTakeDamage?.Invoke(force, hit);
    }
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            OnDeath?.Invoke();
         
        }
    }


}
