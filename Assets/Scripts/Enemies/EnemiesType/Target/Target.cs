using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyTargetNS
{ 
public class Target : Damageable
{

    public float Speed=250;

 

    private void Awake()
    {
        Health = 25;
    }


    public override void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            GetComponent<AudioSource>().Play();
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z), Speed).SetUpdate(UpdateType.Normal, true);

        }

    }


 
}
}