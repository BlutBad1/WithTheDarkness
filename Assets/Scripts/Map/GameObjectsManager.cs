using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocationManagementNS
{
    public class GameObjectsManager : MonoBehaviour
    {
        public GameObject[] GameObjects;
        public float SpawnDistance=50f;
        [SerializeField]
        private GameObject player;

        private bool isSpawned = false;
       
        void Start()
        {
            if (player==null)
            {
                player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
            }
            foreach (var gameObject in GameObjects)
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
              
                if (!isSpawned && Vector3.Distance(player.transform.position,transform.position)<= SpawnDistance)
                {
                    isSpawned = true;
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.SetActive(true);
                    }
                }
               
            }

        }
    }

}


