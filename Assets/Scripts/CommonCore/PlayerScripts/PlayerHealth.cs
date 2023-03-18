using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Damageable
{
   

    public override void TakeDamage(int damage)
    {
        GetComponent<CameraShake>().FooCameraShake(damage/20);
      
        Health -= damage;
        
        if (Health <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log($"Damage {damage}. You're Dead!");
        }
        else
        {
            Debug.Log($"Damage {damage}");
        }
    }

   
}
