using InteractableNS.Usable.Locking;
using LocationManagementNS;
using ScriptableObjectNS.Locking;
using System;
using UnityEngine;

namespace GameObjectsControllingNS
{
    [RequireComponent(typeof(KeyInteractable))]
    public class RequiredKeySpawnChance : ObjectSpawnChance<RequiredKeySpawnChance, RequiredKeySupply>
    {
        protected override void Start()
        {
            AssignSupplyType();
            if (IsConnectedToSupply)
            {
                Key key = GetKey();
                RequiredKey requiredKey = Array.Find(KeysOnLevelManager.Instance.RequiredKeys, x => x.IsGeneric == key.IsGeneric
                && (x.GenericKeyName == key.KeyName || x.KeyName == key.KeyName));
                if (requiredKey == null || !(MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= requiredKey.Amount))
                    if (!objectsSupplyInstance.CalculateObjectChances(Chance))
                        Destroy(gameObject);
                if (requiredKey != null)
                    requiredKey.Amount--;
            }
            else if (Chance > UnityEngine.Random.Range(0, 100))
                Destroy(gameObject);
        }
        public Key GetKey()
        {
            Key key = GetComponent<KeyInteractable>().Key;
            return key;
        }
    }
}