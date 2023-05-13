using MyConstants;
using UnityEngine;
namespace LocationManagementNS
{
    public class NextTeleportPointDefinition : MonoBehaviour
    {
        MapData mapData;
        [SerializeField]
        TeleportTrigger teleportTrigger;
        bool isActivated = false;
        
        private void Start()
        {
            mapData = MapData.instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == CommonConstants.PLAYER && !isActivated)
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
                        teleportTriggers[i].thisLocIndex =-2;
                    mapData.TheLastLocation.EntryTeleportTrigger.connectedLocIndex = teleportTrigger.thisLocIndex;
                }
                isActivated = true;
            }
        }
    }
}