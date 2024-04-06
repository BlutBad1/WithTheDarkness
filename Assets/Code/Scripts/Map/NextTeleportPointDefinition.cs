using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LocationManagementNS
{
    [RequireComponent(typeof(TeleportTrigger))]
    public class NextTeleportPointDefinition : MonoBehaviour
    {
        private LocationsSpawnController locationController;

        public NextTeleportPointDefinition(LocationsSpawnController locationController)
        {
            this.locationController = locationController;
        }

        private TeleportTrigger teleportTrigger;
        private bool isActivated = false;

        private void Start()
        {
            teleportTrigger = GetComponent<TeleportTrigger>();
            locationController = LocationsSpawnController.Instance;
        }
        public void DefineNextLoc()
        {
            if (!isActivated)
            {
                if (locationController.LocationsIterator < locationController.ActiveLocations.Count && locationController.ActiveLocations[locationController.LocationsIterator].LocaionType == LocaionType.Scene)
                    StartCoroutine(CreateSceneLocation());
                else
                    DefineNextLocation();
                isActivated = true;
            }
        }
        private void DefineNextLocation()
        {
            if (locationController.LocationsIterator < locationController.ActiveLocations.Count)
            {
                teleportTrigger.TeleportPoint = locationController.ActiveLocations[locationController.LocationsIterator].EntryTeleportTrigger.TeleportPointToHere;
                locationController.ActiveLocations[locationController.LocationsIterator].EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                teleportTrigger.ConnectedLocIndex = locationController.LocationsIterator;
                locationController.ActiveLocations[locationController.LocationsIterator].EntryTeleportTrigger.ConnectedLocIndex = teleportTrigger.ThisLocIndex;
                TeleportTrigger[] teleportTriggers = locationController.ActiveLocations[locationController.LocationsIterator].MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = locationController.LocationsIterator;
                locationController.LocationsIterator++;
            }
            else
            {
                Location theLastLocation = locationController.GetLocationByIndex((int)LocationIndex.TheLastLocation);
                teleportTrigger.TeleportPoint = theLastLocation.EntryTeleportTrigger.TeleportPointToHere;
                theLastLocation.EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                TeleportTrigger[] teleportTriggers = theLastLocation.MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = -2;
            }
        }
        private IEnumerator CreateSceneLocation()
        {
            AsyncOperation isLoaded = SceneManager.LoadSceneAsync(locationController.ActiveLocations[locationController.LocationsIterator].MapName, LoadSceneMode.Additive);
            while (isLoaded.progress < 0.9)
                yield return null;
            locationController.SpawnLocation(locationController.ActiveLocations[locationController.LocationsIterator]);
            while (!locationController.ActiveLocations[locationController.LocationsIterator].MapData)
                yield return null;
            DefineNextLocation();
        }
    }
}