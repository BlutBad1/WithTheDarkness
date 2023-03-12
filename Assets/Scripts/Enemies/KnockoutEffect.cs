using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoutEffect : MonoBehaviour
{
  
    Enemy enemy;
    public bool KnockoutEnable = false;
    private bool isInKnockout = false;
    private Rigidbody mainRigidbody;
    public float InKnockoutTime = 0.3f;
    public float KnockoutForce= 1f;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        mainRigidbody = GetComponent<Rigidbody>();
        enemy.OnTakeDamage += OnTakeDamage;   
    }
    private void OnTakeDamage(GunData weapon, RaycastHit hit)
    {
       
        if (KnockoutEnable && enemy.Health>0)
        {
            Vector3 moveDirection = transform.position - hit.point;  
            if (!isInKnockout)
            {
             
                enemy.Agent.enabled = false;
                mainRigidbody.isKinematic = false;
                isInKnockout = true;
                enemy.Agent.velocity = Vector3.zero;
                if (mainRigidbody != null)
                    mainRigidbody.AddForce(moveDirection.normalized * KnockoutForce * weapon.force, ForceMode.Impulse);

                StartCoroutine(KnockBackTimer(mainRigidbody));

            }


        }
    }
    IEnumerator KnockBackTimer(Rigidbody hittedRigidbody)
    {
        
            float timeElapsed = 0f;
            while (timeElapsed < InKnockoutTime)
            {
                timeElapsed += Time.deltaTime;
                yield return null;

            }
            if (enemy.Health > 0)
            {
                enemy.Agent.enabled = true;
                if (hittedRigidbody != null)
                    hittedRigidbody.isKinematic = true;

                isInKnockout = false;
            }
          
        
    }
           

    
}
