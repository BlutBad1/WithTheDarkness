using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightAttack : EnemyAttack
{
    
    public LayerMask WhatIsPlayer;
    protected const string PLAYER = "Player";
    [HideInInspector]
    public GameObject Player;

    protected virtual void Awake()
    {
        Player = GameObject.Find(PLAYER);
    }
    protected virtual void Update()
    {
        Debug.Log((transform.position - Player.transform.position).magnitude);
        if ((transform.position - Player.transform.position).magnitude <= AttackDistance)
        {


            damageable = Player.GetComponent<IDamageable>();
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(Attack());


        }
        else
        {
            if (damageable != null)
            {
                damageable = null;
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }


   
}
