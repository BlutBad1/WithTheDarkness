using ExtensionMethods;
using LocationManagementNS;
using MyConstants.EnironmentConstants.SpawnerConstants;
using ScriptableObjectNS.Locking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjectsControllingNS.Spawner
{
    public class KeySpawner : SupplySpawner
    {
        protected override void Start()
        {
            SpawningSupply supplyGameObject = (SpawningSupply)SpawnGameObject(SpawningGameObjects);
            if (supplyGameObject != null && supplyGameObject.GameObject)
            {
                ObjectSpawnChanceBase supplySpawningObject = supplyGameObject.GameObject.GetComponentOrInherited<ObjectSpawnChanceBase>();
                if (!supplySpawningObject)
                    supplySpawningObject = supplyGameObject.GameObject.GetComponentInChildren<ObjectSpawnChanceBase>();
                if (supplySpawningObject)
                    supplySpawningObject.Chance = supplyGameObject.SupplySpawnChance;
            }
            gameObject.SetActive(false);
        }
        protected override SpawningGameObject SpawnGameObject(List<SpawningSupply> spawningGameObjects)
        {
            SpawningGameObject gameobjectToSpawn = null;
            if (spawningGameObjects != null)
            {
                gameobjectToSpawn = KeysOnLevelManager.Instance ? GetMostImportantRequiredKey(spawningGameObjects) : null;
                if (gameobjectToSpawn == null && spawningGameObjects.Where(x => x.SpawnChance > 0).ToList().Count > 0)
                    gameobjectToSpawn = base.SpawnGameObject(spawningGameObjects);
                else if (gameobjectToSpawn != null)
                    gameobjectToSpawn.GameObject = GameObject.Instantiate(gameobjectToSpawn.GameObject, SpawnTransform.position, SpawnTransform.rotation, gameobjectToSpawn.Parent);
            }
            return gameobjectToSpawn;
        }
        private SpawningSupply GetMostImportantRequiredKey(List<SpawningSupply> spawningGameObjects)
        {
            Dictionary<KeyData, SpawningSupply> importantKeys = new Dictionary<KeyData, SpawningSupply>();
            RequiredKeySpawnChance spawnChanceBase = null;
            KeyData requiredKey = null;
            foreach (var spawningSupply in spawningGameObjects)
            {
                spawnChanceBase = (RequiredKeySpawnChance)GetObjectSpawnChanceFromGameObject(spawningSupply.GameObject);
                if (spawnChanceBase)
                {
                    requiredKey = Array.Find(KeysOnLevelManager.Instance.RequiredKeys, x => x.IsGeneric == spawnChanceBase.GetKey().IsGeneric
                    && (x.GenericKeyName == spawnChanceBase.GetKey().KeyName || x.KeyName == spawnChanceBase.GetKey().KeyName));
                    if (requiredKey != null)
                        importantKeys.Add(requiredKey, spawningSupply);
                }
            }
            SpawningSupply theMostImportantKey = null;
            if (importantKeys.Count > 0)
            {
                KeyData keyBuffer = importantKeys.Keys.First();
                foreach (KeyData key in importantKeys.Keys)
                {
                    if (keyBuffer.Amount > key.Amount && MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= key.Amount)
                        keyBuffer = key;
                    else if (keyBuffer.Amount > key.Amount && !(MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= keyBuffer.Amount))
                        keyBuffer = key;
                }
                if (!(MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= keyBuffer.Amount))
                {
                    if (importantKeys[keyBuffer].SpawnChance > UnityEngine.Random.Range(0, 100 / KeySpawnerConstants.REQUIRED_KEY_SPAWN_CHANCE_COEFF_ON_SECOND_HALF))
                        theMostImportantKey = importantKeys[keyBuffer];
                    spawningGameObjects.Remove(importantKeys[keyBuffer]);
                }
                else
                    theMostImportantKey = importantKeys[keyBuffer];
            }
            return theMostImportantKey;
        }
    }
}