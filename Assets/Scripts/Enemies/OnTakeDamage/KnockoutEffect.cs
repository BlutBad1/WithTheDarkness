using EnemyBaseNS;
using System.Collections;
using UnityEngine;

namespace EnemyOnTakeDamageNS
{


    public class KnockoutEffect : MonoBehaviour
    {

        private Enemy enemy;
        public bool KnockoutEnable = false;
        private bool isInKnockout = false;
        private Rigidbody mainRigidbody;
        public float InKnockoutTime = 0.3f;
        public float KnockoutForce = 1f;
        void Start()
        {
            enemy = GetComponent<Enemy>();
            mainRigidbody = GetComponent<Rigidbody>();
            enemy.OnTakeDamage += OnTakeDamage;
        }
        private void OnTakeDamage(float force, Vector3 hit)
        {
         
            if (KnockoutEnable && enemy.Health > 0)
            {

                Vector3 moveDirection = transform.position - hit;
                if (!isInKnockout)
                {
                
                    enemy.Agent.enabled = false;
                    mainRigidbody.isKinematic = false;
                    isInKnockout = true;
                    enemy.Agent.velocity = Vector3.zero;
                    if (mainRigidbody != null)
                        mainRigidbody.AddForce(moveDirection.normalized * KnockoutForce * force, ForceMode.Impulse);

                    StartCoroutine(KnockBackTimer(mainRigidbody));

                }


            }
        }
        IEnumerator KnockBackTimer(Rigidbody hittedRigidbody)
        {

            yield return new WaitForSeconds(InKnockoutTime);
            if (enemy.Health > 0)
            {
                enemy.Agent.enabled = true;
                if (hittedRigidbody != null)
                    hittedRigidbody.isKinematic = true;

              
            }
            isInKnockout = false;

        }



    }
}