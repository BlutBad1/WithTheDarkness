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
        public string MapName;
        public GameObject MapData;
        [Min(0)]
        public float SpawnChance;
        public TeleportTrigger EntryTeleportTrigger;
        public bool IsScene;
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
        int currentMapSpawnPosition = 40;
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
            int it = 0;
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
                        AddMapToArray(i, ref it);
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
                        AddMapToArray(i, ref it);
                    }
                    else
                    {
                        if (locations[i].MapData && !PrefabUtility.IsPartOfAnyPrefab(locations[i].MapData) && !locations[i].IsScene)
                            Destroy(locations[i].MapData);
                    }
                }
            }
            LocationsArr = LocationsArr.OrderBy(x => new System.Random().Next()).ToArray();
        }
        void AddMapToArray(int i, ref int it)
        {
            if (!locations[i].IsScene)
                DefineLocationElements(ref locations[i]);
            LocationsArr[it] = locations[i];
            it++;
        }
        public void DefineLocationElements(ref Location loc)
        {
            if (!loc.MapData)
                loc.MapData = GameObject.Find(loc.MapName);
            if (loc.IsScene || PrefabUtility.IsPartOfAnyPrefab(loc.MapData))
            {
                if (PrefabUtility.IsPartOfAnyPrefab(loc.MapData))
                {
                    loc.MapData = Instantiate(loc.MapData, new Vector3(0, currentMapSpawnPosition, 0), Quaternion.identity);
                    loc.MapData.transform.parent = GameObject.Find(LocationsConstants.MAPS).transform;
                }
                else
                    loc.MapData.transform.position = new Vector3(0, currentMapSpawnPosition, 0);
                loc.EntryTeleportTrigger = null;
                currentMapSpawnPosition += 40;
            }
            if (!loc.EntryTeleportTrigger)
                loc.EntryTeleportTrigger = loc.MapData.transform.Find(LocationsConstants.ENTRY_TO_LOCATION).GetComponentInChildren<TeleportTrigger>();
            loc.MapData.SetActive(true);
            loc.MapData.SetActive(false);
        }
        struct MapShuffleJob : IJobFor
        {
            public NativeArray<bool> _isLocationSpawned;
            public NativeArray<float> _locationsSpawnChance;
            public void Execute(int i)
            {
                if (new System.Random().Next() % 100 >= _locationsSpawnChance[i])
                    _isLocationSpawned[i] = false;
                else
                    _isLocationSpawned[i] = true;
            }
        }
    }
}