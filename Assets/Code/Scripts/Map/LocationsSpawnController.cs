using LocationsConstantsNS;
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
    public class LocationsSpawnController : MonoBehaviour
    {
        public static LocationsSpawnController Instance { get; set; }

        [Header("Settings"), SerializeField, Min(0)]
        private int minAmountOfActiveLocations = 0;
        [Header("LocationsArrays"), SerializeField]
        private Location theFirstLocation;
        [SerializeField]
        private List<Location> locations; //contains all locations 
        [SerializeField]
        private Location theLastLocation;
        [Header("Multithreading"), SerializeField]
        private bool alwaysMultithreading = false;
        [SerializeField, Tooltip("If amount of locations greater that this number, then multithreading would be enable automatically. -1 to disable.")]
        private int automaticallyEnableAfter = 800;

        private int locationsIterator = 0;
        private List<Location> activeLocations; //is using after shuffling, contains only active locations 
        private int currentMapSpawnPositionY = 40;

        public bool AlwaysMultithreading { get => alwaysMultithreading; }
        public int AutomaticallyEnableAfter { get => automaticallyEnableAfter; }
        public int LocationsIterator { get => locationsIterator; set => locationsIterator = value; }
        public List<Location> ActiveLocations { get => activeLocations; }

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
                DefineAndShuffleLocations();
            SpawnLocation(theFirstLocation, true);
            SpawnLocation(theLastLocation, theLastLocation.EntryTeleportTrigger == theFirstLocation.EntryTeleportTrigger);
        }
        public void SpawnLocation(Location loc, bool lastStatus = false)
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
            //loc.MapData.SetActive(true);
            loc.MapData.SetActive(lastStatus);
        }
        private void DefineAndShuffleLocations()
        {
            activeLocations = new List<Location>();
            if ((locations?.Count > AutomaticallyEnableAfter && AutomaticallyEnableAfter != -1) || AlwaysMultithreading)
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
                    if (locations[i].SpawnChance > new System.Random().Next() % 100 || (ActiveLocations.Count < minAmountOfActiveLocations && locations.Count - i <= minAmountOfActiveLocations))
                        AddLocationToActive(locations[i]);
                    else
                    {
                        if (locations[i].MapData && locations[i].LocaionType == LocaionType.GameObject)
                            Destroy(locations[i].MapData);
                    }
                }
            }
            activeLocations = ActiveLocations.OrderBy(x => new System.Random().Next()).ToList();
        }
        public Location GetLocationByIndex(int index)
        {
            if (index == -1)
                return theFirstLocation;
            else if (index == -2)
                return theLastLocation;
            else if (index >= 0)
                return ActiveLocations[index];
            return null;
        }
        public int GetAmountOfRemainingMaps() =>
            ActiveLocations.Count - LocationsIterator;
        private void AddLocationToActive(Location location)
        {
            if (location.LocaionType != LocaionType.Scene)
                SpawnLocation(location);
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