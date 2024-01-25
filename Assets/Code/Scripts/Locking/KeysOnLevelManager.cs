using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    public class KeysOnLevelManager : MonoBehaviour
    {
        private static KeysOnLevelManager instance;
        public static KeysOnLevelManager Instance { get => instance; }

        [SerializeField]
        private List<AvailableKeyData> requiredKeysToCompleteLevel;
        [SerializeField]
        private bool setKeysOnStart = false;
        [SerializeField]
        private List<AvailableKeyData> availableKeysOnStart;
        [SerializeField]
        private AvailableKeysData availableKeyData;

        private List<AvailableKeyData> requiredRegularKeys;

        private void OnEnable()
        {
            if (!instance)
                instance = this;
            else
                Destroy(this);
            if (!availableKeyData)
                Debug.LogWarning("AvailableKeyData is not set!");
            if (setKeysOnStart && availableKeyData)
                SetKeysData();
        }
        public void AddKeyToAvailable(KeyData key) =>
            AddKeyToCollection(availableKeyData.AvailableKeys, key);
        public void AddKeyToRegularRequired(KeyData key) =>
            AddKeyToCollection(requiredRegularKeys, key);
        public KeyData GetRandomRegularReguiredKey()
        {
            KeyData randomKey = requiredRegularKeys[Random.Range(0, requiredRegularKeys.Count)];
            RemoveKeyFromAvailable(randomKey);
            return randomKey;
        }
        public void RemoveKeyFromRequired(KeyData key) =>
            RemoveKeyFromCollection(requiredKeysToCompleteLevel, key);
        public void RemoveKeyFromAvailable(KeyData key) =>
            RemoveKeyFromCollection(availableKeyData.AvailableKeys, key);
        public bool HaveKeyInAvailable(KeyData key) =>
            HaveKeyInCollection(availableKeyData.AvailableKeys, key);
        public bool HaveKeyInRequired(KeyData key) =>
            HaveKeyInCollection(requiredKeysToCompleteLevel, key);
        public int GetAmountKeyFromRequired(KeyData key) =>
            GetKeyAmount(requiredKeysToCompleteLevel, key);
        private void SetKeysData() =>
            availableKeyData.AvailableKeys = availableKeysOnStart;
        private void AddKeyToCollection(List<AvailableKeyData> keyCollection, KeyData key)
        {
            AvailableKeyData keyDataInDB = GetKeyFromCollection(keyCollection, key);
            if (keyDataInDB == null)
            {
                keyDataInDB = new AvailableKeyData(key);
                keyCollection.Add(keyDataInDB);
            }
            else
                keyDataInDB.Amount++;
        }
        private void RemoveKeyFromCollection(List<AvailableKeyData> keyCollection, KeyData key)
        {
            AvailableKeyData keyDataInDB = GetKeyFromCollection(keyCollection, key);
            if (keyDataInDB != null)
            {
                keyDataInDB.Amount--;
                keyDataInDB.Amount = keyDataInDB.Amount < 0 ? 0 : keyDataInDB.Amount;
            }
        }
        private bool HaveKeyInCollection(List<AvailableKeyData> keyCollection, KeyData key)
        {
            AvailableKeyData keyDataInDB = GetKeyFromCollection(keyCollection, key);
            if (keyDataInDB != null)
                return keyDataInDB.Amount > 0;
            return false;
        }
        private int GetKeyAmount(List<AvailableKeyData> keyCollection, KeyData key) =>
            GetKeyFromCollection(keyCollection, key).Amount;
        private AvailableKeyData GetKeyFromCollection(List<AvailableKeyData> keyCollection, KeyData key)
        {
            if (key.IsGeneric)
                return keyCollection.Find(x => x.IsGeneric == true && x.GenericKeyName == key.GenericKeyName);
            else
                return keyCollection.Find(x => x.IsGeneric == false && x.KeyName == key.KeyName);
        }
    }
}