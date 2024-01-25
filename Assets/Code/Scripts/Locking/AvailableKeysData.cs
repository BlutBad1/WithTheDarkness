using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjectNS.Locking
{
    [CreateAssetMenu(fileName = "AvailableKeyData", menuName = "ScriptableObject/Locking/AvailableKeyData")]
    public class AvailableKeysData : ScriptableObject
    {
        [SerializeField, FormerlySerializedAs("AvailableKeys")]
        private List<AvailableKeyData> availableKeys = new List<AvailableKeyData>();
        public List<AvailableKeyData> AvailableKeys { get => availableKeys; set => availableKeys = value; }
    }
}