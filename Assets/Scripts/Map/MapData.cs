using MyConstants;
using System;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

namespace LocationManagementNS
{
    [System.Serializable]
    public struct Location
    {
        [SerializeField]
        string mapName;
        public GameObject MapData;
        public float SpawnChance;
        public TeleportTrigger MainTeleportTrigger;

    }
    public class MapData : MonoBehaviour
    {
        [SerializeField]
        Location[] locations; //contains all locations 
        [SerializeField]
        public TeleportTrigger TheLastLocation;
        [HideInInspector]
        public Location[] LocationsArr; //is using after shuffling, contains only active locations 
        [HideInInspector]
        public int LocationsArrIterator = 0;
        [HideInInspector]
        static public MapData instance;
        public bool alwaysMultithreading = false;
        /*If count of locations greater that this number, then multithreading would be enable automatically.
         -1 to disable.
         */ 
        public int enableAutomaticallyIfGreater = 800;
        public void AddNewLocation(Location location)
        {   
            Array.Resize(ref locations, locations.Length + 1);
            locations[^1] = location;

        }
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            if (locations?.Length > 0)
                ShuffleLocations();
            else
                locations = new Location[0];
        }
        public void ShuffleLocations()
        {
            LocationsArr = new Location[0];
            int it = 0, mapSpawnPositionY = 30;
            if ((locations?.Length> enableAutomaticallyIfGreater && enableAutomaticallyIfGreater!=-1) || alwaysMultithreading)
            {
                var locationsSpawnChance = new NativeArray<float>(locations.Length, Allocator.TempJob);
                var isLocationSpawned = new NativeArray<bool>(locations.Length, Allocator.TempJob);
                for (var i = 0; i < locations.Length; i++)
                    locationsSpawnChance[i] = locations[i].SpawnChance;
                var job = new MapShuffleJob()
                {
                    _locationsSpawnChance = locationsSpawnChance,
                    _isLocationSpawned = isLocationSpawned,
                };
                JobHandle sheduleJobDependency = new JobHandle();
                JobHandle sheduleJobHandle = job.Schedule(locationsSpawnChance.Length, sheduleJobDependency);
                JobHandle sheduleParralelJobHandle = job.ScheduleParallel(locationsSpawnChance.Length, 64, sheduleJobHandle);
                sheduleParralelJobHandle.Complete();
                LocationsArr = new Location[job._isLocationSpawned.Where(c => c).Count()];
               
                for (int i = 0; i < job._isLocationSpawned.Length; i++)
                {
                    if (job._isLocationSpawned[i])
                    {
                        AddMapToArray(i, ref it, ref mapSpawnPositionY);
                    }
                    else
                    {
                        if (!PrefabUtility.IsPartOfAnyPrefab(locations[i].MapData))
                            Destroy(locations[i].MapData);
                    }
                }

                locationsSpawnChance.Dispose();
                isLocationSpawned.Dispose();

            }
            else
            {
               
                for (int i = 0; i < locations?.Length; i++)
                {
                    if (new System.Random().Next() % 100 <= locations[i].SpawnChance)
                    {
                        Array.Resize(ref LocationsArr, LocationsArr.Length + 1);
                        AddMapToArray(i, ref it, ref mapSpawnPositionY);
                    }
                    else
                    {
                        if (!PrefabUtility.IsPartOfAnyPrefab(locations[i].MapData))
                            Destroy(locations[i].MapData);
                    }
                }


            }
            LocationsArr = LocationsArr.OrderBy(x => new System.Random().Next()).ToArray();      
        }
        void AddMapToArray(int i, ref int it, ref int mapSpawnPositionY)
        {
            if (PrefabUtility.IsPartOfAnyPrefab(locations[i].MapData))
            {
                locations[i].MapData = Instantiate(locations[i].MapData, new Vector3(0, mapSpawnPositionY, 0), Quaternion.identity);
                mapSpawnPositionY += 30;
            }
            if (!locations[i].MainTeleportTrigger)
                locations[i].MainTeleportTrigger = locations[i].MapData.transform.Find(MapsConstants.ENTRY_TO_LOCATION).GetComponentInChildren<TeleportTrigger>();
            LocationsArr[it] = locations[i];
            locations[i].MapData.SetActive(false);
            it++;
        }
        struct MapShuffleJob : IJobFor
        {
            public NativeArray<bool> _isLocationSpawned;
            public NativeArray<float> _locationsSpawnChance;

            public void Execute(int i)
            {
                if (new System.Random().Next() % 100 <= _locationsSpawnChance[i])
                    _isLocationSpawned[i] = true;
                else
                    _isLocationSpawned[i] = false;
            }
        }
    }
}