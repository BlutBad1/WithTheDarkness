using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Location{
    [SerializeField]
    string mapName;
    public float spawnChance;
    public TeleportTrigger mainTeleportTrigger;
   
}
public class MapData : MonoBehaviour
{
    [SerializeField]
    Location[] locations;
    [SerializeField]
    public TeleportTrigger theLastLocation;
    [HideInInspector]
    public Location[] mainLocationsArr;
    [HideInInspector]
    public int iterator = 0;
    [HideInInspector]
    public bool isCreated = false;

    public void ShuffleLocations()
    {
        mainLocationsArr = new Location[0];
       
        foreach (var map in locations)
        {
           
                if (new System.Random().Next()%100<=map.spawnChance)
                {
                    Array.Resize(ref mainLocationsArr, mainLocationsArr.Length + 1);
                     mainLocationsArr[^1] = map;
                  
                }
              
            
       
        }
        mainLocationsArr = mainLocationsArr.OrderBy(x => new System.Random().Next()).ToArray();
        isCreated = true; 
    }
}
