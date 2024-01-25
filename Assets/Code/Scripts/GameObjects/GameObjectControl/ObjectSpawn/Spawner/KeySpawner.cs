using LocationManagementNS;
using MyConstants.EnironmentConstants.SpawnerConstants;
using ScriptableObjectNS.Locking;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjectsControllingNS.Spawner
{
    public class KeySpawner : SupplySpawner
    {
        protected override SpawningGameObject SpawnGameObject(List<SpawningSupply> spawningGameObjects)
        {
            SpawningGameObject gameobjectToSpawn = null;
            if (spawningGameObjects != null)
            {
                gameobjectToSpawn = KeysOnLevelManager.Instance ? GetMostImportantRequiredKey(spawningGameObjects) : null;
                if (gameobjectToSpawn != null)
                    gameobjectToSpawn.GameObject = GameObject.Instantiate(gameobjectToSpawn.GameObject, SpawnPoint.position, SpawnPoint.rotation, gameobjectToSpawn.Parent);
            }
            return gameobjectToSpawn;
        }
        private SpawningSupply GetMostImportantRequiredKey(List<SpawningSupply> spawningGameObjects)
        {
            Dictionary<KeyData, SpawningSupply> importantKeys = new Dictionary<KeyData, SpawningSupply>();
            KeysOnLevelManager keysOnLevelManager = KeysOnLevelManager.Instance;
            foreach (var spawningSupply in spawningGameObjects)
            {
                KeyData requiredKey = GetRequiredKeyDataFromGameobject(spawningSupply.GameObject);
                if (requiredKey != null)
                    importantKeys.Add(requiredKey, spawningSupply);
            }
            SpawningSupply theMostImportantKey = null;
            if (importantKeys.Count > 0)
            {
                KeyData keyBuffer = importantKeys.Keys.First();
                foreach (KeyData key in importantKeys.Keys)
                {
                    int keyAmount = keysOnLevelManager.GetAmountKeyFromRequired(key);
                    int keyBufferAmount = keysOnLevelManager.GetAmountKeyFromRequired(keyBuffer);
                    if (keyBufferAmount < keyAmount && MapData.Instance.GetAmountOfRemainingMaps() <= keyAmount)
                        keyBuffer = key;
                    else if (keyBufferAmount > keyAmount && MapData.Instance.GetAmountOfRemainingMaps() > keyAmount)
                        keyBuffer = key;
                }
                if (MapData.Instance.GetAmountOfRemainingMaps() > importantKeys.Sum(x => keysOnLevelManager.GetAmountKeyFromRequired(x.Key)))
                {
                    float spawnChance = importantKeys[keyBuffer].SpawnChance;
                    if (MapData.Instance.ActiveLocations.Count / 2 > MapData.Instance.LocationsIterator)
                        spawnChance /= KeySpawnerConstants.REQUIRED_KEY_SPAWN_CHANCE_DIV_COEFF_ON_FIRST_HALF;
                    if (spawnChance > UnityEngine.Random.Range(0, 100))
                        theMostImportantKey = importantKeys[keyBuffer];
                }
                else
                    theMostImportantKey = importantKeys[keyBuffer];
            }
            return theMostImportantKey;
        }
        private KeyData GetRequiredKeyDataFromGameobject(GameObject gameObject)
        {
            RequiredKeySpawnChance spawnChanceBase = null;
            KeyData requiredKey = null;
            spawnChanceBase = (RequiredKeySpawnChance)GetObjectSpawnChanceFromGameObject(gameObject);
            if (spawnChanceBase)
                requiredKey = spawnChanceBase.GetKey();
            return requiredKey;
        }
    }
}