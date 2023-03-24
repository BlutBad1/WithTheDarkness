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
        protected const string PLAYER = "Player";

        private void Start()
        {
            if (Player == null)
            {
                Player = GameObject.Find(PLAYER);
                if (Player==null)
                {
                    Debug.LogError("Player not found");
                }
            }
        }
        private void Update()
        {
            if (checkForLineOfSightCoroutine == null)
            {
                distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
                if (distanceToPlayer <= ViewDistance)
                {
                    checkForLineOfSightCoroutine = StartCoroutine(checkForLineOfSight(Player));
                }
                else
                {
                  
                    OnLoseSight?.Invoke(Player);
                }

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
             
                OnLoseSight?.Invoke(Player);
                yield return Wait;
            }
          
            OnGainSight?.Invoke(Player);
            checkForLineOfSightCoroutine = null;
        }
    }
}
