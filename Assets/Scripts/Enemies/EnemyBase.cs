using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class EnemyBase : MonoBehaviour, IDamageable
{

    public bool doesHaveKnockout, knockoutPhysics;
    public float force;
    [HideInInspector]
    public bool isInKnockout=false;
    public float inKnockoutTime = 0.3f;
    

    public virtual void TakeDamage(float damage, RaycastHit hit) 
    {
        if (doesHaveKnockout)
        {
            Vector3 moveDirection =  transform.position - hit.point;
          
            if (!isInKnockout)
            {
                isInKnockout = true;
                if (knockoutPhysics)
                {
                    GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                    GetComponent<NavMeshAgent>().enabled = false;
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().AddForce(moveDirection.normalized * force, ForceMode.Impulse);
                }
                StartCoroutine(KnockBackTimer());

            }    
           

        }
    }
    IEnumerator KnockBackTimer()
    {
        float timeElapsed=0f;
        while (timeElapsed < inKnockoutTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;

        }
       
        GetComponent<NavMeshAgent>().enabled = true;
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.isKinematic = true;

        isInKnockout = false;
    }
    protected virtual void DestroyEnemy() { }
    protected virtual void Patroling() { }

    protected virtual void SearchWalkPoint() { }


    protected virtual void ChasePlayer() { }


    protected virtual void AttackPlayer() { }

    protected virtual void ResetAttack() { }

   
}
