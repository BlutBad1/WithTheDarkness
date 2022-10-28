using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShuffle : MonoBehaviour
{
    [SerializeField]
    MapData mapData;
    private void Start()
    {
        mapData.iterator = 0;
            mapData.isCreated = false;
            mapData.ShuffleLocations();
       
    }

}
