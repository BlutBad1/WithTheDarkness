using System;
using UnityEngine;
namespace PoolableObjectsNS
{
    [Serializable]
    public class DamageDecalData
    {
        public string Name;
        public GameObject Prefab;
        [Min(0)]
        public int Size = 10;
        [HideInInspector, Min(0)]
        public int Iterator = 0;
        [HideInInspector]
        public GameObject[] poolableObjects;
    }
}

