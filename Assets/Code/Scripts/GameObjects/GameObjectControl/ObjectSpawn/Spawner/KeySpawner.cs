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
        protected override SpawningGameObject SpawnGameObject(List<SpawningSupply> spawningGameObjects)
        {
            SpawningGameObject gameobjectToSpawn = null;
            if (spawningGameObjects != null)
            {
                gameobjectToSpawn = KeysOnLevelManager.Instance ? GetMostImportantRequiredKey(spawningGameObjects) : null;
                //if (gameobjectToSpawn == null && spawningGameObjects.Where(x => x.SpawnChance > 0).ToList().Count > 0)
                //{
                //    gameobjectToSpawn = base.SpawnGameObject(spawningGameObjects);
                //    KeyData keyData = GetRequiredKeyDataFromGameobject(gameobjectToSpawn.GameObject);
                //    if (keyData != null)
                //        keyData.Amount--;
                //}
                //else
                if (gameobjectToSpawn != null)
                    gameobjectToSpawn.GameObject = GameObject.Instantiate(gameobjectToSpawn.GameObject, SpawnPoint.position, SpawnPoint.rotation, gameobjectToSpawn.Parent);
            }
            return gameobjectToSpawn;
        }
        private KeyData GetRequiredKeyDataFromGameobject(GameObject gameObject)
        {
            RequiredKeySpawnChance spawnChanceBase = null;
            KeyData requiredKey = null;
            spawnChanceBase = (RequiredKeySpawnChance)GetObjectSpawnChanceFromGameObject(gameObject);
            if (spawnChanceBase)
            {
                requiredKey = Array.Find(KeysOnLevelManager.Instance.RequiredKeys, x => x.IsGeneric == spawnChanceBase.GetKey().IsGeneric
                   && (x.GenericKeyName == spawnChanceBase.GetKey().GenericKeyName || x.KeyName == spawnChanceBase.GetKey().KeyName) && x.Amount > 0);
            }
            return requiredKey;
        }
        private SpawningSupply GetMostImportantRequiredKey(List<SpawningSupply> spawningGameObjects)
        {
            Dictionary<KeyData, SpawningSupply> importantKeys = new Dictionary<KeyData, SpawningSupply>();
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
                    if (keyBuffer.Amount < key.Amount && MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator <= key.Amount)
                        keyBuffer = key;
                    else if (keyBuffer.Amount > key.Amount && MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator > keyBuffer.Amount)
                        keyBuffer = key;
                }
                if (MapData.Instance.ActiveLocations.Count - MapData.Instance.LocationsIterator > importantKeys.Sum(x => x.Key.Amount))
                {
                    float spawnChance = importantKeys[keyBuffer].SpawnChance;
                    if (MapData.Instance.ActiveLocations.Count / 2 > MapData.Instance.LocationsIterator)
                        spawnChance /= KeySpawnerConstants.REQUIRED_KEY_SPAWN_CHANCE_DIV_COEFF_ON_FIRST_HALF;
                    if (spawnChance > UnityEngine.Random.Range(0, 100))
                        theMostImportantKey = importantKeys[keyBuffer];
                }
                else
                    theMostImportantKey = importantKeys[keyBuffer];
                keyBuffer.Amount--;
            }
            return theMostImportantKey;
        }
    }
}