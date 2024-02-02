using InteractableNS.Usable.Locking;
using ScriptableObjectNS.Locking;
using UnityEngine;

namespace GameObjectsControllingNS
{
    [RequireComponent(typeof(KeyInteractable))]
    public class KeySpawnChance : ObjectSpawnChance<KeySpawnChance, KeySupply>
    {
        [SerializeField]
        private KeyInteractable keyInteractable;
        [SerializeField]
        private bool spawnRegularRequired = true;

        protected override void Start()
        {
            SpawnKey();
        }
        private void SpawnKey()
        {
            KeysOnLevelManager keysOnLevelManager = KeysOnLevelManager.Instance;
            KeyData keyToSpawn = keysOnLevelManager.GetMostImportantRequiredKey();
            KeyData key = keyInteractable.GetKeyData();
            if (keyToSpawn == null)
            {
                keyToSpawn = spawnRegularRequired ? keysOnLevelManager.GetRandomRegularReguiredKey() : key;
                CalculateIfSpawn();
            }
            keyInteractable.SetKeyData(keyToSpawn);
        }
        private void CalculateIfSpawn()
        {
            AssignSupplyType();
            if (IsConnectedToSupply)
            {
                if (!objectsSupplyInstance.CalculateObjectChances(Chance))
                    gameObject.SetActive(false);
            }
            else if (Chance > UnityEngine.Random.Range(0, 100))
                gameObject.SetActive(false);
        }
    }
}