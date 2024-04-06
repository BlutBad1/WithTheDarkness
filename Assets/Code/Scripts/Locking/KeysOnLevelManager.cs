using LocationManagementNS;
using System.Collections.Generic;
using System.Linq;
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

        private List<AvailableKeyData> requiredRegularKeys = new List<AvailableKeyData>();

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
            RemoveKeyFromCollection(requiredRegularKeys, randomKey);
            return randomKey;
        }
        public void RemoveKeyFromAvailable(KeyData key) =>
            RemoveKeyFromCollection(availableKeyData.AvailableKeys, key);
        public bool HaveKeyInAvailable(KeyData key) =>
            HaveKeyInCollection(availableKeyData.AvailableKeys, key);
        public bool HaveKeyInRequired(KeyData key) =>
            HaveKeyInCollection(requiredKeysToCompleteLevel, key);
        public KeyData GetMostImportantRequiredKey()
        {
            int maxAmount = requiredKeysToCompleteLevel.Max(x => x.Amount);
            KeyData theMostImportantKey = null;
            if (LocationsSpawnController.Instance.GetAmountOfRemainingMaps() <= requiredKeysToCompleteLevel.Sum(x => x.Amount))
                theMostImportantKey = requiredKeysToCompleteLevel.First(x => x.Amount == maxAmount && x.Amount != 0);
            RemoveKeyFromCollection(requiredKeysToCompleteLevel, theMostImportantKey);
            return theMostImportantKey;
        }
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
                if (keyDataInDB.IsGeneric)
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
        private AvailableKeyData GetKeyFromCollection(List<AvailableKeyData> keyCollection, KeyData key) =>
            keyCollection.Find(x => x.IsGeneric == key.IsGeneric && x.KeyName == key.KeyName);
    }
}