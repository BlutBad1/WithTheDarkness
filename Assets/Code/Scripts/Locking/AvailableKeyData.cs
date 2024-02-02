using System;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [Serializable]
    public class AvailableKeyData : KeyData
    {
        [Min(0)]
        public int Amount = 1;
        public AvailableKeyData(KeyData keyData)
         : base(keyData.IsGeneric, keyData.KeyName)
        {
            Amount = 1;
        }
    }
}