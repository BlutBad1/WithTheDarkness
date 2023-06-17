using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableNS.Common
{


public class ItemDamagable : MonoBehaviour,IDamageable
{
    public delegate void TakeDamageEvent(float force, Vector3 hit);
    public TakeDamageEvent OnTakeDamage;
    public Transform GetTransform()
    {
        return transform;
    }

    public void TakeDamage(float damage, float force, Vector3 hit)
    {
            
            OnTakeDamage?.Invoke(force, hit);
    }

    public void TakeDamage(float damage)
    {
      
    }
}
}