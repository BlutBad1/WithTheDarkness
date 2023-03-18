using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LocationManagementNS
{
    public class NextTeleportPointDefinition : MonoBehaviour
    {


        MapData mapData;
        [SerializeField]
        TeleportTrigger teleportTrigger;
        bool isActiveted = false;
        private void Start()
        {
            mapData = GameObject.Find("Maps").GetComponent<MapData>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!isActiveted)
            {
                if (mapData.iterator < mapData.mainLocationsArr.Length)
                {
                    teleportTrigger.teleportPoint = mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPointToHere;
                    teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, teleportTrigger.teleportPoint.position.y, teleportTrigger.teleportPoint.position.z);
                    teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, teleportTrigger.teleportPointToHere.position.y, teleportTrigger.teleportPointToHere.position.z);
                    mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;

                    isActiveted = !isActiveted;
                    mapData.iterator++;
                }
                else
                {
                    teleportTrigger.teleportPoint = mapData.theLastLocation.teleportPointToHere;
                    mapData.theLastLocation.teleportPoint = teleportTrigger.teleportPointToHere;
                    isActiveted = !isActiveted;
                }

            }
        }
    }
}