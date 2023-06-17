using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
  
    public void TakeDamage(float damage, float force, Vector3 hit);
    public void TakeDamage(float damage);
    public Transform GetTransform();
}
