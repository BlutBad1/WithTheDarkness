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
        public TeleportTrigger EntryTeleportTrigger;
    }
    public class MapData : MonoBehaviour
    {
        [SerializeField]
        public Location TheFirstLocation;
        [SerializeField]
        Location[] locations; //contains all locations 
        [SerializeField]
        public Location TheLastLocation;
        [HideInInspector]
        public Location[] LocationsArr; //is using after shuffling, contains only active locations 
        [HideInInspector]
        public int LocationsArrIterator = 0;
        [HideInInspector]
        static public MapData instance;
        public bool alwaysMultithreading = false;
        [Tooltip("If amount of locations greater that this number, then multithreading would be enable automatically. -1 to disable.")]
        public int automaticallyEnableAfter = 800;
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
            TheLastLocation.MapData.SetActive(true);
            TheLastLocation.MapData.SetActive(false);
        }
        public void ShuffleLocations()
        {
            LocationsArr = new Location[0];
            int it = 0, mapSpawnPositionY = 40;
            if ((locations?.Length > automaticallyEnableAfter && automaticallyEnableAfter != -1) || alwaysMultithreading)
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
                        AddMapToArray(i, ref it, ref mapSpawnPositionY);
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
                    if (new System.Random().Next() % 100 <= locations[i].SpawnChance && locations[i].SpawnChance != 0)
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
                locations[i].MapData.transform.parent = GameObject.Find(LocationsConstants.MAPS).transform;
                locations[i].EntryTeleportTrigger = null;
                mapSpawnPositionY += 40;
            }
            if (!locations[i].EntryTeleportTrigger)
                locations[i].EntryTeleportTrigger = locations[i].MapData.transform.Find(LocationsConstants.ENTRY_TO_LOCATION).GetComponentInChildren<TeleportTrigger>();
            LocationsArr[it] = locations[i];
            locations[i].MapData.SetActive(true);
            locations[i].MapData.SetActive(false);
            it++;
        }
        struct MapShuffleJob : IJobFor
        {
            public NativeArray<bool> _isLocationSpawned;
            public NativeArray<float> _locationsSpawnChance;

            public void Execute(int i)
            {
                if (new System.Random().Next() % 100 <= _locationsSpawnChance[i] && _locationsSpawnChance[i] != 0)
                    _isLocationSpawned[i] = true;
                else
                    _isLocationSpawned[i] = false;
            }
        }
    }
}