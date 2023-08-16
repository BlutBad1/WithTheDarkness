using System;
using UnityEngine;
namespace PoolableObjectsNS
{

    [Serializable]
    public class BulletHolesPoolableObject 
    {
        public string Name;
        public GameObject Prefab;
        public int Size = 10;
        [HideInInspector]
        public int Iterator = 0;
        [HideInInspector]
        public GameObject[] poolableObjects;
    }
}

