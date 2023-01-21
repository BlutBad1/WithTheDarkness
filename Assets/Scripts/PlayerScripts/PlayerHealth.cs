using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool doesHaveKnockout { get ; set; }
    public int Health=100;
    public Transform GetTransform()
    {
        return transform;
    }

    public void TakeDamage(int damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        GetComponent<CameraShake>().FooCameraShake();
      
        Health -= damage;
        Debug.Log($"Damage {damage}");
        if (Health<=0)
        {
            Debug.Log("You're Dead");
        }
    }
}
