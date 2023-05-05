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
                    teleportTrigger.teleportPoint = mapData.LocationsArr[mapData.LocationsArrIterator].MainTeleportTrigger.teleportPointToHere;
                    teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, teleportTrigger.teleportPoint.position.y, teleportTrigger.teleportPoint.position.z);
                    teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, teleportTrigger.teleportPointToHere.position.y, teleportTrigger.teleportPointToHere.position.z);
                    mapData.LocationsArr[mapData.LocationsArrIterator].MainTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;
                    mapData.LocationsArrIterator++;
                }
                else
                {
                    teleportTrigger.teleportPoint = mapData.TheLastLocation.teleportPointToHere;
                    mapData.TheLastLocation.teleportPoint = teleportTrigger.teleportPointToHere;

                }
                isActivated = true;
            }
        }
    }
}