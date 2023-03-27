using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
  
    public void TakeDamage(int damage, float force, Vector3 hit);
    public void TakeDamage(int damage);
    public Transform GetTransform();
}
