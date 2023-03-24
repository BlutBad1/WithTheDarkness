using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Damageable
{
   

    public override void TakeDamage(int damage)
    {
        if(TryGetComponent(out CameraShake cameraShake))
        {
            cameraShake.FooCameraShake(damage / 20);
        }
        Health -= damage;
        
        if (Health <= 0)
        {
            Debug.Log($"Damage {damage}. You're Dead!");
            OnDeath?.Invoke();
        }
        else
        {
            Debug.Log($"Damage {damage}");
        }
    }

   
}
