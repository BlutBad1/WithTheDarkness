using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 25f;
   

    public void TakeDamage(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0)
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z),500).SetUpdate(UpdateType.Normal,true);
       
    }
}
