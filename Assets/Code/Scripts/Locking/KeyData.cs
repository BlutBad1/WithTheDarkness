using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [Serializable]
    public class KeyData : ISerializationCallbackReceiver
    {
        public static List<string> LockingTypes;

        public bool IsGeneric = false;
        [ListToPopup(typeof(KeyData), "LockingTypes", "Key Name")]
        public string KeyName;

        public KeyData(bool isGeneric, string genericKeyName)
        {
            KeyName = genericKeyName;
            IsGeneric = isGeneric;
        }
        public KeyData(KeyData key)
        {
            IsGeneric = key.IsGeneric;
            KeyName = key.KeyName;
        }
        public void OnAfterDeserialize() { }
        public void OnBeforeSerialize() =>
            LockingTypes = LockingTypeData.Instance?.LockingTypes;
    }
}

