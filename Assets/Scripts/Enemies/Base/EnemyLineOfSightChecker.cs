using System.Collections;
using UnityEngine;
namespace EnemyBaseNS
{
    public class EnemyLineOfSightChecker : MonoBehaviour
    {
        public GameObject Player;
        public float FieldOfView = 90f;
        public float ViewDistance = 10f;

        public delegate void GainSightEvent(GameObject player);
        public GainSightEvent OnGainSight;
        public delegate void LoseSightEvent(GameObject player);
        public LoseSightEvent OnLoseSight;
        public LayerMask WhatIsPlayer;
        private Coroutine checkForLineOfSightCoroutine;
        float distanceToPlayer;


        private void Update()
        {

                distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
                if (distanceToPlayer <= ViewDistance&&checkLineOfSight(Player))
                {
                    OnGainSight?.Invoke(Player);
                }
                else
                {
                    OnLoseSight?.Invoke(Player);
                }
          


        }


        private bool checkLineOfSight(GameObject player)
        {
            Vector3 Direction = (player.transform.position - transform.position).normalized;
            float DotProduct = Vector3.Dot(transform.forward, Direction);
            if (DotProduct >= Mathf.Cos(FieldOfView))
            {
                RaycastHit Hit;

                if (Physics.Raycast(transform.position, Direction, out Hit, ViewDistance, WhatIsPlayer))
                {
                    if ((1 << Hit.collider.gameObject.layer) == WhatIsPlayer)
                    {


                        return true;
                    }
                }
            }

            return false;
        }

        private IEnumerator checkForLineOfSight(GameObject player)
        {
            WaitForSeconds Wait = new WaitForSeconds(0.1f);

            while (!checkLineOfSight(player))
            {
                yield return Wait;
            }
        }
    }
}
