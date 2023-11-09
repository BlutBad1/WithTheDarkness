using MyConstants.EnironmentConstants.SpawnerConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjectsControllingNS.Spawner
{
    [Serializable]
    public class SpawningGameObject
    {
        public GameObject GameObject;
        public Transform Parent;
        [Min(0)]
        public float SpawnChance;
        public bool IsPrefab = true;
    }
    public class SpawnerBase<T> : MonoBehaviour where T : SpawningGameObject
    {
        public Transform SpawnTransform;
        public List<T> SpawningGameObjects;
        protected virtual void Start()
        {
            SpawnGameObject(SpawningGameObjects);
            gameObject.SetActive(false);
        }
        protected virtual SpawningGameObject SpawnGameObject(List<T> spawningGameObjects)
        {
            SpawningGameObject gameobjectToSpawn = null;
            List<T> notNullChanceGameObjects = spawningGameObjects.Where(x => x.SpawnChance > 0).ToList();
            if (spawningGameObjects != null && notNullChanceGameObjects.Count > 0)
            {
                int amountOfCycles = 0;
                while (gameobjectToSpawn == null && amountOfCycles <= MainSpawnerConstants.MAX_AMOUNT_OF_CYCLES)
                {
                    foreach (SpawningGameObject spawningGameObject in notNullChanceGameObjects)
                    {
                        if (spawningGameObject.SpawnChance > UnityEngine.Random.Range(0, 100))
                        {
                            gameobjectToSpawn = spawningGameObject;
                            break;
                        }
                    }
                    amountOfCycles++;
                }
                if (gameobjectToSpawn == null)
                    gameobjectToSpawn = notNullChanceGameObjects[UnityEngine.Random.Range(0, notNullChanceGameObjects.Count)];
                if (gameobjectToSpawn.IsPrefab)
                    gameobjectToSpawn.GameObject = GameObject.Instantiate(gameobjectToSpawn.GameObject, SpawnTransform.position, SpawnTransform.rotation, gameobjectToSpawn.Parent);
                else
                    gameobjectToSpawn.GameObject.transform.position = SpawnTransform.position;
            }
            foreach (var spawningGameObject in spawningGameObjects)
            {
                if (!spawningGameObject.IsPrefab && gameobjectToSpawn.GameObject != spawningGameObject.GameObject)
                    spawningGameObject.GameObject.SetActive(false);
            }
            return gameobjectToSpawn;
        }
    }
}
