using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public GameObject player;
    public float FieldOfView = 90f;
    public float CheckRaduis = 10f;
   
    public delegate void GainSightEvent(GameObject player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(GameObject player);
    public LoseSightEvent OnLoseSight;
    public LayerMask whatIsPlayer;
    private Coroutine CheckForLineOfSightCoroutine;
    
  
    private void Update()
    {

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (CheckLineOfSight(player)&& distanceToPlayer <= CheckRaduis)
            {            
                OnGainSight?.Invoke(player);
            }
            else
            {
                OnLoseSight?.Invoke(player);
            }
       
            //if(!CheckLineOfSight(player) && isBeenChasing)
            //{
            //    OnLoseSight?.Invoke(player);
            //    //if (CheckForLineOfSightCoroutine != null)
            //    //{
            //    //    StopCoroutine(CheckForLineOfSightCoroutine);
            //    //}
            //}
        
    }
    

    private bool CheckLineOfSight(GameObject player)
    {
        Vector3 Direction = (player.transform.position - transform.position).normalized;
        float DotProduct = Vector3.Dot(transform.forward, Direction);
        if (DotProduct >= Mathf.Cos(FieldOfView))
        {
            RaycastHit Hit;

            if (Physics.Raycast(transform.position, Direction, out Hit, CheckRaduis, whatIsPlayer))
            {
                if ((1<<Hit.collider.gameObject.layer)==whatIsPlayer)
                {
                   
                  
                    return true;
                }
            }
        }
       
        return false;
    }

    private IEnumerator CheckForLineOfSight(GameObject player)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return Wait;
        }
    }
}
