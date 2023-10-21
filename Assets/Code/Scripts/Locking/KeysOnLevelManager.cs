using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    public class KeysOnLevelManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public KeyData[] RequiredKeys;
        public bool SetKeysOnStart = false;
        public List<KeyData> AvailableKeysOnStart;
        public AvailableKeyData AvailableKeyData;
        public static KeysOnLevelManager Instance;
        public void OnAfterDeserialize()
        {
        }
        public void OnBeforeSerialize()
        {
            KeyData.LockingTypes = LockingTypeData.Instance?.LockingTypes;
        }
        private void OnEnable()
        {
            if (!Instance)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
            if (!AvailableKeyData)
                Debug.LogWarning("AvailableKeyData is not set!");
            if (SetKeysOnStart && AvailableKeyData)
                AvailableKeyData.AvailableKeys = AvailableKeysOnStart;
        }
    }
}