using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [CreateAssetMenu(fileName = "AvailableKeyData", menuName = "ScriptableObject/Locking/AvailableKeyData")]
    public class AvailableKeyData : ScriptableObject
    {
        public List<KeyData> AvailableKeys = new List<KeyData>();
    }
}