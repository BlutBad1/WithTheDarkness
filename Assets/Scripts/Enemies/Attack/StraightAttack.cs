using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StraightAttack : EnemyAttack
{
    
    public LayerMask WhatIsPlayer;
    protected const string PLAYER = "Player";
    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public SphereCollider AttackColider;
   
    protected void Start()
    {
        Player = GameObject.Find(PLAYER);
        playerDamageable = Player.GetComponent<IDamageable>();
      
        if (!Player.TryGetComponent(out AttackColider))
        {
            AttackColider = GetComponentInChildren(typeof(SphereCollider)) as SphereCollider;
            if (AttackColider==null)
                Debug.LogError("SphereCollider is not found!");
           
         
        }
        AttackColider.radius = AttackRadius;
        AttackColider.enabled = false;
    }
    public bool CanAttack(GameObject enemy, GameObject player)
    {

        enemy.TryGetComponent(out Enemy enemyScript);
        if (enemyScript != null && enemyScript.Health > 0)
        {
            Vector3 origin = enemy.transform.position;
            origin.y = player.transform.position.y;

            Ray ray = new Ray(origin, player.transform.position - origin);
            if (Physics.SphereCast(ray, 0.3f, (player.transform.position - enemy.transform.position).magnitude, ~(1 << 11 | 1 << 12 | 1 << 20 | 1 << 8 | 1 << 7)))
            {
                return false;
            }

            return true;
        }
        StopAttack();
        return false;
        
        
       
    }

    protected void Update()
    {

      
        if ((transform.position - Player.transform.position).magnitude <= AttackDistance)
        {
            
            if (CanAttack(gameObject,Player))
            {
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(Attack());
                IsAttacking = true;
            }
          


        }
      
    }
    public void StopAttack()
    {
        if (attackCoroutine!=null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = null;
        AttackColider.enabled = false;
        IsAttacking = false;


    }


    public void TryAttack()
    {
        AttackColider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {

        if ((1 << other.gameObject.layer) == WhatIsPlayer)
        {
           
            if (IsAttacking)
            {
                playerDamageable.TakeDamage(Damage);
            }
          

        }
    }
    protected override IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        while (playerDamageable != null)
        {

            OnAttack?.Invoke(playerDamageable);
            yield return Wait;
        }

    }


}
