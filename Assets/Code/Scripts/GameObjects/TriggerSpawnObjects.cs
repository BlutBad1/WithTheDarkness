using System;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class TriggerSpawnObjects : MonoBehaviour
    {
        [Serializable]
        public class ObjectToSpawn
        {
            public string name;
            public GameObject spawnObject;
            public int amount;
        }
        public Collider collider;
        GameObject spawnPosition;
        public ObjectToSpawn[] objects;

    }
}