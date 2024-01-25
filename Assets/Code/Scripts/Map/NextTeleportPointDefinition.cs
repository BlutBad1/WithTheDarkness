using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LocationManagementNS
{
    [RequireComponent(typeof(TeleportTrigger))]
    public class NextTeleportPointDefinition : MonoBehaviour
    {
        private MapData mapData;
        private TeleportTrigger teleportTrigger;
        private bool isActivated = false;

        private void Start()
        {
            teleportTrigger = GetComponent<TeleportTrigger>();
            mapData = MapData.Instance;
        }
        public void DefineNextLoc()
        {
            if (!isActivated)
            {
                if (mapData.LocationsIterator < mapData.ActiveLocations.Count && mapData.ActiveLocations[mapData.LocationsIterator].LocaionType == LocaionType.Scene)
                    StartCoroutine(CreateSceneLocation());
                else
                    DefineNextLocation();
                isActivated = true;
            }
        }
        private void DefineNextLocation()
        {
            if (mapData.LocationsIterator < mapData.ActiveLocations.Count)
            {
                teleportTrigger.TeleportPoint = mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.TeleportPointToHere;
                mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                teleportTrigger.ConnectedLocIndex = mapData.LocationsIterator;
                mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.ConnectedLocIndex = teleportTrigger.ThisLocIndex;
                TeleportTrigger[] teleportTriggers = mapData.ActiveLocations[mapData.LocationsIterator].MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = mapData.LocationsIterator;
                mapData.LocationsIterator++;
            }
            else
            {
                Location theLastLocation = mapData.GetLocationByIndex((int)LocationIndex.TheLastLocation);
                teleportTrigger.TeleportPoint = theLastLocation.EntryTeleportTrigger.TeleportPointToHere;
                theLastLocation.EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                TeleportTrigger[] teleportTriggers = theLastLocation.MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = -2;
            }
        }
        private IEnumerator CreateSceneLocation()
        {
            AsyncOperation isLoaded = SceneManager.LoadSceneAsync(mapData.ActiveLocations[mapData.LocationsIterator].MapName, LoadSceneMode.Additive);
            while (isLoaded.progress < 0.9)
                yield return null;
            mapData.DefineLocationElements(mapData.ActiveLocations[mapData.LocationsIterator]);
            while (!mapData.ActiveLocations[mapData.LocationsIterator].MapData)
                yield return null;
            DefineNextLocation();
        }
    }
}