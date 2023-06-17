using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class EventsSpawnChance : MonoBehaviour
    {
        public float Chance = 100f;
        void Awake()
        {
            if(!EventsSupply.CalculateEventChances(Chance))
                Destroy(gameObject);
        }
    }
}