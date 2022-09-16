using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTeleportPointDefinition : MonoBehaviour
{
   
    [SerializeField]
    MapData mapData;
    [SerializeField]
    TeleportTrigger teleportTrigger;
    bool isActiveted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isActiveted)
        {
            if (mapData.iterator < mapData.mainLocationsArr.Length)
            {
                teleportTrigger.teleportPoint = mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPointToHere;
                teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, 0.98f, teleportTrigger.teleportPoint.position.z);
                teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, 0.98f, teleportTrigger.teleportPointToHere.position.z);
                mapData.mainLocationsArr[mapData.iterator].mainTeleportTrigger.teleportPoint = teleportTrigger.teleportPointToHere;

                isActiveted = !isActiveted;
                mapData.iterator++;
            }
            else
            {
                teleportTrigger.teleportPoint = mapData.theLastLocation.teleportPointToHere;
                teleportTrigger.teleportPoint.position = new Vector3(teleportTrigger.teleportPoint.position.x, 0.98f, teleportTrigger.teleportPoint.position.z);
                teleportTrigger.teleportPointToHere.position = new Vector3(teleportTrigger.teleportPointToHere.position.x, 0.98f, teleportTrigger.teleportPointToHere.position.z);
                mapData.theLastLocation.teleportPoint = teleportTrigger.teleportPointToHere;
                isActiveted = !isActiveted;
            }

        }
    }
}
