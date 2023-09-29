using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [Serializable]
    public class Key
    {
        public bool IsGeneric = false;
        public string KeyName;
    }
    [CreateAssetMenu(fileName = "AvailableKeyData", menuName = "ScriptableObject/Locking/AvailableKeyData")]
    public class AvailableKeyData : ScriptableObject
    {
        public List<Key> AvailableKeys = new List<Key>();
    }
}