using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
  
    public void TakeDamage(GunData weapon, RaycastHit hit);
    public void TakeDamage(int damage);
    Transform GetTransform();
}
