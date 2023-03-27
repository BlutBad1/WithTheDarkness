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
            mapData = GameObject.Find(CommonConstants.MAPS).GetComponent<MapData>();
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.name == CommonConstants.PLAYER && !isActivated)
            {
                if (mapData.iterator < mapData.mainLocationsArr.Length)
                {
                    teleportTrigger.teleportPoint = mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPointToHere;
                    teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, teleportTrigger.teleportPoint.position.y, teleportTrigger.teleportPoint.position.z);
                    teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, teleportTrigger.teleportPointToHere.position.y, teleportTrigger.teleportPointToHere.position.z);
                    mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;
                    mapData.iterator++;
                }
                else
                {
                    teleportTrigger.teleportPoint = mapData.theLastLocation.teleportPointToHere;
                    mapData.theLastLocation.teleportPoint = teleportTrigger.teleportPointToHere;

                }
                isActivated = true;
            }
        }
    }
}