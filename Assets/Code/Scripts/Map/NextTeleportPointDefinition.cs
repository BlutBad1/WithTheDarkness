using MyConstants;
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
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == CommonConstants.PLAYER && !isActivated)
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
                mapData.ActiveLocations[mapData.LocationsIterator].MapData.SetActive(true);
                teleportTrigger.TeleportPoint = mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.TeleportPointToHere;
                teleportTrigger.TeleportPoint.position = new Vector3(teleportTrigger.TeleportPoint.position.x, teleportTrigger.TeleportPoint.position.y, teleportTrigger.TeleportPoint.position.z);
                teleportTrigger.TeleportPointToHere.position = new Vector3(teleportTrigger.TeleportPointToHere.position.x, teleportTrigger.TeleportPointToHere.position.y, teleportTrigger.TeleportPointToHere.position.z);
                teleportTrigger.ConnectedLocIndex = mapData.LocationsIterator;
                mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                mapData.ActiveLocations[mapData.LocationsIterator].EntryTeleportTrigger.ConnectedLocIndex = teleportTrigger.ThisLocIndex;
                TeleportTrigger[] teleportTriggers = mapData.ActiveLocations[mapData.LocationsIterator].MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = mapData.LocationsIterator;
                mapData.LocationsIterator++;
            }
            else
            {
                teleportTrigger.TeleportPoint = mapData.TheLastLocation.EntryTeleportTrigger.TeleportPointToHere;
                mapData.TheLastLocation.EntryTeleportTrigger.TeleportPoint = teleportTrigger.TeleportPointToHere;
                TeleportTrigger[] teleportTriggers = mapData.TheLastLocation.MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].ThisLocIndex = -2;
                mapData.TheLastLocation.EntryTeleportTrigger.ConnectedLocIndex = teleportTrigger.ThisLocIndex;
            }
        }
        private IEnumerator CreateSceneLocation()
        {
            while (teleportTrigger.dimming?.BlackScreen.color.a < 1f)
                yield return null;
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