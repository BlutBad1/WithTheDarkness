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
                int mutualSpawnChance = (int)notNullChanceGameObjects.Sum(x => x.SpawnChance);
                int randomNumber = UnityEngine.Random.Range(1, mutualSpawnChance);
                float spawnChanceCounter = 0;
                foreach (var spawningGameObject in notNullChanceGameObjects)
                {
                    if (randomNumber > spawnChanceCounter && randomNumber <= spawnChanceCounter + spawningGameObject.SpawnChance)
                    {
                        gameobjectToSpawn = spawningGameObject;
                        break;
                    }
                    spawnChanceCounter += spawningGameObject.SpawnChance;
                }
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
