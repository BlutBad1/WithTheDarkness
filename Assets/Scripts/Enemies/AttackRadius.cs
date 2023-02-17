using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour
{
    [HideInInspector]
    public SphereCollider Collider;
   
    public int Damage = 10;
    public float AttackDelay = 0.5f;
    public float AttackRange = 0.5f;
    public delegate void AttackEvent(IDamageable Target);
    public AttackEvent OnAttack;
    private IDamageable Damageable;
    private Coroutine AttackCoroutine;
    public LayerMask whatIsPlayer;
 
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }
  
    private void OnTriggerEnter(Collider other)
    {

      
        if ((1<<other.gameObject.layer)==whatIsPlayer)
        {
            Damageable = other.GetComponent<IDamageable>();
       
            if (AttackCoroutine == null)
                AttackCoroutine = StartCoroutine(Attack());
          

        }
            
        
    }


    private void OnTriggerExit(Collider other)
    {
      
        if ((1 << other.gameObject.layer) ==  whatIsPlayer)
        {
            if (Damageable!=null)
            {
                Damageable = null;
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
                
            
        }
    }

    private IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);
       
      
        while (Damageable!=null)
        {

        
          
            OnAttack?.Invoke(Damageable);
            Damageable.TakeDamage(Damage);
            yield return Wait;
        }

       


        //IDamageable player = null;
        //float closestDistance = float.MaxValue;


        //    if (player != null)
        //    {
        //        OnAttack?.Invoke(player);

        //        player.TakeDamage(Damage);
        //    }

        //    player = null;
        //    closestDistance = float.MaxValue;


        /////////////////

    }

    
}