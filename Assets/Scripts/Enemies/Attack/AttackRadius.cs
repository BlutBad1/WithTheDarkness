
using UnityEngine;

namespace EnemyAttackNS
{ 
[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : EnemyAttack
{
    [HideInInspector]
    public SphereCollider Collider;
    public LayerMask WhatIsPlayer;
  

    protected void Awake()
    {
        Collider = GetComponent<SphereCollider>();
        Collider.radius = AttackRadius;
    }
    private void OnTriggerEnter(Collider other)
    {


        if ((1 << other.gameObject.layer) == WhatIsPlayer)
        {


          
            playerDamageable = other.GetComponent<IDamageable>();

                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(Attack());

           
        }


    }
  
    private void OnTriggerExit(Collider other)
    {


        if ((1 << other.gameObject.layer) == WhatIsPlayer)
        {
           
            if ((other.TryGetComponent(out ILastTouched lastTouched)&& lastTouched.iLastExited is SphereCollider) || (transform.position-other.transform.position).magnitude>AttackDistance)
            {
            
                if (playerDamageable != null)
                {
                    playerDamageable = null;
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }

            }
        }

    }




}
}