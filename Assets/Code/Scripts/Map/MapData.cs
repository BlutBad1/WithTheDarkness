using MyConstants;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LocationManagementNS
{
    public enum LocaionType
    {
        GameObject, Prefab, Scene
    }
    [System.Serializable]
    public class Location
    {
        public string MapName;
        public GameObject MapData;
        [Min(0)]
        public float SpawnChance;
        public TeleportTrigger EntryTeleportTrigger;
        public LocaionType LocaionType;
    }
    public class MapData : MonoBehaviour
    {
        [Header("Settings"), Min(0)]
        public int MinAmountOfActiveLocations = 0;
        [Header("LocationsArrays")]
        [SerializeField]
        public Location TheFirstLocation;
        [SerializeField]
        private List<Location> locations; //contains all locations 
        [SerializeField]
        public Location TheLastLocation;
        [HideInInspector]
        public List<Location> ActiveLocations; //is using after shuffling, contains only active locations 
        [HideInInspector]
        public int LocationsIterator = 0;
        [Header("Multithreading")]
        public bool alwaysMultithreading = false;
        [Tooltip("If amount of locations greater that this number, then multithreading would be enable automatically. -1 to disable.")]
        public int automaticallyEnableAfter = 800;
        private int currentMapSpawnPositionY = 40;
        [HideInInspector]
        public static MapData Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
                return;
            }
            if (locations != null && locations?.Count > 0)
                ShuffleLocations();
            DefineLocationElements(TheFirstLocation, true);
            DefineLocationElements(TheLastLocation, TheLastLocation.EntryTeleportTrigger == TheFirstLocation.EntryTeleportTrigger);
        }
        public void AddNewLocation(Location location)
        {
            locations.Add(location);
        }
        public void ShuffleLocations()
        {
            ActiveLocations = new List<Location>();
            if ((locations?.Count > automaticallyEnableAfter && automaticallyEnableAfter != -1) || alwaysMultithreading)
            {
                var locationsSpawnChance = new NativeArray<float>(locations.Count, Allocator.TempJob);
                var isLocationSpawned = new NativeArray<bool>(locations.Count, Allocator.TempJob);
                for (var i = 0; i < locations.Count; i++)
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
                //LocationsArr = new Location[job._isLocationSpawned.Where(c => c).Count()];
                for (int i = 0; i < job._isLocationSpawned.Length; i++)
                {
                    if (job._isLocationSpawned[i])
                        AddLocationToActive(locations[i]);
                    else if (locations[i].MapData && locations[i].LocaionType == LocaionType.GameObject)
                        Destroy(locations[i].MapData);
                }
                locationsSpawnChance.Dispose();
                isLocationSpawned.Dispose();
            }
            else
            {
                locations = locations.OrderBy(x => new System.Random().Next()).ToList();
                for (int i = 0; i < locations?.Count; i++)
                {
                    if (locations[i].SpawnChance > new System.Random().Next() % 100 || (ActiveLocations.Count < MinAmountOfActiveLocations && locations.Count - i <= MinAmountOfActiveLocations))
                        AddLocationToActive(locations[i]);
                    else
                    {
                        if (locations[i].MapData && locations[i].LocaionType == LocaionType.GameObject)
                            Destroy(locations[i].MapData);
                    }
                }
            }
            ActiveLocations = ActiveLocations.OrderBy(x => new System.Random().Next()).ToList();
        }
        public void DefineLocationElements(Location loc, bool lastStatus = false)
        {
            if (!loc.MapData)
                loc.MapData = GameObject.Find(loc.MapName);
            switch (loc.LocaionType)
            {
                case LocaionType.GameObject:
                    break;
                case LocaionType.Prefab:
                    loc.MapData = Instantiate(loc.MapData, new Vector3(0, currentMapSpawnPositionY, 0), Quaternion.identity);
                    loc.MapData.transform.parent = GameObject.Find(LocationsConstants.MAPS).transform;
                    loc.EntryTeleportTrigger = null;
                    currentMapSpawnPositionY += 40;
                    break;
                case LocaionType.Scene:
                    loc.MapData.transform.position = new Vector3(0, currentMapSpawnPositionY, 0);
                    loc.EntryTeleportTrigger = null;
                    currentMapSpawnPositionY += 40;
                    break;
                default:
                    break;
            }
            if (!loc.EntryTeleportTrigger)
            {
                loc.EntryTeleportTrigger = loc.MapData.GetComponentInChildren<EntryToLocation>().EntryTeleportTrigger;
                //loc.EntryTeleportTrigger = loc.MapData.transform.Find(LocationsConstants.ENTRY_TO_LOCATION).GetComponentInChildren<TeleportTrigger>();
            }
            loc.MapData.SetActive(true);
            loc.MapData.SetActive(lastStatus);
        }
        public Location GetLocationByIndex(int index)
        {
            if (index == -1)
                return TheFirstLocation;
            else if (index == -2)
                return TheLastLocation;
            else if (index >= 0)
                return ActiveLocations[index];
            return null;
        }
        private void AddLocationToActive(Location location)
        {
            if (location.LocaionType != LocaionType.Scene)
                DefineLocationElements(location);
            ActiveLocations.Add(location);
        }
        private struct MapShuffleJob : IJobFor
        {
            public NativeArray<bool> _isLocationSpawned;
            public NativeArray<float> _locationsSpawnChance;
            public void Execute(int i)
            {
                if (_locationsSpawnChance[i] > new System.Random().Next() % 100)
                    _isLocationSpawned[i] = false;
                else
                    _isLocationSpawned[i] = true;
            }
        }
    }
}