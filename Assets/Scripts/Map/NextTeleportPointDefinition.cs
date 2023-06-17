using MyConstants;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LocationManagementNS
{
    [RequireComponent(typeof(TeleportTrigger))]
    public class NextTeleportPointDefinition : MonoBehaviour
    {
        MapData mapData;
        TeleportTrigger teleportTrigger;
        bool isActivated = false;
        private void Start()
        {
            teleportTrigger = GetComponent<TeleportTrigger>();
            mapData = MapData.instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == CommonConstants.PLAYER && !isActivated)
            {
                if (mapData.LocationsArrIterator < mapData.LocationsArr.Length && mapData.LocationsArr[mapData.LocationsArrIterator].IsScene)
                    StartCoroutine(CreateSceneLocation());
                else
                    DefineNextLocation();
                isActivated = true;
            }
        }
        void DefineNextLocation()
        {
            if (mapData.LocationsArrIterator < mapData.LocationsArr.Length)
            {
                mapData.LocationsArr[mapData.LocationsArrIterator].MapData.SetActive(true);
                teleportTrigger.teleportPoint = mapData.LocationsArr[mapData.LocationsArrIterator].EntryTeleportTrigger.teleportPointToHere;
                teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, teleportTrigger.teleportPoint.position.y, teleportTrigger.teleportPoint.position.z);
                teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, teleportTrigger.teleportPointToHere.position.y, teleportTrigger.teleportPointToHere.position.z);
                teleportTrigger.connectedLocIndex = mapData.LocationsArrIterator;
                mapData.LocationsArr[mapData.LocationsArrIterator].EntryTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;
                mapData.LocationsArr[mapData.LocationsArrIterator].EntryTeleportTrigger.connectedLocIndex = teleportTrigger.thisLocIndex;
                TeleportTrigger[] teleportTriggers = mapData.LocationsArr[mapData.LocationsArrIterator].MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].thisLocIndex = mapData.LocationsArrIterator;
                mapData.LocationsArrIterator++;
            }
            else
            {
                teleportTrigger.teleportPoint = mapData.TheLastLocation.EntryTeleportTrigger.teleportPointToHere;
                mapData.TheLastLocation.EntryTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;
                TeleportTrigger[] teleportTriggers = mapData.TheLastLocation.MapData.GetComponentsInChildren<TeleportTrigger>();
                for (int i = 0; i < teleportTriggers.Length; i++)
                    teleportTriggers[i].thisLocIndex = -2;
                mapData.TheLastLocation.EntryTeleportTrigger.connectedLocIndex = teleportTrigger.thisLocIndex;
            }
        }
        IEnumerator CreateSceneLocation()
        {
            while (teleportTrigger.dimming?.blackScreen.color.a < 1f)
                yield return null;
            AsyncOperation isLoaded = SceneManager.LoadSceneAsync(mapData.LocationsArr[mapData.LocationsArrIterator].MapName, LoadSceneMode.Additive);
            while (isLoaded.progress < 0.9)
                yield return null;
            mapData.DefineLocationElements(ref mapData.LocationsArr[mapData.LocationsArrIterator]);
            while (!mapData.LocationsArr[mapData.LocationsArrIterator].MapData)
                yield return null;
            DefineNextLocation();
        }
    }
}