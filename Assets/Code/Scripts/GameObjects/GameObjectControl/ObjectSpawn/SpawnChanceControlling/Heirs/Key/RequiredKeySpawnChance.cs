using InteractableNS.Usable.Locking;
using LocationManagementNS;
using ScriptableObjectNS.Locking;
using UnityEngine;

namespace GameObjectsControllingNS
{
    [RequireComponent(typeof(KeyInteractable))]
    public class RequiredKeySpawnChance : ObjectSpawnChance<RequiredKeySpawnChance, RequiredKeySupply>
    {
        protected override void Start()
        {
            CalculateIfSpawn();
            // Destroy(gameObject);
        }
        public KeyData GetKey()
        {
            KeyData key = GetComponent<KeyInteractable>().Key;
            return key;
        }
        private void CalculateIfSpawn()
        {
            AssignSupplyType();
            if (IsConnectedToSupply)
            {
                KeyData key = GetKey();
                KeysOnLevelManager keysOnLevelManager = KeysOnLevelManager.Instance;
                if (CheckCondtions(key))
                {
                    if (!objectsSupplyInstance.CalculateObjectChances(Chance))
                        gameObject.SetActive(false);
                }
                else
                    keysOnLevelManager.RemoveKeyFromRequired(key);
            }
            else if (Chance > UnityEngine.Random.Range(0, 100))
                gameObject.SetActive(false);
        }
        private bool CheckCondtions(KeyData key)
        {
            KeysOnLevelManager keysOnLevelManager = KeysOnLevelManager.Instance;
            return !keysOnLevelManager.HaveKeyInRequired(key) || !(MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= keysOnLevelManager.GetAmountKeyFromRequired(key));
        }
    }
}