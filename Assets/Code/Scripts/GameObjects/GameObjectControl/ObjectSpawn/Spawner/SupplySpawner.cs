using System;
using UnityEngine;

namespace GameObjectsControllingNS.Spawner
{
    [Serializable]
    public class SpawningSupply : SpawningGameObject
    {
        [Min(0)]
        public float InnerSupplySpawnChance = 100f;
    }
    public class SupplySpawner : SpawnerBase<SpawningSupply>
    {
        protected override void Start()
        {
            SpawningSupply supplyGameObject = (SpawningSupply)SpawnGameObject(SpawningGameObjects);
            if (supplyGameObject != null && supplyGameObject.GameObject)
            {
                ObjectSpawnChanceBase supplySpawningObject = GetObjectSpawnChanceFromGameObject(supplyGameObject.GameObject);
                if (supplySpawningObject)
                    supplySpawningObject.Chance = supplyGameObject.InnerSupplySpawnChance;
            }
            gameObject.SetActive(false);
        }
        protected ObjectSpawnChanceBase GetObjectSpawnChanceFromGameObject(GameObject gameObject)
        {
            ObjectSpawnChanceBase supplySpawningObject = UtilitiesNS.Utilities.GetComponentFromGameObject<ObjectSpawnChanceBase>(gameObject);
            return supplySpawningObject;
        }
    }
}