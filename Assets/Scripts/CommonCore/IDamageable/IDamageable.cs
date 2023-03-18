using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public delegate void TakeDamageEvent(float force, Vector3 hit);
    public delegate void DeathEvent();
    public void TakeDamage(int damage, float force, Vector3 hit);
    public void TakeDamage(int damage);
    Transform GetTransform();
}
